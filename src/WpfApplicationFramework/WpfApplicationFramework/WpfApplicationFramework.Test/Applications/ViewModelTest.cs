using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Applications;
using System.Waf.UnitTesting;

namespace Test.Waf.Applications
{
    [TestClass]
    public class ViewModelTest
    {
        [TestMethod]
        public void GetView() 
        {
            IView view = new MockView();
            MockViewModel viewModel = new MockViewModel(view);
            Assert.AreEqual(view, viewModel.View);
        }

        [TestMethod]
        public void ConstructorParameter()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => new MockViewModel(null));
        }



        private class MockViewModel : ViewModel
        {
            public MockViewModel(IView view) : base(view)
            {
            }
        }

        private class MockView : IView
        {
            public object DataContext { get; set;}
        }
    }
}
