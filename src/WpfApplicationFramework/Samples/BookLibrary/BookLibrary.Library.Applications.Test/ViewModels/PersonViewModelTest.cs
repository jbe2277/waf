using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Test.BookLibrary.Library.Applications.Views;
using System.Waf.UnitTesting;
using Waf.BookLibrary.Library.Domain;
using System.Waf.Applications;

namespace Test.BookLibrary.Library.Applications.ViewModels
{
    [TestClass]
    public class PersonViewModelTest
    {
        [TestMethod]
        public void PersonViewModelPersonTest()
        {
            MockPersonView personView = new MockPersonView();
            PersonViewModel personViewModel = new PersonViewModel(personView);

            Assert.IsFalse(personViewModel.IsEnabled);

            Person person = new Person();
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
            MockPersonView personView = new MockPersonView();
            PersonViewModel personViewModel = new PersonViewModel(personView);
            
            Assert.IsTrue(personViewModel.IsValid);

            AssertHelper.PropertyChangedEvent(personViewModel, x => x.IsValid, () => personViewModel.IsValid = false);
            Assert.IsFalse(personViewModel.IsValid);
        }

        [TestMethod]
        public void PersonViewModelCommandsTest()
        {
            MockPersonView personView = new MockPersonView();
            PersonViewModel personViewModel = new PersonViewModel(personView);

            DelegateCommand mockCommand = new DelegateCommand(() => { });
            AssertHelper.PropertyChangedEvent(personViewModel, x => x.CreateNewEmailCommand, () =>
                personViewModel.CreateNewEmailCommand = mockCommand);
            Assert.AreEqual(mockCommand, personViewModel.CreateNewEmailCommand);
        }
    }
}
