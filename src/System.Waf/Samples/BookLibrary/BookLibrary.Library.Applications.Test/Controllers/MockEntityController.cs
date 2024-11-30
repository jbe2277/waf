using Waf.BookLibrary.Library.Applications.Controllers;
using System.ComponentModel.Composition;

namespace Test.BookLibrary.Library.Applications.Controllers;

[Export]
public class MockEntityController : IEntityController
{
    public bool InitializeCalled { get; set; }

    public bool ShutdownCalled { get; set; }

    public bool HasChangesResult { get; set; }

    public bool CanSaveResult { get; set; } = true;

    public Func<Task<bool>>? SaveCoreStub { get; set; }

    public bool SaveCoreCalled { get; set; }

    public void Initialize() => InitializeCalled = true;

    public bool HasChanges() => HasChangesResult;

    public bool CanSave() => CanSaveResult;

    public Task<bool> SaveCore() => SaveCoreStub?.Invoke() ?? Task.FromResult(true);

    public void Shutdown() => ShutdownCalled = true;
}
