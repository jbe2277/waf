using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;
using System.Waf.UnitTesting;
using System.Waf.Foundation;
using Test.InformationManager.Common.Domain;

namespace Test.InformationManager.EmailClient.Modules.Domain.AccountSettings
{
    [TestClass]
    public class UserCreditsTest : DomainTest
    {
        [TestMethod]
        public void PropertiesTest()
        {
            UserCredits userCredits = new UserCredits();

            AssertHelper.PropertyChangedEvent(userCredits, x => x.UserName, () => userCredits.UserName = "bill");
            Assert.AreEqual("bill", userCredits.UserName);

            AssertHelper.PropertyChangedEvent(userCredits, x => x.Password, () => userCredits.Password = "secret");
            Assert.AreEqual("secret", userCredits.Password);
        }

        [TestMethod]
        public void CloneTest()
        {
            UserCredits userCredits = new UserCredits() { UserName = "bill", Password = "secret" };
            UserCredits clone = userCredits.Clone();

            Assert.AreNotEqual(userCredits, clone);
            Assert.AreEqual(userCredits.UserName, clone.UserName);
            Assert.AreEqual(userCredits.Password, clone.Password);
        }

        [TestMethod]
        public void ValidationTest()
        {
            UserCredits userCredits = new UserCredits();
            Assert.AreEqual("The Username field is required.", userCredits.Validate("UserName"));

            userCredits.UserName = "bill";
            Assert.AreEqual("", userCredits.Validate("UserName"));
        }
    }
}
