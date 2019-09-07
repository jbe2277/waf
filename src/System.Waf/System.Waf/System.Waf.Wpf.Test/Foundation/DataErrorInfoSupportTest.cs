using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Foundation;
using System.ComponentModel.DataAnnotations;
using System.Waf.UnitTesting;
using System.Globalization;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class DataErrorInfoSupportTest
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        }

        [TestMethod]
        public void InvalidArgumentsTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => new DataErrorInfoSupport(null));

            var dataErrorInfoSupport = new DataErrorInfoSupport(new MockEntity());
            string message;
            AssertHelper.ExpectedException<ArgumentException>(() => message = dataErrorInfoSupport["InvalidMember"]);
        }
        
        [TestMethod]
        public void ValidateTest()
        {
            var mockEntity = new MockEntity();
            var dataErrorInfoSupport = new DataErrorInfoSupport(mockEntity);
            
            Assert.AreEqual("The Firstname field is required.", dataErrorInfoSupport[nameof(MockEntity.Firstname)]);
            Assert.AreEqual("The Lastname field is required.", dataErrorInfoSupport[nameof(MockEntity.Lastname)]);
            Assert.AreEqual("", dataErrorInfoSupport[nameof(MockEntity.Email)]);
            Assert.AreEqual("The Firstname field is required." + Environment.NewLine + "The Lastname field is required.", 
                dataErrorInfoSupport.Error);

            mockEntity.Firstname = "Harry";
            mockEntity.Lastname = "Potter";
            Assert.AreEqual("", dataErrorInfoSupport[nameof(MockEntity.Lastname)]);
            Assert.AreEqual("", dataErrorInfoSupport.Error);

            mockEntity.Email = "InvalidEmailAddress";
            Assert.AreEqual("", dataErrorInfoSupport[nameof(MockEntity.Lastname)]);
            Assert.AreEqual(@"The field Email must match the regular expression '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$'.", 
                dataErrorInfoSupport[nameof(MockEntity.Email)]);
            Assert.AreEqual(@"The field Email must match the regular expression '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$'.", 
                dataErrorInfoSupport.Error);
        }


        private class MockEntity
        {
            [Required, StringLength(30)]
            public string Firstname { get; set; }

            [Required, StringLength(30)]
            public string Lastname { get; set; }

            [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$")]
            public string Email { get; set; }
        }
    }
}
