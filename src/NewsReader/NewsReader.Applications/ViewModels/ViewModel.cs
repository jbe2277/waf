using System.Waf.Applications;

namespace Waf.NewsReader.Applications.ViewModels
{
    // TODO: Move to WAF
    public interface IViewModel
    {
        object View { get; }

        void Initialize();
    }

    public abstract class ViewModel<TView> : ViewModelCore<TView>, IViewModel where TView : IView
    {
        protected ViewModel(TView view) : base(view, false)
        {
        }

        public void Initialize()
        {
            if (ViewCore.DataContext != this) ViewCore.DataContext = this;
        }
    }
}
