using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.IO;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Waf.Applications;
using System.Waf.Foundation;
using Waf.InformationManager.EmailClient.Modules.Applications.SampleData;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Waf.InformationManager.EmailClient.Modules.Applications.Controllers
{
    /// <summary>
    /// Responsible for the whole module. This controller delegates the tasks to other controllers.
    /// </summary>
    [Export(typeof(IModuleController)), Export]
    internal class ModuleController : IModuleController
    {
        private const string documentPartPath = "EmailClient/Content.xml";
        
        private readonly IShellService shellService;
        private readonly IDocumentService documentService;
        private readonly INavigationService navigationService;
        private readonly EmailAccountsController emailAccountsController;
        private readonly ExportFactory<EmailFolderController> emailFolderControllerFactory;
        private readonly ExportFactory<NewEmailController> newEmailControllerFactory;
        private readonly DelegateCommand newEmailCommand;
        private readonly Lazy<DataContractSerializer> serializer;
        private readonly List<ItemCountSynchronizer> itemCountSychronizers;
        private EmailClientRoot root;
        private EmailFolderController activeEmailFolderController;
        
        
        [ImportingConstructor]
        public ModuleController(IShellService shellService, IDocumentService documentService, INavigationService navigationService,
            EmailAccountsController emailAccountsController, ExportFactory<EmailFolderController> emailFolderControllerFactory,
            ExportFactory<NewEmailController> newEmailControllerFactory)
        {
            this.shellService = shellService;
            this.documentService = documentService;
            this.navigationService = navigationService;
            this.emailAccountsController = emailAccountsController;
            this.emailFolderControllerFactory = emailFolderControllerFactory;
            this.newEmailControllerFactory = newEmailControllerFactory;
            this.newEmailCommand = new DelegateCommand(NewEmail);
            this.itemCountSychronizers = new List<ItemCountSynchronizer>();
            this.serializer = new Lazy<DataContractSerializer>(CreateDataContractSerializer);
        }


        internal EmailClientRoot Root { get { return root; } }


        public void Initialize()
        {
            using (var stream = documentService.GetStream(documentPartPath, MediaTypeNames.Text.Xml, FileMode.Open))
            {
                if (stream.Length == 0)
                {
                    this.root = new EmailClientRoot();
                    root.AddEmailAccount(SampleDataProvider.CreateEmailAccount());
                    foreach (var email in SampleDataProvider.CreateInboxEmails()) { root.Inbox.AddEmail(email); }
                    foreach (var email in SampleDataProvider.CreateSentEmails()) { root.Sent.AddEmail(email); }
                    foreach (var email in SampleDataProvider.CreateDrafts()) { root.Drafts.AddEmail(email); }
                }
                else
                {
                    root = (EmailClientRoot)serializer.Value.ReadObject(stream);
                }
            }

            emailAccountsController.Root = root;
            
            INavigationNode node = navigationService.AddNavigationNode("Inbox", ShowInbox, CloseCurrentView, 1, 1);
            itemCountSychronizers.Add(new ItemCountSynchronizer(node, root.Inbox));
            node = navigationService.AddNavigationNode("Outbox", ShowOutbox, CloseCurrentView, 1, 2);
            itemCountSychronizers.Add(new ItemCountSynchronizer(node, root.Outbox));
            node = navigationService.AddNavigationNode("Sent", ShowSentEmails, CloseCurrentView, 1, 3);
            itemCountSychronizers.Add(new ItemCountSynchronizer(node, root.Sent));
            node = navigationService.AddNavigationNode("Drafts", ShowDrafts, CloseCurrentView, 1, 4);
            itemCountSychronizers.Add(new ItemCountSynchronizer(node, root.Drafts));
            node = navigationService.AddNavigationNode("Deleted", ShowDeletedEmails, CloseCurrentView, 1, 5);
            itemCountSychronizers.Add(new ItemCountSynchronizer(node, root.Deleted));
        }

        public void Run()
        {
        }

        public void Shutdown()
        {
            using (var stream = documentService.GetStream(documentPartPath, MediaTypeNames.Text.Xml, FileMode.Create))
            {
                serializer.Value.WriteObject(stream, root);
            }
        }

        private void ShowEmails(EmailFolder emailFolder)
        {
            activeEmailFolderController = emailFolderControllerFactory.CreateExport().Value;
            activeEmailFolderController.EmailFolder = emailFolder;
            activeEmailFolderController.Initialize();
            activeEmailFolderController.Run();

            ToolBarCommand uiNewEmailCommand = new ToolBarCommand(newEmailCommand, "_New email", 
                "Creates a new email.");
            ToolBarCommand uiDeleteEmailCommand = new ToolBarCommand(activeEmailFolderController.DeleteEmailCommand, "_Delete",
                "Deletes the selected email.");
            ToolBarCommand uiEmailAccountsCommand = new ToolBarCommand(emailAccountsController.EmailAccountsCommand, "_Email accounts",
                "Opens a window that shows the email accounts.");
            shellService.AddToolBarCommands(new[] { uiNewEmailCommand, uiDeleteEmailCommand, uiEmailAccountsCommand });
        }

        private void ShowInbox()
        {
            ShowEmails(root.Inbox);
        }

        private void ShowOutbox()
        {
            ShowEmails(root.Outbox);
        }

        private void ShowSentEmails()
        {
            ShowEmails(root.Sent);
        }

        private void ShowDrafts()
        {
            ShowEmails(root.Drafts);
        }

        private void ShowDeletedEmails()
        {
            ShowEmails(root.Deleted);
        }

        private void CloseCurrentView()
        {
            shellService.ClearToolBarCommands();
            
            if (activeEmailFolderController != null)
            {
                activeEmailFolderController.Shutdown();
                activeEmailFolderController = null;
            }
        }

        private void NewEmail()
        {
            NewEmailController newEmailController = newEmailControllerFactory.CreateExport().Value;
            newEmailController.Root = root;
            newEmailController.Initialize();
            newEmailController.Run();
        }

        private DataContractSerializer CreateDataContractSerializer()
        {
            return new DataContractSerializer(typeof(EmailClientRoot), new[] { typeof(ExchangeSettings), typeof(Pop3Settings) });
        }


        private class ItemCountSynchronizer : Model
        {
            private readonly INavigationNode node;
            private readonly EmailFolder folder;
            

            public ItemCountSynchronizer(INavigationNode node, EmailFolder folder)
            {
                this.node = node;
                this.folder = folder;
                CollectionChangedEventManager.AddHandler(folder.Emails, EmailsCollectionChanged);
                UpdateItemCount();
            }


            private void EmailsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) { UpdateItemCount(); }

            private void UpdateItemCount() { node.ItemCount = folder.Emails.Count; }
        }
    }
}
