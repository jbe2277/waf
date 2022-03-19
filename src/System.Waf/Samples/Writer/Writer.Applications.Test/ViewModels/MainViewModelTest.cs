using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.ViewModels;

namespace Test.Writer.Applications.ViewModels;

[TestClass]
public class MainViewModelTest : ApplicationsTest
{
    protected override void OnInitialize()
    {
        base.OnInitialize();
        InitializePrintController();
    }

    [TestMethod]
    public void DocumentViewTest()
    {
        var mainViewModel = Get<MainViewModel>();

        Assert.IsFalse(mainViewModel.DocumentViews.Any());
        Assert.IsNull(mainViewModel.ActiveDocumentView);

        mainViewModel.FileService.NewCommand.Execute(null);

        Assert.AreEqual(mainViewModel.DocumentViews.Single(), mainViewModel.ActiveDocumentView);

        mainViewModel.FileService.NewCommand.Execute(null);

        Assert.AreEqual(mainViewModel.DocumentViews[^1], mainViewModel.ActiveDocumentView);
        Assert.AreEqual(2, mainViewModel.DocumentViews.Count);

        mainViewModel.ActiveDocumentView = mainViewModel.DocumentViews[0];
        mainViewModel.FileService.CloseCommand.Execute(null);

        Assert.AreEqual(1, mainViewModel.DocumentViews.Count);
        Assert.IsNull(mainViewModel.ActiveDocumentView);

        mainViewModel.ActiveDocumentView = mainViewModel.DocumentViews.Single();
        mainViewModel.FileService.CloseCommand.Execute(null);

        Assert.IsFalse(mainViewModel.DocumentViews.Any());
        Assert.IsNull(mainViewModel.ActiveDocumentView);
    }

    [TestMethod]
    public void UpdateShellServiceDocumentNameTest()
    {
        var fileService = Get<IFileService>();
        var shellService = Get<IShellService>();

        fileService.NewCommand.Execute(null);
        fileService.ActiveDocument = fileService.Documents[0];
        AssertHelper.PropertyChangedEvent(shellService, x => x.DocumentName, () => fileService.ActiveDocument.FileName = "Unit Test.rtf");
        Assert.AreEqual("Unit Test.rtf", shellService.DocumentName);
    }
}
