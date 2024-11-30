namespace Waf.BookLibrary.Library.Applications.Controllers;

/// <summary>Responsible for the database persistence of the entities.</summary>
internal interface IEntityController
{
    void Initialize();

    bool HasChanges();

    bool CanSave();

    Task<bool> SaveCore();

    void Shutdown();
}
