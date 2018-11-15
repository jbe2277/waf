using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using System.Waf.UnitTesting;
using Test.InformationManager.Common.Domain;

namespace Test.InformationManager.EmailClient.Modules.Domain.Emails
{
    [TestClass]
    public class EmailTest : DomainTest
    {
        [TestMethod]
        public void PropertiesTest()
        {
            var email = new Email();

            AssertHelper.PropertyChangedEvent(email, x => x.EmailType, () => email.EmailType = EmailType.Sent);
            Assert.AreEqual(EmailType.Sent, email.EmailType);

            AssertHelper.PropertyChangedEvent(email, x => x.Title, () => email.Title = "Duis nunc");
            Assert.AreEqual("Duis nunc", email.Title);

            AssertHelper.PropertyChangedEvent(email, x => x.From, () => email.From = "user@adventure-works.com");
            Assert.AreEqual("user@adventure-works.com", email.From);

            var to = new[] { "harry@example.com", "admin@adventure-works.com" };
            AssertHelper.PropertyChangedEvent(email, x => x.To, () => email.To = to);
            Assert.AreEqual(to, email.To);
            AssertHelper.ExpectedException<ArgumentNullException>(() => email.To = null);
            Assert.AreEqual(to, email.To);

            var cc = new[] { "user-2@fabrikam.com" };
            AssertHelper.PropertyChangedEvent(email, x => x.CC, () => email.CC = cc);
            Assert.AreEqual(cc, email.CC);
            AssertHelper.ExpectedException<ArgumentNullException>(() => email.CC = null);
            Assert.AreEqual(cc, email.CC);

            var bcc = new[] { "someone@example.com" };
            AssertHelper.PropertyChangedEvent(email, x => x.Bcc, () => email.Bcc = bcc);
            Assert.AreEqual(bcc, email.Bcc);
            AssertHelper.ExpectedException<ArgumentNullException>(() => email.Bcc = null);
            Assert.AreEqual(bcc, email.Bcc);

            var sent = new DateTime(2012, 8, 2);
            AssertHelper.PropertyChangedEvent(email, x => x.Sent, () => email.Sent = sent);
            Assert.AreEqual(sent, email.Sent);

            AssertHelper.PropertyChangedEvent(email, x => x.Message, () => email.Message = "abc");
            Assert.AreEqual("abc", email.Message);
        }

        [TestMethod]
        public void ValidateTest()
        {
            var longText = "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz";
            var email = new Email();
            email.Validate();

            // No recipient.
            Assert.AreEqual(nameof(Email.To), email.Errors.Single().MemberNames.Single());

            email.Title = longText;
            Assert.AreEqual("The field Title must be a string with a maximum length of 255.", email.GetErrors(nameof(Email.Title)).Single().ErrorMessage);
            email.Title = "";

            Assert.IsFalse(email.GetErrors(nameof(Email.From)).Any());
            email.From = longText;
            Assert.AreEqual("The field From must be a string with a maximum length of 255.", email.GetErrors(nameof(Email.From)).Single().ErrorMessage);
            email.From = "";

            Assert.AreEqual("This email doesn't define a recipient.", string.Join(Environment.NewLine, email.Errors));
            
            email.To = new[] { "wrong email address" };
            email.CC = email.To;
            email.Bcc = email.To;
            Assert.AreEqual("The email wrong email address in the To field is not valid." + Environment.NewLine 
                + "The email wrong email address in the CC field is not valid." + Environment.NewLine
                + "The email wrong email address in the BCC field is not valid.", string.Join(Environment.NewLine, email.Errors));

            email.To = new[] { "correct-address@mail.com" };
            email.CC = email.To;
            email.Bcc = email.To;
            Assert.IsFalse(email.HasErrors);
        }
    }
}
