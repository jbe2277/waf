using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;
using Test.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Domain;

namespace Test.BookLibrary.Library.Applications.ViewModels
{
    [TestClass]
    public class PersonViewModelTest
    {
        [TestMethod]
        public void PersonViewModelPersonTest()
        {
            var personView = new MockPersonView();
            var personViewModel = new PersonViewModel(personView);

            Assert.IsFalse(personViewModel.IsEnabled);

            var person = new Person();
            AssertHelper.PropertyChangedEvent(personViewModel, x => x.Person, () => personViewModel.Person = person);
            Assert.AreEqual(person, personViewModel.Person);
            Assert.IsTrue(personViewModel.IsEnabled);

            AssertHelper.PropertyChangedEvent(personViewModel, x => x.IsEnabled, () => personViewModel.Person = null);
            Assert.IsNull(personViewModel.Person);
            Assert.IsFalse(personViewModel.IsEnabled);
        }

        [TestMethod]
        public void PersonViewModelIsValidTest()
        {
            var personView = new MockPersonView();
            var personViewModel = new PersonViewModel(personView);
            
            Assert.IsTrue(personViewModel.IsValid);

            AssertHelper.PropertyChangedEvent(personViewModel, x => x.IsValid, () => personViewModel.IsValid = false);
            Assert.IsFalse(personViewModel.IsValid);
        }
    }
}
