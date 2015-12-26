using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Waf.UnitTesting;
using Waf.Writer.Applications.Controllers;
using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Services;

namespace Test.Writer.Applications.Controllers
{
    [TestClass]
    public class DocumentControllerTest
    {
        [TestMethod]
        public void DocumentControllerConstructorTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => new TestDocumentController(null));
        }


        private class TestDocumentController : DocumentController
        {
            public TestDocumentController(IFileService fileService) : base(fileService)
            {
            }
            
            protected override void OnDocumentAdded(IDocument document)
            {
                throw new NotSupportedException();
            }

            protected override void OnDocumentRemoved(IDocument document)
            {
                throw new NotSupportedException();
            }

            protected override void OnActiveDocumentChanged(IDocument activeDocument)
            {
                throw new NotSupportedException();
            }
        }
    }
}
