using System;
using System.ComponentModel.Composition;

namespace System.Waf.UnitTesting.Mocks
{
    /// <summary>
    /// Base class for a mock dialog view implementation.
    /// </summary>
    /// <typeparam name="TMockView">The type of the concrete mock dialog view.</typeparam>
    public abstract class MockDialogView<TMockView> : MockView where TMockView : MockDialogView<TMockView>
    {
        private ContainerDisposedNotifier containerDisposedNotifier;
        

        /// <summary>
        /// Gets or sets a delegate which is called when this view should be shown.
        /// </summary>
        public static Action<TMockView> ShowDialogAction { get; set; }

        /// <summary>
        /// Gets the owner of this view.
        /// </summary>
        public object Owner { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this view is visible.
        /// </summary>
        public bool IsVisible { get; private set; }

        [Import(AllowDefault = true)]
        internal ContainerDisposedNotifier ContainerDisposedNotifier
        {
            get { return containerDisposedNotifier; }
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


        /// <summary>
        /// Shows the view. This method calls the ShowDialogAction.
        /// </summary>
        /// <param name="owner">The owner of this view.</param>
        public void ShowDialog(object owner)
        {
            this.Owner = owner;
            this.IsVisible = true;
            if (ShowDialogAction != null) { ShowDialogAction((TMockView)this); }
            this.Owner = null;
            this.IsVisible = false;
        }

        /// <summary>
        /// Close the view. This method sets IsVisible to false.
        /// </summary>
        public void Close()
        {
            this.Owner = null;
            this.IsVisible = false;
        }

        private void ContainerDisposedNotifierDisposed(object sender, EventArgs e)
        {
            containerDisposedNotifier.Disposed -= ContainerDisposedNotifierDisposed;
            ShowDialogAction = null;
        }
    }


    [Export, PartCreationPolicy(CreationPolicy.Shared)]
    internal sealed class ContainerDisposedNotifier : IDisposable
    {
        public event EventHandler Disposed;
        

        public void Dispose()
        {
            if (Disposed != null) { Disposed(this, EventArgs.Empty); }
        }
    }
}
