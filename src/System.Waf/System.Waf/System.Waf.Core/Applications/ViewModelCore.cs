using System.Waf.Foundation;

namespace System.Waf.Applications
{
    /// <summary>Abstract base class for a ViewModel implementation with a simple approach to set the DataContext.</summary>
    /// <typeparam name="TView">The type of the view. Do provide an interface as type and not the concrete type itself.</typeparam>
    public abstract class ViewModelCore<TView> : Model, IViewModelCore where TView : IView
    {
        private bool isInitialized;

        /// <summary>Initializes a new instance of the <see cref="ViewModelCore{TView}"/> class and attaches itself as DataContext to the view.</summary>
        /// <param name="view">The view.</param>
        protected ViewModelCore(TView view) : this(view, true)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ViewModelCore{TView}"/> class and attaches itself as DataContext to the view.</summary>
        /// <param name="view">The view.</param>
        /// <param name="initializeDataContext">If set to true then the DataContext of the view is set within this constructor call.</param>
        protected ViewModelCore(TView view, bool initializeDataContext)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            ViewCore = view;
            if (initializeDataContext)
            {
                view.DataContext = this;
            }
        }

        /// <summary>Gets the associated view.</summary>
        public object View => ViewCore;

        /// <summary>Gets the associated view as specified view type.</summary>
        /// <remarks>Use this property in a ViewModel class to avoid casting.</remarks>
        protected TView ViewCore { get; }

        /// <summary>Initialize the view model and set the DataContext of the view.</summary>
        public void Initialize()
        {
            if (!isInitialized)
            {
                isInitialized = true;
                OnInitialize();
            }
            if (ViewCore.DataContext != this) ViewCore.DataContext = this;
        }

        /// <summary>Override this method to initialize the view model.</summary>
        protected virtual void OnInitialize() { }
    }
}
