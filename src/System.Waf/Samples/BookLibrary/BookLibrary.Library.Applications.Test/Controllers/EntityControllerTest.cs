using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Waf.BookLibrary.Library.Applications.Controllers;

namespace Test.BookLibrary.Library.Applications.Controllers
{
    [TestClass]
    public class EntityControllerTest : TestClassBase
    {
        [TestMethod]
        public void EntityToStringTest()
        {
            var entity = new Entity() { ToStringValue = "Test1" };
            Assert.AreEqual("Test1", EntityController.EntityToString(entity));

            var entity2 = new FormattableEntity() { ToStringValue = "Test2" };
            Assert.AreEqual("Test2", EntityController.EntityToString(entity2));
        }


        private class Entity
        {
            public string ToStringValue;

            public override string ToString() { return ToStringValue; }
        }

        private class FormattableEntity : IFormattable
        {
            public string ToStringValue;

            public string ToString(string format, IFormatProvider formatProvider)
            {
                return ToStringValue;
            }
        }
    }
}
