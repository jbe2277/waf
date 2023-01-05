using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Applications;

namespace Test.Waf.Applications
{
    [TestClass]
    public class ViewTest
    {
        [TestMethod]
        public void ViewImplementationTest()
        {
            var v1 = new MyView1();
            var vm1 = new MyViewModel(v1);
            Assert.AreSame(vm1, v1.DataContext);
#if NET6_0_OR_GREATER
            Assert.IsNull(((IView)v1).BindingContext);

            var v2 = new MyView2();
            var vm2 = new MyViewModel(v2);
            Assert.AreSame(vm2, ((IView)v2).DataContext);
            Assert.AreSame(vm2, v2.BindingContext);

            var v3 = new MyView3();
            var vm3 = new MyViewModel(v3);
            Assert.IsNull(((IView)v3).DataContext);
            Assert.IsNull(((IView)v3).BindingContext);
#endif
        }

        private class MyViewModel : ViewModelCore<IView>
        {
            public MyViewModel(IView view) : base(view) { }
        }

        private sealed class MyView1 : IView
        {
            public object? DataContext { get; set; }
        }

#if NET6_0_OR_GREATER
        private sealed class MyView2 : IView
        {
            public object? BindingContext { get; set; }
        }

        private sealed class MyView3 : IView { }
#endif
    }
}
