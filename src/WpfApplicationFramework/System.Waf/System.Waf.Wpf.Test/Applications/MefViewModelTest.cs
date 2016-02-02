using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Waf.Applications;
using System.Waf.UnitTesting.Mocks;

namespace Test.Waf.Applications
{
    [TestClass]
    public class MefViewModelTest
    {
        [TestMethod]
        public void OnImportsSatisfiedTest()
        {
            var container = new CompositionContainer(new AssemblyCatalog(typeof(MefViewModel).Assembly), CompositionOptions.DisableSilentRejection);
            container.Compose(new CompositionBatch());
            var viewModel = container.GetExportedValue<MefViewModel>();
            Assert.AreEqual(viewModel, ((IView)viewModel.View).DataContext);
            Assert.AreEqual(1, viewModel.OnImportsSatisfiedCalled);
            container.Dispose();
        }

        [Export]
        private class MefViewModel : ViewModel<IMefView>
        {
            [ImportingConstructor]
            public MefViewModel(IMefView view) : base(view)
            {
            }

            public int OnImportsSatisfiedCalled { get; private set; }

            protected override void OnImportsSatisfied()
            {
                base.OnImportsSatisfied();
                OnImportsSatisfiedCalled++;
            }
        }

        private interface IMefView : IView
        {
        }

        [Export(typeof(IMefView))]
        private class MefView : MockView, IMefView
        {
        }
    }
}
