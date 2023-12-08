using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Applications;
using System.Waf.UnitTesting.Mocks;

namespace Test.Waf.UnitTesting.Mocks
{
    [TestClass]
    public class MockViewTest
    {
        [TestMethod]
        public void MockViewContextTest()
        {
            IView v1 = new MyView();
            var vm1 = new MyViewModel(v1);
            Assert.AreSame(vm1, v1.DataContext);
#if NET6_0_OR_GREATER
            Assert.AreSame(vm1, v1.BindingContext);
#endif
        }

        private class MyViewModel : ViewModelCore<IView>
        {
            public MyViewModel(IView view) : base(view) { }
        }

        private sealed class MyView : MockView
        {
        }
    }
}
