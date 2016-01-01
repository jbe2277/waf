using System.ComponentModel.DataAnnotations;
using System.Waf.Foundation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.Common.Domain;

namespace Test.InformationManager.Common.Domain
{
    [TestClass]
    public class ValidationModelTest : DomainTest
    {
        [TestMethod]
        public void ValidateTheModel()
        {
            var model = new MockValidationModel();
            Assert.AreEqual("The Name field is required.", model.Validate());
            Assert.AreEqual("The Name field is required.", model.Validate(nameof(model.Name)));

            model.Name = "Bill";
            Assert.AreEqual("", model.Validate());
            Assert.AreEqual("", model.Validate(nameof(model.Name)));
        }



        private class MockValidationModel : ValidationModel
        {
            [Required]
            public string Name { get; set; }
        }
    }
}
