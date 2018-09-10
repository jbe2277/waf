using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Waf.Foundation;
using System.Waf.UnitTesting;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class ValidatableModelTest
    {
        [TestMethod]
        public void HasAndGetErrorsWithPropertyValidation()
        {
            Person person = new Person();

            // Person is invalid but until now nobody has validated this object.

            Assert.IsFalse(person.HasErrors);
            Assert.IsFalse(person.Errors.Any());

            // Validate person and see that Name is required.

            AssertHelper.PropertyChangedEvent(person, x => x.HasErrors, () 
                => AssertErrorsChangedEvent(person, x => x.Name, () => person.Validate()));
            Assert.IsTrue(person.HasErrors);
            Assert.AreEqual(Person.NameRequiredError, person.Errors.Single().ErrorMessage);
            Assert.AreEqual(Person.NameRequiredError, person.GetErrors(nameof(Person.Name)).Single().ErrorMessage);

            // Set a valid name.

            AssertHelper.PropertyChangedEvent(person, x => x.Errors, () => person.Name = "Bill");
            Assert.IsFalse(person.HasErrors);
            Assert.IsFalse(person.Errors.Any());
            Assert.IsFalse(((INotifyDataErrorInfo)person).GetErrors(nameof(Person.Name)).Cast<object>().Any());

            // Set another valid name; ErrorsChanged event must not be called.

            AssertHelper.ExpectedException<AssertFailedException>(() => AssertErrorsChangedEvent(person, x => x.Name, () => person.Name = "Steve"));

            // Set the same name again; ErrorsChanged event must not be called.

            AssertHelper.ExpectedException<AssertFailedException>(() => AssertErrorsChangedEvent(person, x => x.Name, () => person.Name = "Steve"));

            // Call validate on the valid Person; ErrorsChanged event must not be called.

            AssertHelper.ExpectedException<AssertFailedException>(() => AssertErrorsChangedEvent(person, null, () => person.Validate()));

            // Set an invalid name (null)

            AssertErrorsChangedEvent(person, x => x.Name, () => person.Name = null);
            Assert.IsTrue(person.HasErrors);
            Assert.AreEqual(Person.NameRequiredError, person.Errors.Single().ErrorMessage);
            Assert.AreEqual(Person.NameRequiredError, person.GetErrors(nameof(Person.Name)).Single().ErrorMessage);

            // Set an invalid email address that creates two additional validation errors.

            person.Email = "TooLongAndAnInvalidEmailAddress@";
            bool isValid = true;
            AssertErrorsChangedEvent(person, x => x.Email, () => isValid = person.Validate());
            Assert.IsFalse(isValid);
            Assert.IsTrue(person.HasErrors);
            Assert.AreEqual(3, person.Errors.Count());
            Assert.AreEqual(2, person.GetErrors("Email").Count());
            Assert.IsTrue(person.GetErrors("Email").Any(x => x.ErrorMessage == Person.EmailInvalidError));
            Assert.IsTrue(person.GetErrors("Email").Any(x => x.ErrorMessage == Person.EmailLengthError));
            
            // Set a valid name and email address

            AssertErrorsChangedEvent(person, x => x.Name, () => person.Name = "Bill");
            person.Email = "h.p@h.edu";
            AssertErrorsChangedEvent(person, x => x.Email, () => isValid = person.Validate());
            Assert.IsTrue(isValid);
            Assert.IsFalse(person.HasErrors);
            Assert.IsFalse(person.Errors.Any());
            Assert.IsFalse(person.GetErrors("Email").Any());

            string test = "";
            AssertHelper.ExpectedException<ArgumentException>(() => person.SetPropertyAndValidate(ref test, "Test", null));
        }

        [TestMethod]
        public void HasAndGetErrorsWithObjectValidation()
        {
            Person person = new Person() { Name = "Bill" };
            person.Validate();
            Assert.IsFalse(person.HasErrors);

            // Create an entity error

            var entityError = new ValidationResult("My entity error");
            person.EntityError = entityError;

            bool isValid = true;
            AssertHelper.PropertyChangedEvent(person, x => x.HasErrors, () =>
                AssertErrorsChangedEvent(person, null, () => isValid = person.Validate()));
            Assert.IsFalse(isValid);
            Assert.IsTrue(person.HasErrors);
            Assert.AreEqual(entityError, person.Errors.Single());
            Assert.AreEqual(entityError, person.GetErrors("").Single());
            Assert.AreEqual(entityError, person.GetErrors(null).Single());
        }

        //[TestMethod]
        public void HasMultipleErrorsWithDifferentValidationTypes()
        {
            Person person = new Person() { Name = "Bill", Age = 200 };
            Assert.AreEqual(nameof(Person.Age), person.Errors.Single().MemberNames.Single());

            person.Name = "";
            Assert.AreEqual(2, person.Errors.Count);
            Assert.IsNotNull(person.Errors.Single(x => x.MemberNames.Single() == nameof(Person.Name)));

            var entityError = new ValidationResult("My entity error");
            person.EntityError = entityError;
            person.Validate();

            Assert.AreEqual(3, person.Errors.Count);  // TODO: Does not work because of an unwanted optimization in Validator.
            // See: Validator.GetObjectValidationErrors
        }

        [TestMethod]
        public void SerializationWithDcsTest()
        {
            var serializer = new DataContractSerializer(typeof(Person));

            using (MemoryStream stream = new MemoryStream())
            {
                Person person = new Person() { Name = "Hugo" };
                serializer.WriteObject(stream, person);

                stream.Position = 0;
                Person newPerson = (Person)serializer.ReadObject(stream);
                Assert.AreEqual(person.Name, newPerson.Name);
            }
        }

        [TestMethod]
        public void ValidationResultComparerTest()
        {
            var comparerType = typeof(ValidatableModel).GetNestedType("ValidationResultComparer", BindingFlags.NonPublic);
            var comparer = (IEqualityComparer<ValidationResult>)comparerType.GetProperty("Default", BindingFlags.Static | BindingFlags.Public).GetValue(null);

            Assert.IsTrue(comparer.Equals(null, null));
            Assert.IsFalse(comparer.Equals(new ValidationResult(null), null));
            Assert.IsFalse(comparer.Equals(null, new ValidationResult(null)));
            Assert.IsTrue(comparer.Equals(new ValidationResult(null), new ValidationResult(null)));
            Assert.IsFalse(comparer.Equals(new ValidationResult("Test"), new ValidationResult("Bill")));
            Assert.IsTrue(comparer.Equals(new ValidationResult("Test"), new ValidationResult("Test")));
            Assert.IsTrue(comparer.Equals(new ValidationResult("Test", new[] { "Name", "Age" }), new ValidationResult("Test", new[] { "Name", "Age" })));
            Assert.IsTrue(comparer.Equals(new ValidationResult("Test", new[] { "Name", null }), new ValidationResult("Test", new[] { "Name", null })));
            Assert.IsFalse(comparer.Equals(new ValidationResult("Test", new[] { "Name", "Wrong" }), new ValidationResult("Test", new[] { "Name", "Age" })));

            Assert.AreEqual(0, comparer.GetHashCode(null));
            Assert.AreEqual(0, comparer.GetHashCode(new ValidationResult(null)));
            Assert.AreEqual("".GetHashCode(), comparer.GetHashCode(new ValidationResult("")));
            Assert.AreEqual("Test".GetHashCode(), comparer.GetHashCode(new ValidationResult("Test")));
            Assert.AreEqual(0, comparer.GetHashCode(new ValidationResult(null, new string[0])));
            Assert.AreEqual("Test".GetHashCode(), comparer.GetHashCode(new ValidationResult("Test", new string[0])));
            Assert.AreEqual("Test".GetHashCode() ^ "Name".GetHashCode(), comparer.GetHashCode(new ValidationResult("Test", new[] { "Name" })));
            Assert.AreEqual("Test".GetHashCode() ^ "Name".GetHashCode(), comparer.GetHashCode(new ValidationResult("Test", new[] { "Name", null })));
            Assert.AreEqual("Test".GetHashCode() ^ "Name".GetHashCode() ^ "Age".GetHashCode(), comparer.GetHashCode(new ValidationResult("Test", new[] { "Name", "Age" })));
        }

        private static void AssertErrorsChangedEvent<T>(T model, Expression<Func<T, object>> expression, Action raiseErrorsChanged) where T : INotifyDataErrorInfo
        {
            string propertyName = expression == null ? null : AssertHelper.GetProperty(expression).Name;
            int errorsChangedCount = 0;

            EventHandler<DataErrorsChangedEventArgs> handler = (sender, e) =>
            {
                Assert.AreEqual(model, sender);
                if (propertyName == null || e.PropertyName == propertyName)
                {
                    errorsChangedCount++;
                }
            };

            model.ErrorsChanged += handler;
            raiseErrorsChanged();
            model.ErrorsChanged -= handler;

            Assert.AreEqual(1, errorsChangedCount);
        }


        [DataContract]
        public class Person : ValidatableModel, IValidatableObject
        {
            public const string NameRequiredError = "The Name field is required.";
            public const string EmailLengthError = "The field Email must be a string with a maximum length of 10.";
            public const string EmailInvalidError = "The Email field is not a valid e-mail address.";

            [DataMember] private string name;
            [DataMember] private string email;
            [DataMember] private int age;

            public ValidationResult EntityError { get; set; }

            [Required(ErrorMessage = NameRequiredError)]
            public string Name
            {
                get { return name; }
                set { SetPropertyAndValidate(ref name, value); }
            }

            [EmailAddress(ErrorMessage = EmailInvalidError)]
            [StringLength(10, ErrorMessage = EmailLengthError)]
            public string Email
            {
                get { return email; }
                set { SetProperty(ref email, value); }
            }

            [CustomValidation(typeof(Person), nameof(ValidateAge))]
            public int Age
            {
                get { return age; }
                set { SetPropertyAndValidate(ref age, value); }
            }

            public new bool SetPropertyAndValidate<T>(ref T field, T value, string propertyName)
            {
                return base.SetPropertyAndValidate(ref field, value, propertyName);
            }

            public static ValidationResult ValidateAge(int value, ValidationContext context)
            {
                if (value > 150) return new ValidationResult("Too old", new[] { nameof(Age) });
                return ValidationResult.Success;
            }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                var validationResults = new List<ValidationResult>();
                if (EntityError != null) { validationResults.Add(EntityError); }
                return validationResults;
            }
        }
    }
}
