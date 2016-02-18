using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace Test.InformationManager.Common.Domain
{
    [TestClass]
    public abstract class DomainTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            OnTestInitialize();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            OnTestCleanup();
        }

        protected virtual void OnTestInitialize() { }

        protected virtual void OnTestCleanup() { }
    }
}
