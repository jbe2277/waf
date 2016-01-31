using System.Waf.Foundation;

namespace System.Waf.Applications
{
    /// <summary>
    /// Abstract base class for a ViewModel implementation with a simple approach to set the DataContext.
    /// </summary>
    public abstract class ViewModelCore : Model
    {
        private readonly IView view;


        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelCore"/> class and attaches itself as DataContext to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="initializeDataContext">If set to true then the DataContext of the view is set within this constructor call.</param>
        protected ViewModelCore(IView view, bool initializeDataContext)
        {
            if (view == null) { throw new ArgumentNullException(nameof(view)); }
            this.view = view;
            if (initializeDataContext)
            {
                view.DataContext = this;
            }
        }


        /// <summary>
        /// Gets the associated view.
        /// </summary>
        public object View => view;
    }
}
