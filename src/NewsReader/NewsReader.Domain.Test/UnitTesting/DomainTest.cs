using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace Test.NewsReader.Domain.UnitTesting
{
    [TestClass]
    public abstract class DomainTest
    {
        public TestContext TestContext { get; set; }


        [TestInitialize]
        public void Initialize()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            OnInitialize();
        }

        [TestCleanup]
        public void Cleanup()
        {
            OnCleanup();
        }

        protected virtual void OnInitialize() { }

        protected virtual void OnCleanup() { }
    }
}
