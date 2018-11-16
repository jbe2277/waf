using System;
using System.Waf.Applications.Services;
using System.Waf.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting.Mocks;

namespace Test.Waf.Applications.Services
{
    [TestClass]
    public class MessageServiceExtensionsTest
    {
        [TestMethod]
        public void ShowMessageTest()
        {
            var messageService = new MockMessageService();

            var message = "Hello World";
            AssertHelper.ExpectedException<ArgumentNullException>(() => MessageServiceExtensions.ShowMessage(null, message));
            AssertHelper.ExpectedException<ArgumentNullException>(() => MessageServiceExtensions.ShowWarning(null, message));
            AssertHelper.ExpectedException<ArgumentNullException>(() => MessageServiceExtensions.ShowError(null, message));
            
            messageService.ShowMessage(message);
            Assert.AreEqual(MessageType.Message, messageService.MessageType);
            Assert.AreEqual(message, messageService.Message);

            messageService.Clear();
            messageService.ShowWarning(message);
            Assert.AreEqual(MessageType.Warning, messageService.MessageType);
            Assert.AreEqual(message, messageService.Message);

            messageService.Clear();
            messageService.ShowError(message);
            Assert.AreEqual(MessageType.Error, messageService.MessageType);
            Assert.AreEqual(message, messageService.Message);
        }

        [TestMethod]
        public void ShowQuestionTest()
        {
            var messageService = new MockMessageService();

            var message = "Hello World";
            AssertHelper.ExpectedException<ArgumentNullException>(() => MessageServiceExtensions.ShowQuestion(null, message));
            AssertHelper.ExpectedException<ArgumentNullException>(() => MessageServiceExtensions.ShowYesNoQuestion(null, message));

            bool showQuestionCalled = false;
            messageService.ShowQuestionAction = m =>
            {
                showQuestionCalled = true;
                Assert.AreEqual(message, m);
                return true;
            };
            Assert.IsTrue(messageService.ShowQuestion(message) == true);
            Assert.IsTrue(showQuestionCalled);

            showQuestionCalled = false;
            messageService.Clear();
            messageService.ShowYesNoQuestionAction = m =>
            {
                showQuestionCalled = true;
                Assert.AreEqual(message, m);
                return true;
            };
            Assert.IsTrue(messageService.ShowYesNoQuestion(message));
            Assert.IsTrue(showQuestionCalled);
        }
    }
}
