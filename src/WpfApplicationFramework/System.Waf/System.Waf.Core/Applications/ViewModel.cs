using System.Threading;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace System.Waf.Applications
{
    /// <summary>
    /// Abstract base class for a ViewModel implementation.
    /// </summary>
    public abstract class ViewModel : Model
    {
        private readonly IView view;


        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class and attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        protected ViewModel(IView view)
        {
            if (view == null) { throw new ArgumentNullException(nameof(view)); }
            this.view = view;
            TaskScheduler uiTaskScheduler = SynchronizationContext.Current != null ? TaskScheduler.FromCurrentSynchronizationContext() : null;


            // Check if the code is running within the UI application model
            if (uiTaskScheduler != null)
            {
                // Set DataContext of the view has to be delayed so that the ViewModel can initialize the internal data (e.g. Commands)
                // before the view starts with DataBinding.
                Task.Factory.StartNew(() => this.view.DataContext = this, CancellationToken.None, TaskCreationOptions.DenyChildAttach, uiTaskScheduler);
            }
            else
            {
                // When the code runs outside of the UI application model then we set the DataContext immediately.
                view.DataContext = this;
            }
        }


        /// <summary>
        /// Gets the associated view.
        /// </summary>
        public object View => view;
    }
}
