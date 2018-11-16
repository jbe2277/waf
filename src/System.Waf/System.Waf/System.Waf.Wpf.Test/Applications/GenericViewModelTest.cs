using System;
using System.Waf.Applications;
using System.Waf.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting.Mocks;

namespace Test.Waf.Applications
{
    [TestClass]
    public class GenericViewModelTest
    {
        [TestMethod]
        public void GetView()
        {
            IView view = new MockView();
            var viewModel = new GenericMockViewModel(view);
            Assert.AreEqual(view, viewModel.ViewInternal);
        }

        [TestMethod]
        public void ConstructorParameter()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => new GenericMockViewModel(null));
        }


        private class GenericMockViewModel : ViewModel<IView>
        {
            public GenericMockViewModel(IView view)
                : base(view)
            {
            }

            public IView ViewInternal => ViewCore;
        }
    }
}
