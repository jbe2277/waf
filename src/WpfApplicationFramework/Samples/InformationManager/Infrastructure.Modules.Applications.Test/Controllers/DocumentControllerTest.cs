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
        [TestMethod]
        public void GetStreamTest()
        {
            var controller = Container.GetExportedValue<DocumentController>();
            controller.Initialize();

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

            controller.Shutdown();
        }


        [DataContract]
        private class Data
        {
            [DataMember]
            public string Name { get; set; }
        }
    }
}
