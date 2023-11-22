using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System.Windows.Input;
using S = System;

namespace System.Waf.Foundation
{
    /// <summary>Proxy created by adding a weak event handler.</summary>
    public interface IWeakEventProxy
    {
        /// <summary>Remove the weak event handler. This stops receiving event notifications.</summary>
        void Remove();
    }

    /// <summary>Supports listening to events via a weak reference.<br/>
    /// Listening for events can lead to memory leaks. The publisher has a strong reference to event subscribers. Therefore, subscribers are kept in memory as long as the publisher lives.
    /// The memory leak might occur when the subscriber does not know when to stop listening or if the subscriber should listen as long it lives.
    /// This class provides a solution to prevent the memory leak in such cases. It creates a proxy object which holds the subscriber via a week reference.
    /// </summary>
    /// <remarks>
    /// Performance considerations:
    /// - Using this weak event implementation is slightly slower than listening for the event via C# default mechanism.
    /// - The implementation does not use Reflection and no code emitting. Thus, it is very fast.
    /// - A small proxy object is created for adding a weak event handler. This object is kept by the publisher in memory. 
    /// - The proxy object removes itself from the publisher when the Remove method on the proxy is called or when the publisher raises its event and the subscriber is not alive anymore.
    /// </remarks>
    public static class WeakEvent
    {
        private static readonly WeakEventHandlerTable weakTable = new WeakEventHandlerTable();

        /// <summary>Supports listening to EventHandler events via a weak reference.</summary>
        public static class EventHandler
        {
            /// <summary>Add an event handler which is kept only by a weak reference.</summary>
            /// <typeparam name="TSource">The type of the source.</typeparam>
            /// <param name="source">The source (publisher).</param>
            /// <param name="targetHandler">The target event handler of the subscriber.</param>
            /// <param name="subscribe">Action which subscribes the event. E.g. (s, h) => s.MyEvent += h</param>
            /// <param name="unsubscribe">Action which unsubscribes the event. E.g. (s, h) => s.MyEvent -= h</param>
            /// <returns>The created proxy object.</returns>
            public static IWeakEventProxy Add<TSource>(TSource source, S.EventHandler targetHandler, Action<TSource, S.EventHandler> subscribe, Action<TSource, S.EventHandler> unsubscribe)
                where TSource : class
            {
                if (source is null) throw new ArgumentNullException(nameof(source));
                if (targetHandler is null) throw new ArgumentNullException(nameof(targetHandler));
                if (subscribe is null) throw new ArgumentNullException(nameof(subscribe));
                if (unsubscribe is null) throw new ArgumentNullException(nameof(unsubscribe));
                return new WeakEventProxy<TSource>(source, targetHandler, subscribe, unsubscribe);
            }

            private sealed class WeakEventProxy<TSource> : IWeakEventProxy
                where TSource : class
            {
                private readonly WeakReference<TSource> source;
                private readonly WeakReference<S.EventHandler> weakTargetHandler;
                private Action<TSource, S.EventHandler>? unsubscribe;

                public WeakEventProxy(TSource source, S.EventHandler targetHandler, Action<TSource, S.EventHandler> subscribe, Action<TSource, S.EventHandler> unsubscribe)
                {
                    this.source = new WeakReference<TSource>(source);
                    weakTargetHandler = new WeakReference<S.EventHandler>(targetHandler);
                    this.unsubscribe = unsubscribe;
                    subscribe(source, ProxyHandler);
                    weakTable.Add(targetHandler);
                }

                public void Remove()
                {
                    if (RemoveCore() && weakTargetHandler.TryGetTarget(out var targetHandler)) weakTable.Remove(targetHandler);
                }

                private bool RemoveCore()
                {
                    var unsub = Interlocked.Exchange(ref unsubscribe, null);
                    if (unsub is null) return false;
                    if (source.TryGetTarget(out var src)) unsub.Invoke(src, ProxyHandler);
                    return true;
                }

                private void ProxyHandler(object? sender, EventArgs e)
                {
                    if (weakTargetHandler.TryGetTarget(out var targetHandler)) targetHandler(sender, e);
                    else RemoveCore();
                }
            }
        }

        /// <summary>Supports listening to EventHandler{TArgs} events via a weak reference.</summary>
        /// <typeparam name="TArgs">The type of the event data generated by the event.</typeparam>
        public static class EventHandler<TArgs> where TArgs : EventArgs
        {
            /// <summary>Add an event handler which is kept only by a weak reference.</summary>
            /// <typeparam name="TSource">The type of the source.</typeparam>
            /// <param name="source">The source (publisher).</param>
            /// <param name="targetHandler">The target event handler of the subscriber.</param>
            /// <param name="subscribe">Action which subscribes the event. E.g. (s, h) => s.MyEvent += h</param>
            /// <param name="unsubscribe">Action which unsubscribes the event. E.g. (s, h) => s.MyEvent -= h</param>
            /// <returns>The created proxy object.</returns>
            public static IWeakEventProxy Add<TSource>(TSource source, S.EventHandler<TArgs> targetHandler, Action<TSource, S.EventHandler<TArgs>> subscribe, Action<TSource, S.EventHandler<TArgs>> unsubscribe)
                where TSource : class
            {
                if (source is null) throw new ArgumentNullException(nameof(source));
                if (targetHandler is null) throw new ArgumentNullException(nameof(targetHandler));
                if (subscribe is null) throw new ArgumentNullException(nameof(subscribe));
                if (unsubscribe is null) throw new ArgumentNullException(nameof(unsubscribe));
                return new WeakEventProxy<TSource>(source, targetHandler, subscribe, unsubscribe);
            }

            private sealed class WeakEventProxy<TSource> : IWeakEventProxy
                where TSource : class
            {
                private readonly WeakReference<TSource> source;
                private readonly WeakReference<S.EventHandler<TArgs>> weakTargetHandler;
                private Action<TSource, S.EventHandler<TArgs>>? unsubscribe;

                public WeakEventProxy(TSource source, S.EventHandler<TArgs> targetHandler, Action<TSource, S.EventHandler<TArgs>> subscribe, Action<TSource, S.EventHandler<TArgs>> unsubscribe)
                {
                    this.source = new WeakReference<TSource>(source);
                    weakTargetHandler = new WeakReference<S.EventHandler<TArgs>>(targetHandler);
                    this.unsubscribe = unsubscribe;
                    subscribe(source, ProxyHandler);
                    weakTable.Add(targetHandler);
                }

                public void Remove()
                {
                    if (RemoveCore() && weakTargetHandler.TryGetTarget(out var targetHandler)) weakTable.Remove(targetHandler);
                }

                private bool RemoveCore()
                {
                    var unsub = Interlocked.Exchange(ref unsubscribe, null);
                    if (unsub is null) return false;
                    if (source.TryGetTarget(out var src)) unsub.Invoke(src, ProxyHandler);
                    return true;
                }

                private void ProxyHandler(object? sender, TArgs e)
                {
                    if (weakTargetHandler.TryGetTarget(out var targetHandler)) targetHandler(sender, e);
                    else RemoveCore();
                }
            }
        }

        /// <summary>Supports listening to static EventHandler events via a weak reference.</summary>
        public static class StaticEventHandler
        {
            /// <summary>Add an event handler to a static event which is kept only by a weak reference.</summary>
            /// <param name="targetHandler">The target event handler of the subscriber.</param>
            /// <param name="subscribe">Action which subscribes the static event. E.g. h => MyClass.MyStaticEvent += h</param>
            /// <param name="unsubscribe">Action which unsubscribes the static event. E.g. h => MyClass.MyStaticEvent -= h</param>
            /// <returns>The created proxy object.</returns>
            public static IWeakEventProxy Add(S.EventHandler targetHandler, Action<S.EventHandler> subscribe, Action<S.EventHandler> unsubscribe)
            {
                if (targetHandler is null) throw new ArgumentNullException(nameof(targetHandler));
                if (subscribe is null) throw new ArgumentNullException(nameof(subscribe));
                if (unsubscribe is null) throw new ArgumentNullException(nameof(unsubscribe));
                return new WeakEventProxy(targetHandler, subscribe, unsubscribe);
            }

            private sealed class WeakEventProxy : IWeakEventProxy
            {
                private readonly WeakReference<S.EventHandler> weakTargetHandler;
                private Action<S.EventHandler>? unsubscribe;

                public WeakEventProxy(S.EventHandler targetHandler, Action<S.EventHandler> subscribe, Action<S.EventHandler> unsubscribe)
                {
                    weakTargetHandler = new WeakReference<S.EventHandler>(targetHandler);
                    this.unsubscribe = unsubscribe;
                    subscribe(ProxyHandler);
                    weakTable.Add(targetHandler);
                }

                public void Remove()
                {
                    if (RemoveCore() && weakTargetHandler.TryGetTarget(out var targetHandler)) weakTable.Remove(targetHandler);
                }

                private bool RemoveCore()
                {
                    var unsub = Interlocked.Exchange(ref unsubscribe, null);
                    if (unsub is null) return false;
                    unsub.Invoke(ProxyHandler);
                    return true;
                }

                private void ProxyHandler(object? sender, EventArgs e)
                {
                    if (weakTargetHandler.TryGetTarget(out var targetHandler)) targetHandler(sender, e);
                    else RemoveCore();
                }
            }
        }

        /// <summary>Supports listening to static EventHandler{TArgs} events via a weak reference.</summary>
        /// <typeparam name="TArgs">The type of the event data generated by the event.</typeparam>
        public static class StaticEventHandler<TArgs>
        {
            /// <summary>Add an event handler to a static event which is kept only by a weak reference.</summary>
            /// <param name="targetHandler">The target event handler of the subscriber.</param>
            /// <param name="subscribe">Action which subscribes the static event. E.g. h => MyClass.MyStaticEvent += h</param>
            /// <param name="unsubscribe">Action which unsubscribes the static event. E.g. h => MyClass.MyStaticEvent -= h</param>
            /// <returns>The created proxy object.</returns>
            public static IWeakEventProxy Add(S.EventHandler<TArgs> targetHandler, Action<S.EventHandler<TArgs>> subscribe, Action<S.EventHandler<TArgs>> unsubscribe)
            {
                if (targetHandler is null) throw new ArgumentNullException(nameof(targetHandler));
                if (subscribe is null) throw new ArgumentNullException(nameof(subscribe));
                if (unsubscribe is null) throw new ArgumentNullException(nameof(unsubscribe));
                return new WeakEventProxy(targetHandler, subscribe, unsubscribe);
            }

            private sealed class WeakEventProxy : IWeakEventProxy
            {
                private readonly WeakReference<S.EventHandler<TArgs>> weakTargetHandler;
                private Action<S.EventHandler<TArgs>>? unsubscribe;

                public WeakEventProxy(S.EventHandler<TArgs> targetHandler, Action<S.EventHandler<TArgs>> subscribe, Action<S.EventHandler<TArgs>> unsubscribe)
                {
                    weakTargetHandler = new WeakReference<S.EventHandler<TArgs>>(targetHandler);
                    this.unsubscribe = unsubscribe;
                    subscribe(ProxyHandler);
                    weakTable.Add(targetHandler);
                }

                public void Remove()
                {
                    if (RemoveCore() && weakTargetHandler.TryGetTarget(out var targetHandler)) weakTable.Remove(targetHandler);
                }

                private bool RemoveCore()
                {
                    var unsub = Interlocked.Exchange(ref unsubscribe, null);
                    if (unsub is null) return false;
                    unsub.Invoke(ProxyHandler);
                    return true;
                }

                private void ProxyHandler(object? sender, TArgs e)
                {
                    if (weakTargetHandler.TryGetTarget(out var targetHandler)) targetHandler(sender, e);
                    else RemoveCore();
                }
            }
        }

        /// <summary>Supports listening to INotifyPropertyChanged.PropertyChanged events via a weak reference.</summary>
        public static class PropertyChanged
        {
            /// <summary>Add an event handler which is kept only by a weak reference.</summary>
            /// <param name="source">The source (publisher).</param>
            /// <param name="targetHandler">The target event handler of the subscriber.</param>
            /// <returns>The created proxy object.</returns>
            public static IWeakEventProxy Add(INotifyPropertyChanged source, PropertyChangedEventHandler targetHandler)
            {
                if (source is null) throw new ArgumentNullException(nameof(source));
                if (targetHandler is null) throw new ArgumentNullException(nameof(targetHandler));
                return new WeakEventProxy(source, targetHandler, (s, h) => s.PropertyChanged += h, (s, h) => s.PropertyChanged -= h);
            }

            private sealed class WeakEventProxy : IWeakEventProxy
            {
                private readonly WeakReference<INotifyPropertyChanged> source;
                private readonly WeakReference<PropertyChangedEventHandler> weakTargetHandler;
                private Action<INotifyPropertyChanged, PropertyChangedEventHandler>? unsubscribe;

                public WeakEventProxy(INotifyPropertyChanged source, PropertyChangedEventHandler targetHandler, Action<INotifyPropertyChanged, PropertyChangedEventHandler> subscribe, Action<INotifyPropertyChanged, PropertyChangedEventHandler> unsubscribe)
                {
                    this.source = new WeakReference<INotifyPropertyChanged>(source);
                    weakTargetHandler = new WeakReference<PropertyChangedEventHandler>(targetHandler);
                    this.unsubscribe = unsubscribe;
                    subscribe(source, ProxyHandler);
                    weakTable.Add(targetHandler);
                }

                public void Remove()
                {
                    if (RemoveCore() && weakTargetHandler.TryGetTarget(out var targetHandler)) weakTable.Remove(targetHandler);
                }

                private bool RemoveCore()
                {
                    var unsub = Interlocked.Exchange(ref unsubscribe, null);
                    if (unsub is null) return false;
                    if (source.TryGetTarget(out var src)) unsub.Invoke(src, ProxyHandler);
                    return true;
                }

                private void ProxyHandler(object? sender, PropertyChangedEventArgs e)
                {
                    if (weakTargetHandler.TryGetTarget(out var targetHandler)) targetHandler(sender, e);
                    else RemoveCore();
                }
            }
        }

        /// <summary>Supports listening to INotifyCollectionChanged.CollectionChanged events via a weak reference.</summary>
        public static class CollectionChanged
        {
            /// <summary>Add an event handler which is kept only by a weak reference.</summary>
            /// <param name="source">The source (publisher).</param>
            /// <param name="targetHandler">The target event handler of the subscriber.</param>
            /// <returns>The created proxy object.</returns>
            public static IWeakEventProxy Add(INotifyCollectionChanged source, NotifyCollectionChangedEventHandler targetHandler)
            {
                if (source is null) throw new ArgumentNullException(nameof(source));
                if (targetHandler is null) throw new ArgumentNullException(nameof(targetHandler));
                return new WeakEventProxy(source, targetHandler, (s, h) => s.CollectionChanged += h, (s, h) => s.CollectionChanged -= h);
            }

            private sealed class WeakEventProxy : IWeakEventProxy
            {
                private readonly WeakReference<INotifyCollectionChanged> source;
                private readonly WeakReference<NotifyCollectionChangedEventHandler> weakTargetHandler;
                private Action<INotifyCollectionChanged, NotifyCollectionChangedEventHandler>? unsubscribe;

                public WeakEventProxy(INotifyCollectionChanged source, NotifyCollectionChangedEventHandler targetHandler, Action<INotifyCollectionChanged, NotifyCollectionChangedEventHandler> subscribe, Action<INotifyCollectionChanged, NotifyCollectionChangedEventHandler> unsubscribe)
                {
                    this.source = new WeakReference<INotifyCollectionChanged>(source);
                    weakTargetHandler = new WeakReference<NotifyCollectionChangedEventHandler>(targetHandler);
                    this.unsubscribe = unsubscribe;
                    subscribe(source, ProxyHandler);
                    weakTable.Add(targetHandler);
                }

                public void Remove()
                {
                    if (RemoveCore() && weakTargetHandler.TryGetTarget(out var targetHandler)) weakTable.Remove(targetHandler);
                }

                private bool RemoveCore()
                {
                    var unsub = Interlocked.Exchange(ref unsubscribe, null);
                    if (unsub is null) return false;
                    if (source.TryGetTarget(out var src)) unsub.Invoke(src, ProxyHandler);
                    return true;
                }

                private void ProxyHandler(object? sender, NotifyCollectionChangedEventArgs e)
                {
                    if (weakTargetHandler.TryGetTarget(out var targetHandler)) targetHandler(sender, e);
                    else RemoveCore();
                }
            }
        }

        /// <summary>Supports listening to ICommand.CanExecuteChanged events via a weak reference.</summary>
        public static class CanExecuteChanged
        {
            /// <summary>Add an event handler which is kept only by a weak reference.</summary>
            /// <param name="source">The source (publisher).</param>
            /// <param name="targetHandler">The target event handler of the subscriber.</param>
            /// <returns>The created proxy object.</returns>
            public static IWeakEventProxy Add(ICommand source, S.EventHandler targetHandler)
            {
                return EventHandler.Add(source, targetHandler, (s, h) => s.CanExecuteChanged += h, (s, h) => s.CanExecuteChanged -= h);
            }
        }

        /// <summary>Supports listening to INotifyDataErrorInfo.ErrorsChanged events via a weak reference.</summary>
        public static class ErrorsChanged
        {
            /// <summary>Add an event handler which is kept only by a weak reference.</summary>
            /// <param name="source">The source (publisher).</param>
            /// <param name="targetHandler">The target event handler of the subscriber.</param>
            /// <returns>The created proxy object.</returns>
            public static IWeakEventProxy Add(INotifyDataErrorInfo source, S.EventHandler<DataErrorsChangedEventArgs> targetHandler)
            {
                return EventHandler<DataErrorsChangedEventArgs>.Add(source, targetHandler, (s, h) => s.ErrorsChanged += h, (s, h) => s.ErrorsChanged -= h);
            }
        }

        /// <summary>Remove the weak event handler if the passed proxy is not null. And set the proxy to null.</summary>
        /// <param name="proxy">The weak event proxy.</param>
        public static void TryRemove(ref IWeakEventProxy? proxy)
        {
            proxy?.Remove();
            proxy = null;
        }
    }
}
