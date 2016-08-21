using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Waf.Applications;
using System.Waf.UnitTesting;
using System.Waf.UnitTesting.Mocks;

namespace Test.Waf.Applications
{
    [TestClass]
    public class ViewModelCoreTest
    {
        [TestMethod]
        public void GetViewAndVerifyDataContext()
        {
            IView view1 = new MockView();
            MockViewModel viewModel1 = new MockViewModel(view1, false);
            Assert.AreEqual(view1, viewModel1.View);
            Assert.IsNull(view1.DataContext);

            IView view2 = new MockView();
            MockViewModel viewModel2 = new MockViewModel(view2, true);
            Assert.AreEqual(view2, viewModel2.View);
            Assert.AreEqual(viewModel2, view2.DataContext);
        }

        [TestMethod]
        public void ConstructorParameter()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => new MockViewModel(null, true));
        }
        

        private class MockViewModel : ViewModelCore<IView>
        {
            public MockViewModel(IView view, bool initializeDataContext) : base(view, initializeDataContext)
            {
            }
        }
    }
}
