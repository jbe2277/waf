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
            var viewModel1 = new MockViewModel(view1, false);
            Assert.AreEqual(view1, viewModel1.View);
            Assert.IsNull(view1.DataContext);
            int initCount = 0;
            viewModel1.InitializeStub = () => initCount++;
            viewModel1.Initialize();
            Assert.AreEqual(viewModel1, view1.DataContext);
            Assert.AreEqual(1, initCount);
            viewModel1.Initialize();  // Second call does not initialize twice
            Assert.AreEqual(viewModel1, view1.DataContext);
            Assert.AreEqual(1, initCount);
            view1.DataContext = new object();
            viewModel1.Initialize();  // But it sets every time the DataContext
            Assert.AreEqual(viewModel1, view1.DataContext);
            Assert.AreEqual(1, initCount);

            IView view2 = new MockView();
            var viewModel2 = new MockViewModel(view2, true);
            Assert.AreEqual(view2, viewModel2.View);
            Assert.AreEqual(viewModel2, view2.DataContext);

            IView view3 = new MockView();
            var viewModel3 = new MockViewModel(view3);
            Assert.AreEqual(view3, viewModel3.View);
            Assert.AreEqual(viewModel3, view3.DataContext);
        }

        [TestMethod]
        public void ConstructorParameter()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => new MockViewModel(null!, true));
            AssertHelper.ExpectedException<ArgumentNullException>(() => new MockViewModel(null!));
        }
        

        private class MockViewModel : ViewModelCore<IView>
        {
            public MockViewModel(IView view) : base(view)
            {
            }

            public MockViewModel(IView view, bool initializeDataContext) : base(view, initializeDataContext)
            {
            }

            public Action? InitializeStub { get; set; }

            protected override void OnInitialize() => InitializeStub?.Invoke();            
        }
    }
}
