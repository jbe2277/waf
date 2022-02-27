using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using Test.InformationManager.Common.Domain;
using System.Waf.UnitTesting;

namespace Test.InformationManager.EmailClient.Modules.Domain.Emails;

[TestClass]
public class EmailClientRootTest : DomainTest
{
    [TestMethod]
    public void EmailAccountsTest()
    {
        var root = new EmailClientRoot();
        Assert.IsFalse(root.EmailAccounts.Any());

        var emailAccount1 = new EmailAccount();
        root.AddEmailAccount(emailAccount1);
        Assert.AreEqual(emailAccount1, root.EmailAccounts.Single());

        var emailAccount2 = new EmailAccount();
        root.AddEmailAccount(emailAccount2);
        AssertHelper.SequenceEqual(new[] { emailAccount1, emailAccount2 }, root.EmailAccounts);

        root.RemoveEmailAccount(emailAccount1);
        Assert.AreEqual(emailAccount2, root.EmailAccounts.Single());

        var emailAccount3 = new EmailAccount();
        root.ReplaceEmailAccount(emailAccount2, emailAccount3);
        Assert.AreEqual(emailAccount3, root.EmailAccounts.Single());
    }

    [TestMethod]
    public void EmailFolderTest()
    {
        var root = new EmailClientRoot();

        Assert.IsNotNull(root.Inbox);
        Assert.IsNotNull(root.Outbox);
        Assert.IsNotNull(root.Sent);
        Assert.IsNotNull(root.Drafts);
        Assert.IsNotNull(root.Deleted);

        var email1 = new Email();
        root.Inbox.AddEmail(email1);
        root.Inbox.RemoveEmail(email1);
        Assert.AreEqual(email1, root.Deleted.Emails.Single());

        root.Deleted.RemoveEmail(email1);
        Assert.IsFalse(root.Deleted.Emails.Any());
    }
}
