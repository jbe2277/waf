using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Waf.Foundation;
using System.Waf.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class DataErrorInfoExtensionsTest
    {
        [TestMethod]
        public void ValidateTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => DataErrorInfoExtensions.Validate(null!));
            AssertHelper.ExpectedException<ArgumentNullException>(() => DataErrorInfoExtensions.Validate(null!, "Name"));

            var entity = new MockEntity() { Error = "Test Error" };
            Assert.AreEqual("Test Error", entity.Validate());
            entity.Error = null;
            Assert.AreEqual("", entity.Validate());

            entity.Errors.Add("Name", "Name Error");
            Assert.AreEqual("Name Error", entity.Validate("Name"));
            entity.Errors.Add("Address", null);
            Assert.AreEqual("", entity.Validate("Address"));
        }


        private class MockEntity : IDataErrorInfo
        {
            public readonly Dictionary<string, string?> Errors = new Dictionary<string, string?>();

            public string? Error { get; set; }

            public string? this[string columnName] => Errors[columnName];
        }
    }
}
