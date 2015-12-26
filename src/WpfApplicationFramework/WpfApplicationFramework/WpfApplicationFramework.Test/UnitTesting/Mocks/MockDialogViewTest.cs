using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Waf.UnitTesting.Mocks;

namespace Test.Waf.UnitTesting.Mocks
{
    [TestClass]
    public class MockDialogViewTest
    {
        [TestMethod]
        public void AutomaticCleanupTestWithMef()
        {
            var container = CreateContainerAndShowView(true);
            container.Dispose();
            Assert.IsNull(TestMockDialogView.ShowDialogAction);
        }
        
        [TestMethod]
        public void AutomaticCleanupTestWithoutRegisteringUnitTesting()
        {
            var container = CreateContainerAndShowView(false);
            container.Dispose();
            Assert.IsNotNull(TestMockDialogView.ShowDialogAction);
            TestMockDialogView.ShowDialogAction = null;
        }

        private CompositionContainer CreateContainerAndShowView(bool registerUnitTesting)
        {
            AggregateCatalog catalog = new AggregateCatalog();
            if (registerUnitTesting)
            {
                catalog.Catalogs.Add(new AssemblyCatalog(typeof(MockDialogView<>).Assembly));
            }
            catalog.Catalogs.Add(new TypeCatalog(typeof(TestMockDialogView)));

            var container = new CompositionContainer(catalog, CompositionOptions.DisableSilentRejection);
            CompositionBatch batch = new CompositionBatch();
            container.Compose(batch);

            TestMockDialogView shownView = null;
            TestMockDialogView.ShowDialogAction = view =>
            {
                shownView = view;
            };

            var dialogView = container.GetExportedValue<TestMockDialogView>();
            Assert.IsNull(shownView);
            dialogView.ShowDialog(null);
            Assert.AreEqual(dialogView, shownView);

            return container;
        }

        [TestMethod]
        public void AutomaticCleanupTestWithoutMef()
        {
            TestMockDialogView shownView = null;
            TestMockDialogView.ShowDialogAction = view =>
            {
                shownView = view;
            };

            var dialogView = new TestMockDialogView();
            Assert.IsNull(shownView);
            dialogView.ShowDialog(null);
            Assert.AreEqual(dialogView, shownView);

            Assert.IsNotNull(TestMockDialogView.ShowDialogAction);
            TestMockDialogView.ShowDialogAction = null;
        }


        [Export, PartCreationPolicy(CreationPolicy.NonShared)]
        private class TestMockDialogView : MockDialogView<TestMockDialogView>
        {
        }
    }
}
