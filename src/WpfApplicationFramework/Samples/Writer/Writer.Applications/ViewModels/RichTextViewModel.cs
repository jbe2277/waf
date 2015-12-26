using System.ComponentModel;
using System.ComponentModel.Composition;
using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.Views;

namespace Waf.Writer.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class RichTextViewModel : ZoomViewModel<IRichTextView>, IEditingCommands
    {
        private readonly IShellService shellService;
        private RichTextDocument document;
        private bool isBold;
        private bool isItalic;
        private bool isUnderline;
        private bool isNumberedList;
        private bool isBulletList;
        private bool isSpellCheckEnabled;

        
        [ImportingConstructor]
        public RichTextViewModel(IRichTextView view, IShellService shellService) : base(view, shellService)
        {
            this.shellService = shellService;
        }


        public RichTextDocument Document
        {
            get { return document; }
            set { SetProperty(ref document, value); }
        }

        public bool IsBold
        {
            get { return isBold; }
            set { SetProperty(ref isBold, value); }
        }

        public bool IsItalic
        {
            get { return isItalic; }
            set { SetProperty(ref isItalic, value); }
        }
        
        public bool IsUnderline
        {
            get { return isUnderline; }
            set { SetProperty(ref isUnderline, value); }
        }

        public bool IsNumberedList
        {
            get { return isNumberedList; }
            set { SetProperty(ref isNumberedList, value); }
        }

        public bool IsBulletList
        {
            get { return isBulletList; }
            set { SetProperty(ref isBulletList, value); }
        }

        public bool IsSpellCheckAvailable { get { return true; } }
        
        public bool IsSpellCheckEnabled
        {
            get { return isSpellCheckEnabled; }
            set { SetProperty(ref isSpellCheckEnabled, value); }
        }


        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == "IsVisible")
            {
                if (IsVisible) { shellService.ActiveEditingCommands = this; }
                else { shellService.ActiveEditingCommands = null; }
            }
        }
    }
}
