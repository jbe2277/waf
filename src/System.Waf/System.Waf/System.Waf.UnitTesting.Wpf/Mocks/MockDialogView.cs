using System.ComponentModel.Composition;

namespace System.Waf.UnitTesting.Mocks
{
    /// <summary>Base class for a mock dialog view implementation.</summary>
    /// <typeparam name="TMockView">The type of the concrete mock dialog view.</typeparam>
    public abstract class MockDialogView<TMockView> : MockView where TMockView : MockDialogView<TMockView>
    {
        private ContainerDisposedNotifier? containerDisposedNotifier;
        
        /// <summary>Gets or sets a delegate which is called when this view should be shown.</summary>
        public static Action<TMockView>? ShowDialogAction { get; set; }

        /// <summary>Gets the owner of this view.</summary>
        public object? Owner { get; private set; }

        /// <summary>Gets a value indicating whether this view is visible.</summary>
        public bool IsVisible { get; private set; }

        [Import(AllowDefault = true)]
        internal ContainerDisposedNotifier? ContainerDisposedNotifier
        {
            get => containerDisposedNotifier;
            set
            {
                if (containerDisposedNotifier != value)
                {
                    containerDisposedNotifier = value;
                    if (containerDisposedNotifier != null)
                    {
                        containerDisposedNotifier.Disposed += ContainerDisposedNotifierDisposed;
                    }
                }
            }
        }

        /// <summary>Shows the view. This method calls the ShowDialogAction.</summary>
        /// <param name="owner">The owner of this view.</param>
        public void ShowDialog(object? owner)
        {
            Owner = owner;
            IsVisible = true;
            ShowDialogAction?.Invoke((TMockView)this);
            Owner = null;
            IsVisible = false;
        }

        /// <summary>Close the view. This method sets IsVisible to false.</summary>
        public void Close()
        {
            Owner = null;
            IsVisible = false;
        }

        private void ContainerDisposedNotifierDisposed(object? sender, EventArgs e)
        {
            containerDisposedNotifier!.Disposed -= ContainerDisposedNotifierDisposed;
            ShowDialogAction = null;
        }
    }

    /// <summary>Base class for a mock dialog view implementation.</summary>
    /// <typeparam name="TMockView">The type of the concrete mock dialog view.</typeparam>
    /// <typeparam name="TViewModel">Type of the associated view model.</typeparam>
    public abstract class MockDialogView<TMockView, TViewModel> : MockDialogView<TMockView> where TMockView : MockDialogView<TMockView> where TViewModel : class
    {
        /// <summary>The view model.</summary>
        public TViewModel ViewModel => (TViewModel)(DataContext ?? throw new InvalidOperationException("ViewModel is not set."));
    }

    [Export, PartCreationPolicy(CreationPolicy.Shared)]
    internal sealed class ContainerDisposedNotifier : IDisposable
    {
        public event EventHandler? Disposed;
        
        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }
}
