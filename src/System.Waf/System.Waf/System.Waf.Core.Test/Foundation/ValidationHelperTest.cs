using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Waf.Foundation;
using System.Waf.UnitTesting;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class ValidationHelperTest
    {
        [TestMethod]
        public void ValidateTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => ValidationHelper.Validate(null!));

            var instance = new TypeValidationClass();
            Assert.IsFalse(ValidationHelper.Validate(instance).Any());

            var error = new ValidationResult("My error");
            instance.Result = error;
            Assert.AreSame(error, ValidationHelper.Validate(instance).Single());
        }

        [CustomValidation(typeof(TypeValidationClass), nameof(TestValidate))]
        public class TypeValidationClass : IValidatableObject
        {
            public ValidationResult? Result { get; set; } = ValidationResult.Success;

            public static ValidationResult? TestValidate(TypeValidationClass value, ValidationContext context)
            {
                return value.Result;
            }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                return null!;
            }
        }
    }
}
