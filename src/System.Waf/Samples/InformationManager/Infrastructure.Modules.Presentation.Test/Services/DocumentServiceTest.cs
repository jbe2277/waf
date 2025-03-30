using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Net.Mime;
using System.Runtime.Serialization;
using Test.InformationManager.Infrastructure.Modules.Applications;
using Test.InformationManager.Infrastructure.Modules.Applications.Services;
using Waf.InformationManager.Infrastructure.Modules.Presentation.Services;

namespace Test.InformationManager.Infrastructure.Modules.Presentation;

[TestClass]
public class DocumentServiceTest : InfrastructureTest
{
    private DocumentService service = null!;

    public TestContext TestContext { get; init; }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        var systemService = Get<MockSystemService>();
        systemService.DataDirectory = TestContext.TestRunDirectory ?? throw new InvalidOperationException("TestRunDirectory is not set.");
        service = new DocumentService(systemService);
        if (File.Exists(service.PackagePath)) File.Delete(service.PackagePath);
    }

    [TestMethod]
    public void GetStreamTest()
    {
        using (var stream = service.GetStream("Test/MyFile.xml", MediaTypeNames.Text.Xml, FileMode.Open))
        {
            Assert.AreEqual(0, stream.Length);  // File does not exist
        }

        var serializer = new DataContractSerializer(typeof(Data));
        using (var stream = service.GetStream("Test/MyFile.xml", MediaTypeNames.Text.Xml, FileMode.Create))
        {
            serializer.WriteObject(stream, new Data() { Name = "Bill" });
        }

        using (var stream = service.GetStream("Test/MyFile.xml", MediaTypeNames.Text.Xml, FileMode.Open))
        {
            var data = (Data)serializer.ReadObject(stream)!;
            Assert.AreEqual("Bill", data.Name);
        }
    }


    [DataContract]
    private class Data
    {
        [DataMember]
        public string? Name { get; set; }
    }
}
