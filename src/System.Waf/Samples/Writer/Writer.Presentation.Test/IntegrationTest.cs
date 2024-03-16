using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting.Mocks;
using Test.Writer.Applications.Services;

namespace Test.Writer.Presentation;

[TestClass]
public class IntegrationTest : PresentationTest
{
    [TestMethod]
    public void OpenDocumentViaCommandLineIntegrationTest()
    {
        var systemService = Get<MockSystemService>();
        systemService.DocumentFileName = "2i0501fh-89f1-4197-a318-d5241135f4f6.rtf";

        var messageService = Get<MockMessageService>();

        // Call open with a fileName that doesn't exist
        messageService.Clear();

        StartApp();

        Assert.AreEqual(MessageType.Error, messageService.MessageType);
        Assert.IsFalse(string.IsNullOrEmpty(messageService.Message));
    }
}