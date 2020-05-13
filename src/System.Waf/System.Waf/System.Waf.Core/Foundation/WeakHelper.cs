using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System.Windows.Input;
using S = System;

namespace System.Waf.Foundation
{
    public interface IWeakEventProxy
    {
        void Remove();
    }

    public static class WeakHelper
    {
        public static class EventHandler
        {
            public static IWeakEventProxy Add<TSource>(TSource source, S.EventHandler targetHandler, Action<TSource, S.EventHandler> subscribe, Action<TSource, S.EventHandler> unsubscribe)
                where TSource : class
            {
                return new WeakEventProxy<TSource>(source, targetHandler, subscribe, unsubscribe);
            }

            private sealed class WeakEventProxy<TSource> : IWeakEventProxy
                where TSource : class
            {
                private readonly TSource source;
                private readonly WeakReference<S.EventHandler> weakTargetHandler;
                private Action<TSource, S.EventHandler>? unsubscribe;

                public WeakEventProxy(TSource source, S.EventHandler targetHandler, Action<TSource, S.EventHandler> subscribe, Action<TSource, S.EventHandler> unsubscribe)
                {
                    this.source = source;
                    weakTargetHandler = new WeakReference<S.EventHandler>(targetHandler);
                    this.unsubscribe = unsubscribe;
                    subscribe(source, ProxyHandler);
                }

                public void Remove()
                {
                    Interlocked.Exchange(ref unsubscribe, null)?.Invoke(source, ProxyHandler);
                }

                private void ProxyHandler(object sender, EventArgs e)
                {
                    if (weakTargetHandler.TryGetTarget(out var targetHandler)) targetHandler(sender, e);
                    else Remove();
                }
            }
        }

        public static class EventHandler<TArgs> where TArgs : EventArgs
        {
            public static IWeakEventProxy Add<TSource>(TSource source, S.EventHandler<TArgs> targetHandler, Action<TSource, S.EventHandler<TArgs>> subscribe, Action<TSource, S.EventHandler<TArgs>> unsubscribe)
                where TSource : class
            {
                return new WeakEventProxy<TSource, TArgs>(source, targetHandler, subscribe, unsubscribe);
            }

            private sealed class WeakEventProxy<TSource, TArgs> : IWeakEventProxy 
                where TSource : class 
                where TArgs : EventArgs
            {
                private readonly TSource source;
                private readonly WeakReference<S.EventHandler<TArgs>> weakTargetHandler;
                private Action<TSource, S.EventHandler<TArgs>>? unsubscribe;

                public WeakEventProxy(TSource source, S.EventHandler<TArgs> targetHandler, Action<TSource, S.EventHandler<TArgs>> subscribe, Action<TSource, S.EventHandler<TArgs>> unsubscribe)
                {
                    this.source = source;
                    weakTargetHandler = new WeakReference<S.EventHandler<TArgs>>(targetHandler);
                    this.unsubscribe = unsubscribe;
                    subscribe(source, ProxyHandler);
                }

                public void Remove()
                {
                    Interlocked.Exchange(ref unsubscribe, null)?.Invoke(source, ProxyHandler);
                }

                private void ProxyHandler(object sender, TArgs e)
                {
                    if (weakTargetHandler.TryGetTarget(out var targetHandler)) targetHandler(sender, e);
                    else Remove();
                }
            }
        }

        public static class PropertyChanged
        {
            public static IWeakEventProxy Add(INotifyPropertyChanged source, PropertyChangedEventHandler targetHandler)
            {
                return new WeakEventProxy(source, targetHandler, (s, h) => s.PropertyChanged +=h, (s, h) => s.PropertyChanged -= h);
            }

            private sealed class WeakEventProxy : IWeakEventProxy
            {
                private readonly INotifyPropertyChanged source;
                private readonly WeakReference<PropertyChangedEventHandler> weakTargetHandler;
                private Action<INotifyPropertyChanged, PropertyChangedEventHandler>? unsubscribe;

                public WeakEventProxy(INotifyPropertyChanged source, PropertyChangedEventHandler targetHandler, Action<INotifyPropertyChanged, PropertyChangedEventHandler> subscribe, Action<INotifyPropertyChanged, PropertyChangedEventHandler> unsubscribe)
                {
                    this.source = source;
                    weakTargetHandler = new WeakReference<PropertyChangedEventHandler>(targetHandler);
                    this.unsubscribe = unsubscribe;
                    subscribe(source, ProxyHandler);
                }

                public void Remove()
                {
                    Interlocked.Exchange(ref unsubscribe, null)?.Invoke(source, ProxyHandler);
                }

                private void ProxyHandler(object sender, PropertyChangedEventArgs e)
                {
                    if (weakTargetHandler.TryGetTarget(out var targetHandler)) targetHandler(sender, e);
                    else Remove();
                }
            }
        }

        public static class CollectionChanged
        {
            public static IWeakEventProxy Add(INotifyCollectionChanged source, NotifyCollectionChangedEventHandler targetHandler)
            {
                return new WeakEventProxy(source, targetHandler, (s, h) => s.CollectionChanged += h, (s, h) => s.CollectionChanged -= h);
            }

            private sealed class WeakEventProxy : IWeakEventProxy
            {
                private readonly INotifyCollectionChanged source;
                private readonly WeakReference<NotifyCollectionChangedEventHandler> weakTargetHandler;
                private Action<INotifyCollectionChanged, NotifyCollectionChangedEventHandler>? unsubscribe;

                public WeakEventProxy(INotifyCollectionChanged source, NotifyCollectionChangedEventHandler targetHandler, Action<INotifyCollectionChanged, NotifyCollectionChangedEventHandler> subscribe, Action<INotifyCollectionChanged, NotifyCollectionChangedEventHandler> unsubscribe)
                {
                    this.source = source;
                    weakTargetHandler = new WeakReference<NotifyCollectionChangedEventHandler>(targetHandler);
                    this.unsubscribe = unsubscribe;
                    subscribe(source, ProxyHandler);
                }

                public void Remove()
                {
                    Interlocked.Exchange(ref unsubscribe, null)?.Invoke(source, ProxyHandler);
                }

                private void ProxyHandler(object sender, NotifyCollectionChangedEventArgs e)
                {
                    if (weakTargetHandler.TryGetTarget(out var targetHandler)) targetHandler(sender, e);
                    else Remove();
                }
            }
        }

        public static class CanExecuteChanged
        {
            public static IWeakEventProxy Add(ICommand source, S.EventHandler targetHandler)
            {
                return EventHandler.Add(source, targetHandler, (s, h) => s.CanExecuteChanged += h, (s, h) => s.CanExecuteChanged -= h);
            }
        }

        public static class ErrorsChanged
        {
            public static IWeakEventProxy Add(INotifyDataErrorInfo source, S.EventHandler<DataErrorsChangedEventArgs> targetHandler)
            {
                return EventHandler<DataErrorsChangedEventArgs>.Add(source, targetHandler, (s, h) => s.ErrorsChanged += h, (s, h) => s.ErrorsChanged -= h);
            }
        }
    }
}
