using System;
using System.ComponentModel.Composition;
using System.Waf.Foundation;

namespace Waf.BookLibrary.Library.Applications.Services
{
    [Export(typeof(IShellService)), Export]
    internal class ShellService : Model, IShellService
    {
        private object shellView;
        private object bookListView;
        private object bookView;
        private object personListView;
        private object personView;
        private bool isReportingEnabled;
        private Lazy<object> lazyReportingView;
        

        public object ShellView
        {
            get { return shellView; }
            set { SetProperty(ref shellView, value); }
        }

        public object BookListView
        {
            get { return bookListView; }
            set { SetProperty(ref bookListView, value); }
        }

        public object BookView
        {
            get { return bookView; }
            set { SetProperty(ref bookView, value); }
        }

        public object PersonListView
        {
            get { return personListView; }
            set { SetProperty(ref personListView, value); }
        }

        public object PersonView
        {
            get { return personView; }
            set { SetProperty(ref personView, value); }
        }

        public bool IsReportingEnabled
        {
            get { return isReportingEnabled; }
            set { SetProperty(ref isReportingEnabled, value); }
        }

        public Lazy<object> LazyReportingView
        {
            get { return lazyReportingView; }
            set { SetProperty(ref lazyReportingView, value); }
        }
    }
}
