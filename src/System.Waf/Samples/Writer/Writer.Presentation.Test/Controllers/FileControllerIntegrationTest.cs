using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting.Mocks;
using Waf.Writer.Applications.Services;

namespace Test.Writer.Presentation.Controllers;

[TestClass]
public class FileControllerIntegrationTest : PresentationTest
{
    [TestMethod]
    public void OpenDocumentViaCommandLineIntegrationTest()
    {
        var fileService = Get<IFileService>();

        Assert.IsFalse(fileService.Documents.Any());
        Assert.IsNull(fileService.ActiveDocument);

        var messageService = Get<MockMessageService>();

        // Call open with a fileName that doesn't exist
        messageService.Clear();
        fileService.OpenCommand.Execute("2i0501fh-89f1-4197-a318-d5241135f4f6.rtf");
        Assert.AreEqual(MessageType.Error, messageService.MessageType);
        Assert.IsFalse(string.IsNullOrEmpty(messageService.Message));
    }
}