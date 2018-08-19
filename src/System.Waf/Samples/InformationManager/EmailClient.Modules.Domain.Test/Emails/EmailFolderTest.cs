using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using Test.InformationManager.Common.Domain;
using System.Waf.UnitTesting;

namespace Test.InformationManager.EmailClient.Modules.Domain.Emails
{
    [TestClass]
    public class EmailFolderTest : DomainTest
    {
        [TestMethod]
        public void AddAndRemoveEmails()
        {
            var emailDeletionService = new MockEmailDeletionService();
            var emailFolder = new EmailFolder() { EmailDeletionService = emailDeletionService };

            Assert.IsFalse(emailFolder.Emails.Any());
            var email1 = new Email();
            
            emailFolder.AddEmail(email1);
            Assert.AreEqual(email1, emailFolder.Emails.Single());

            var email2 = new Email();
            emailFolder.AddEmail(email2);
            AssertHelper.SequenceEqual(new[] { email1, email2 }, emailFolder.Emails);

            bool deleteEmailCalled = false;
            emailDeletionService.DeleteEmailAction = (folder, email) =>
            {
                deleteEmailCalled = true;
                Assert.AreEqual(emailFolder, folder);
                Assert.AreEqual(email1, email);
            };
            emailFolder.RemoveEmail(email1);
            Assert.IsTrue(deleteEmailCalled);
        }
    }
}
