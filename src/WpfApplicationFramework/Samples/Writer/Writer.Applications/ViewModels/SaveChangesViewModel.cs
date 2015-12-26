using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows.Input;
using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Views;

namespace Waf.Writer.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SaveChangesViewModel : ViewModel<ISaveChangesView>
    {
        private readonly DelegateCommand yesCommand;
        private readonly DelegateCommand noCommand;
        private IReadOnlyList<IDocument> documents;
        private bool? dialogResult;

        
        [ImportingConstructor]
        public SaveChangesViewModel(ISaveChangesView view) : base(view)
        {
            this.yesCommand = new DelegateCommand(() => Close(true));
            this.noCommand = new DelegateCommand(() => Close(false));
        }


        public static string Title { get { return ApplicationInfo.ProductName; } }

        public ICommand YesCommand { get { return yesCommand; } }

        public ICommand NoCommand { get { return noCommand; } }

        public IReadOnlyList<IDocument> Documents
        {
            get { return documents; }
            set { SetProperty(ref documents, value); }
        }


        public bool? ShowDialog(object owner)
        {
            ViewCore.ShowDialog(owner);
            return dialogResult;
        }

        private void Close(bool? dialogResult)
        {
            this.dialogResult = dialogResult;
            ViewCore.Close();
        }
    }
}
