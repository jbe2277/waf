using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Net.Mime;
using System.Runtime.Serialization;
using Waf.InformationManager.Infrastructure.Modules.Applications.Controllers;

namespace Test.InformationManager.Infrastructure.Modules.Applications.Controllers
{
    [TestClass]
    public class DocumentControllerTest : InfrastructureTest
    {
        private DocumentController controller;

        protected override void OnTestInitialize()
        {
            base.OnTestInitialize();
            controller = Container.GetExportedValue<DocumentController>();
            if (File.Exists(controller.PackagePath)) File.Delete(controller.PackagePath);
            controller.Initialize();
        }

        protected override void OnTestCleanup()
        {
            controller.Shutdown();
            base.OnTestCleanup();
        }

        [TestMethod]
        public void GetStreamTest()
        {
            using (var stream = controller.GetStream("Test/MyFile.xml", MediaTypeNames.Text.Xml, FileMode.Open))
            {
                Assert.AreEqual(0, stream.Length);  // File does not exist
            }

            var serializer = new DataContractSerializer(typeof(Data));
            using (var stream = controller.GetStream("Test/MyFile.xml", MediaTypeNames.Text.Xml, FileMode.Create))
            {
                serializer.WriteObject(stream, new Data() { Name = "Bill" });
            }

            using (var stream = controller.GetStream("Test/MyFile.xml", MediaTypeNames.Text.Xml, FileMode.Open))
            {
                var data = (Data)serializer.ReadObject(stream);
                Assert.AreEqual("Bill", data.Name);
            }
        }


        [DataContract]
        private class Data
        {
            [DataMember]
            public string Name { get; set; }
        }
    }
}
