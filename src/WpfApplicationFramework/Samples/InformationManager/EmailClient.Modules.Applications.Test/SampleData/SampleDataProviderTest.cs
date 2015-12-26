using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Applications.SampleData;

namespace Test.InformationManager.EmailClient.Modules.Applications.SampleData
{
    [TestClass]
    public class SampleDataProviderTest
    {
        [TestMethod]
        public void CreateSampleDataTest()
        {
            var emailAccount = SampleDataProvider.CreateEmailAccount();
            Assert.IsNotNull(emailAccount);

            var inboxEmails = SampleDataProvider.CreateInboxEmails();
            Assert.IsTrue(inboxEmails.Any());

            var sentEmails = SampleDataProvider.CreateSentEmails();
            Assert.IsTrue(sentEmails.Any());

            var drafts = SampleDataProvider.CreateDrafts();
            Assert.IsTrue(drafts.Any());
        }
    }
}
