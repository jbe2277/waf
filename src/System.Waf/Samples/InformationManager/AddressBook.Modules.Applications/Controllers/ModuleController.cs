using System.Net.Mime;
using System.Runtime.Serialization;
using System.Waf.Applications;
using Waf.InformationManager.AddressBook.Interfaces.Applications;
using Waf.InformationManager.AddressBook.Interfaces.Domain;
using Waf.InformationManager.AddressBook.Modules.Applications.SampleData;
using Waf.InformationManager.AddressBook.Modules.Domain;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Waf.InformationManager.AddressBook.Modules.Applications.Controllers;

/// <summary>Responsible for the whole module. This controller delegates the tasks to other controllers.</summary>
internal class ModuleController(IShellService shellService, IDocumentService documentService, INavigationService navigationService,
    Func<ContactController> contactControllerFactory, Func<SelectContactController> selectContactControllerFactory) 
    : IModuleController, IAddressBookService
{
    private const string documentPartPath = "AddressBook/Content.xml";

    private readonly IShellService shellService = shellService;
    private readonly IDocumentService documentService = documentService;
    private readonly INavigationService navigationService = navigationService;
    private readonly Func<ContactController> contactControllerFactory = contactControllerFactory;
    private readonly Func<SelectContactController> selectContactControllerFactory = selectContactControllerFactory;
    private readonly Lazy<DataContractSerializer> serializer = new(CreateDataContractSerializer);
    private ContactController? activeContactController;

    internal AddressBookRoot Root { get; private set; } = null!;

    public void Initialize()
    {
        using (var stream = documentService.GetStream(documentPartPath, MediaTypeNames.Text.Xml, FileMode.Open))
        {
            if (stream.Length == 0)
            {
                Root = new();
                foreach (var x in SampleDataProvider.CreateContacts()) Root.AddContact(x);
            }
            else Root = (AddressBookRoot)serializer.Value.ReadObject(stream)!;
        }
        navigationService.AddNavigationNode("ContactsNode", "Contacts", ShowAddressBook, CloseAddressBook, 2, 1);
    }

    public void Run() { }

    public void Shutdown()
    {
        using var stream = documentService.GetStream(documentPartPath, MediaTypeNames.Text.Xml, FileMode.Create);
        serializer.Value.WriteObject(stream, Root);
    }

    public ContactDto? ShowSelectContactView(object ownerView)
    {
        var selectContactController = selectContactControllerFactory();
        selectContactController.OwnerView = ownerView;
        selectContactController.Root = Root;
        selectContactController.Initialize();
        selectContactController.Run();
        selectContactController.Shutdown();
        return selectContactController.SelectedContact.ToDto();
    }

    private void ShowAddressBook()
    {
        activeContactController = contactControllerFactory();
        activeContactController.Root = Root;
        activeContactController.Initialize();
        activeContactController.Run();

        var uiNewContactCommand = new ToolBarCommand("NewContactCommand", activeContactController.NewContactCommand, "_New contact", "Creates a new contact.");
        var uiDeleteCommand = new ToolBarCommand("DeleteCommand", activeContactController.DeleteContactCommand, "_Delete", "Deletes the selected contact.");
        shellService.AddToolBarCommands([ uiNewContactCommand, uiDeleteCommand ]);
    }

    private void CloseAddressBook()
    {
        shellService.ClearToolBarCommands();
        activeContactController?.Shutdown();
        activeContactController = null;
    }

    private static DataContractSerializer CreateDataContractSerializer() => new(typeof(AddressBookRoot));
}
