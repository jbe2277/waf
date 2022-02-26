using Waf.BookLibrary.Library.Applications.Controllers;
using System.ComponentModel.Composition;

namespace Test.BookLibrary.Library.Applications.Controllers;

[Export]
public class MockEntityController : IEntityController
{
    public bool InitializeCalled { get; set; }

    public bool ShutdownCalled { get; set; }

    public bool HasChangesResult { get; set; }

    public bool CanSaveResult { get; set; }

    public bool SaveResult { get; set; }

    public bool SaveCalled { get; set; }

    public MockEntityController() => CanSaveResult = true;

    public void Initialize() => InitializeCalled = true;

    public bool HasChanges() => HasChangesResult;

    public bool CanSave() => CanSaveResult;

    public bool Save()
    {
        SaveCalled = true;
        return SaveResult;
    }

    public void Shutdown() => ShutdownCalled = true;
}
