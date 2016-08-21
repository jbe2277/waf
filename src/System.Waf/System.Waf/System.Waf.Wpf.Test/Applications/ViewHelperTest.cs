using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Applications;
using System.Windows.Threading;
using System.Waf.UnitTesting;
using System.Threading;

namespace Test.Waf.Applications
{
    [TestClass]
    public class ViewHelperTest
    {
        [TestMethod]
        public void GetViewModelTest()
        {
            MockView view = new MockView();
            MockViewModel viewModel = new MockViewModel(view);

            Assert.AreEqual(viewModel, view.GetViewModel<MockViewModel>());

            AssertHelper.ExpectedException<ArgumentNullException>(() => ViewHelper.GetViewModel<MockViewModel>(null));
        }

        [TestMethod]
        public void GetViewModelWithDispatcherTest()
        {
            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());
            
            MockView view = new MockView();
            MockViewModel viewModel = new MockViewModel(view);

            Assert.AreEqual(viewModel, view.GetViewModel<MockViewModel>());
        }


        private class MockView : IView
        {
            public object DataContext { get; set; }
        }

        private class MockViewModel : ViewModel
        {
            public MockViewModel(IView view) : base(view)
            {
            }
        }
    }
}
