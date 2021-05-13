using System;
using System.Waf.Applications.Services;
using System.Waf.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting.Mocks;
using System.Globalization;

namespace Test.Waf.Applications.Services
{
    [TestClass]
    public class MessageServiceExtensionsTest
    {
        [TestMethod]
        public void ShowMessageTest()
        {
            var messageService = new MockMessageService();

            var view = new object();
            var message = "Hello World";
            var format = "Result: {0}";
            AssertHelper.ExpectedException<ArgumentNullException>(() => MessageServiceExtensions.ShowMessage(null!, message));
            AssertHelper.ExpectedException<ArgumentNullException>(() => MessageServiceExtensions.ShowMessage(null!, null, message));
            AssertHelper.ExpectedException<ArgumentNullException>(() => MessageServiceExtensions.ShowWarning(null!, message));
            AssertHelper.ExpectedException<ArgumentNullException>(() => MessageServiceExtensions.ShowWarning(null!, null, message));
            AssertHelper.ExpectedException<ArgumentNullException>(() => MessageServiceExtensions.ShowError(null!, message));
            AssertHelper.ExpectedException<ArgumentNullException>(() => MessageServiceExtensions.ShowError(null!, null, message));

            messageService.ShowMessage(message);
            Assert.AreEqual(MessageType.Message, messageService.MessageType);
            Assert.AreEqual(message, messageService.Message);

            messageService.Clear();
            messageService.ShowMessage(view, format, 42);
            Assert.AreSame(view, messageService.Owner);
            Assert.AreEqual(MessageType.Message, messageService.MessageType);
            Assert.AreEqual(string.Format(CultureInfo.CurrentCulture, format, 42), messageService.Message);

            messageService.Clear();
            messageService.ShowWarning(message);
            Assert.AreEqual(MessageType.Warning, messageService.MessageType);
            Assert.AreEqual(message, messageService.Message);

            messageService.Clear();
            messageService.ShowWarning(view, format, 42);
            Assert.AreSame(view, messageService.Owner);
            Assert.AreEqual(MessageType.Warning, messageService.MessageType);
            Assert.AreEqual(string.Format(CultureInfo.CurrentCulture, format, 42), messageService.Message);

            messageService.Clear();
            messageService.ShowError(message);
            Assert.AreEqual(MessageType.Error, messageService.MessageType);
            Assert.AreEqual(message, messageService.Message);

            messageService.Clear();
            messageService.ShowError(view, format, 42);
            Assert.AreSame(view, messageService.Owner);
            Assert.AreEqual(MessageType.Error, messageService.MessageType);
            Assert.AreEqual(string.Format(CultureInfo.CurrentCulture, format, 42), messageService.Message);
        }

        [TestMethod]
        public void ShowQuestionTest()
        {
            var messageService = new MockMessageService();

            var view = new object();
            var message = "Hello World";
            var format = "Result: {0}";
            AssertHelper.ExpectedException<ArgumentNullException>(() => MessageServiceExtensions.ShowQuestion(null!, message));
            AssertHelper.ExpectedException<ArgumentNullException>(() => MessageServiceExtensions.ShowQuestion(null!, null, message));
            AssertHelper.ExpectedException<ArgumentNullException>(() => MessageServiceExtensions.ShowYesNoQuestion(null!, message));
            AssertHelper.ExpectedException<ArgumentNullException>(() => MessageServiceExtensions.ShowYesNoQuestion(null!, null, message));

            bool showQuestionCalled = false;
            messageService.ShowQuestionStub = (_, m) =>
            {
                showQuestionCalled = true;
                Assert.AreEqual(message, m);
                return true;
            };
            Assert.IsTrue(messageService.ShowQuestion(message) == true);
            Assert.IsTrue(showQuestionCalled);

            showQuestionCalled = false;
            messageService.ShowQuestionStub = (o, m) =>
            {
                showQuestionCalled = true;
                Assert.AreSame(view, o);
                Assert.AreEqual(string.Format(CultureInfo.CurrentCulture, format, 42), m);
                return true;
            };
            Assert.IsTrue(messageService.ShowQuestion(view, format, 42) == true);
            Assert.IsTrue(showQuestionCalled);

            showQuestionCalled = false;
            messageService.Clear();
            messageService.ShowYesNoQuestionStub = (_, m) =>
            {
                showQuestionCalled = true;
                Assert.AreEqual(message, m);
                return true;
            };
            Assert.IsTrue(messageService.ShowYesNoQuestion(message));
            Assert.IsTrue(showQuestionCalled);

            showQuestionCalled = false;
            messageService.Clear();
            messageService.ShowYesNoQuestionStub = (o, m) =>
            {
                showQuestionCalled = true;
                Assert.AreSame(view, o);
                Assert.AreEqual(string.Format(CultureInfo.CurrentCulture, format, 42), m);
                return true;
            };
            Assert.IsTrue(messageService.ShowYesNoQuestion(view, format, 42));
            Assert.IsTrue(showQuestionCalled);
        }
    }
}
