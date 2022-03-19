using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Applications;
using System.Waf.UnitTesting;
using System.Waf.UnitTesting.Mocks;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.ViewModels;

namespace Test.Writer.Applications.ViewModels;

[TestClass]
public class ZoomViewModelTest : ApplicationsTest
{
    private IShellService shellService = null!;
    private MockZoomViewModel zoomViewModel = null!;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        shellService = Get<IShellService>();
        zoomViewModel = new MockZoomViewModel(new MockView(), shellService);
    }

    [TestMethod]
    public void DefaultZoomsTest()
    {
        AssertHelper.SequenceEqual(new[] { "200%", "150%", "125%", "100%", "75%", "50%" }, zoomViewModel.DefaultZooms);
    }

    [TestMethod]
    public void ZoomTest()
    {
        Assert.AreEqual(1d, zoomViewModel.Zoom);

        zoomViewModel.Zoom = 0.75;
        Assert.AreEqual(0.75, zoomViewModel.Zoom);

        zoomViewModel.ZoomInCommand.Execute(null);
        Assert.AreEqual(0.80, zoomViewModel.Zoom);

        zoomViewModel.ZoomOutCommand.Execute(null);
        Assert.AreEqual(0.70, zoomViewModel.Zoom);

        AssertHelper.CanExecuteChangedEvent(zoomViewModel.ZoomOutCommand, () => zoomViewModel.Zoom = 0.1);
        Assert.AreEqual(0.2, zoomViewModel.Zoom);
        Assert.IsFalse(zoomViewModel.ZoomOutCommand.CanExecute(null));

        AssertHelper.CanExecuteChangedEvent(zoomViewModel.ZoomInCommand, () => zoomViewModel.Zoom = 25);
        Assert.AreEqual(16, zoomViewModel.Zoom);
        Assert.IsFalse(zoomViewModel.ZoomInCommand.CanExecute(null));
    }

    [TestMethod]
    public void FitToWidthCommandTest()
    {
        zoomViewModel.FitToWidthCommand.Execute(null);
        Assert.IsTrue(zoomViewModel.FitToWidthCalled);
    }

    [TestMethod]
    public void SyncWithShellServiceTest()
    {
        Assert.AreEqual(0, shellService.ActiveZoomCommands.DefaultZooms.Count);
        AssertHelper.PropertyChangedEvent(shellService, x => x.ActiveZoomCommands, () => zoomViewModel.IsVisible = true);
        Assert.IsTrue(zoomViewModel.IsVisible);
        Assert.AreEqual(zoomViewModel, shellService.ActiveZoomCommands);
        zoomViewModel.IsVisible = false;
        Assert.AreNotEqual(zoomViewModel, shellService.ActiveZoomCommands);
    }


    private class MockZoomViewModel : ZoomViewModel<IView>
    {
        public MockZoomViewModel(IView view, IShellService shellService) : base(view, shellService)
        {
        }

        public bool FitToWidthCalled { get; set; }

        protected override void FitToWidthCore()
        {
            base.FitToWidthCore();
            FitToWidthCalled = true;
        }
    }
}
