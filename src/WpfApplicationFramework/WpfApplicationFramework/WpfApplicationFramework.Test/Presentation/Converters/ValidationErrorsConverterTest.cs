using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Waf.Presentation.Converters;
using System.Waf.UnitTesting;
using System.Windows;
using System.Windows.Controls;

namespace Test.Waf.Presentation.Converters
{
    [TestClass]
    public class ValidationErrorsConverterTest
    {
        [TestMethod]
        public void ConverterTest()
        {
            ValidationErrorsConverter converter = ValidationErrorsConverter.Default;
            Assert.AreEqual(converter, ValidationErrorsConverter.Default);
            
            AssertHelper.ExpectedException<NotSupportedException>(() => converter.ConvertBack(null, (Type[])null, null, null));

            Assert.AreEqual(DependencyProperty.UnsetValue, converter.Convert((object[])null, null, null, null));

            Assert.AreEqual(DependencyProperty.UnsetValue, converter.Convert(new[] { "WrongType" }, null, null, null));

            List<ValidationError> validationErrors = new List<ValidationError>();
            Assert.AreEqual("", converter.Convert(new[] { validationErrors }, null, null, null));

            ExceptionValidationRule rule = new ExceptionValidationRule();
            validationErrors.Add(new ValidationError(rule, new object(), "First error message", null));
            Assert.AreEqual("First error message", converter.Convert(new[] { validationErrors }, null, null, null));

            validationErrors.Add(new ValidationError(rule, new object(), "Second error message", null));
            Assert.AreEqual("First error message" + Environment.NewLine + "Second error message",
                converter.Convert(new[] { validationErrors }, null, null, null));


            // Call the obsolete methods

#pragma warning disable 618
            AssertHelper.ExpectedException<NotSupportedException>(() => converter.ConvertBack(null, (Type)null, null, null));

            validationErrors = new List<ValidationError>();
            validationErrors.Add(new ValidationError(rule, new object(), "First error message", null));
            Assert.AreEqual("First error message", converter.Convert(validationErrors, null, null, null));
#pragma warning restore 618
        }
    }
}
