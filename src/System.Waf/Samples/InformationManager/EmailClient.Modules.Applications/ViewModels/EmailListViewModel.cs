using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows.Input;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class EmailListViewModel : ViewModel<IEmailListView>
    {
        private Email? selectedEmail;
        private string filterText = "";

        [ImportingConstructor]
        public EmailListViewModel(IEmailListView view) : base(view)
        {
        }

        public IReadOnlyList<Email> Emails { get; set; } = null!;

        public Email? SelectedEmail
        {
            get => selectedEmail;
            set => SetProperty(ref selectedEmail, value);
        }

        public ICommand DeleteEmailCommand { get; set; } = DelegateCommand.DisabledCommand;

        public string FilterText
        {
            get => filterText;
            set => SetProperty(ref filterText, value);
        }

        public void FocusItem()
        {
            ViewCore.FocusItem();
        }

        public bool Filter(Email email)
        {
            return string.IsNullOrEmpty(filterText)
                || (!string.IsNullOrEmpty(email.Title) && email.Title.Contains(FilterText, StringComparison.CurrentCultureIgnoreCase))
                || (!string.IsNullOrEmpty(email.From) && email.From.Contains(FilterText, StringComparison.CurrentCultureIgnoreCase))
                || email.To.Any(x => !string.IsNullOrEmpty(x) && x.Contains(FilterText, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
