using System.ComponentModel.Composition;
using System.Threading;
using System.Windows.Threading;

namespace System.Waf.Applications
{
    /// <summary>Abstract base class for a ViewModel implementation.</summary>
    /// <typeparam name="TView">The type of the view. Do provide an interface as type and not the concrete type itself.</typeparam>
    public abstract class ViewModel<TView> : ViewModelCore<TView>, IPartImportsSatisfiedNotification where TView : IView
    {
        /// <summary>Initializes a new instance of the <see cref="ViewModel{TView}"/> class and attaches itself as DataContext to the view.</summary>
        /// <param name="view">The view.</param>
        protected ViewModel(TView view) : base(view, false)
        {
            // Check if the code is running within the WPF application model
            if (SynchronizationContext.Current is DispatcherSynchronizationContext)
            {
                // Set DataContext of the view has to be delayed so that the ViewModel can initialize the internal data (e.g. Commands)
                // before the view starts with DataBinding.
                Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate ()
                {
                    ViewCore.DataContext = this;
                });
            }
            else
            {
                // When the code runs outside of the WPF application model then we set the DataContext immediately.
                view.DataContext = this;
            }
        }

        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            ViewCore.DataContext = this;
            OnImportsSatisfied();
        }

        /// <summary>
        /// Called when a part's imports have been satisfied and it is safe to use.
        /// This method is called when MEF (Managed Extensibility Framework) is used.
        /// </summary>
        protected virtual void OnImportsSatisfied()
        {
        }
    }
}
