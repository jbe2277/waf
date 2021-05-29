using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;
using System.ComponentModel;
using System.Waf.Foundation;
using System.IO;
using System.Runtime.Serialization;
using System;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class ModelTest
    {
        [TestMethod]
        public void RaisePropertyChangedTest()
        {
            var luke = new Person();

            AssertHelper.PropertyChangedEvent(luke, x => x.Name, () => luke.Name = "Luke");
            Assert.AreEqual("Luke", luke.Name);

            AssertHelper.PropertyChangedEvent(luke, x => x.Name, () => luke.Name = "Skywalker");
            Assert.AreEqual("Skywalker", luke.Name);

            AssertHelper.PropertyChangedEvent(luke, x => x.Email, () => luke.Email = "luke.skywalker@tatooine.com");
            Assert.AreEqual("luke.skywalker@tatooine.com", luke.Email);

            AssertHelper.PropertyChangedEvent(luke, x => x.Phone, () => luke.Phone = "42");
            Assert.AreEqual("42", luke.Phone);
        }

        [TestMethod]
        public void RaisePropertiesChangedTest()
        {
            var luke = new Person();
            AssertHelper.PropertyChangedEvent(luke, x => x.Name, () => luke.RaiseAllChanged());
            AssertHelper.PropertyChangedEvent(luke, x => x.Email, () => luke.RaiseAllChanged());
            AssertHelper.PropertyChangedEvent(luke, x => x.Phone, () => luke.RaiseAllChanged());

            luke.InnerRaisePropertiesChanged();
            AssertHelper.ExpectedException<ArgumentNullException>(() => luke.InnerRaisePropertiesChanged(null!));
            AssertHelper.PropertyChangedEvent(luke, x => x.Name, () => luke.InnerRaisePropertiesChanged(nameof(luke.Name)));
        }

        [TestMethod]
        public void AddAndRemoveEventHandler()
        {
            var luke = new Person();
            bool eventRaised;

            void EventHandler(object sender, PropertyChangedEventArgs e)
            {
                eventRaised = true;
            }

            eventRaised = false;
            luke.PropertyChanged += EventHandler;
            luke.Name = "Luke";
            Assert.IsTrue(eventRaised, "The property changed event needs to be raised");

            eventRaised = false;
            luke.PropertyChanged -= EventHandler;
            luke.Name = "Luke Skywalker";
            Assert.IsFalse(eventRaised, "The event handler must not be called because it was removed from the event.");
        }

        [TestMethod]
        public void SerializationWithDcsTest()
        {
            var serializer = new DataContractSerializer(typeof(Person));

            using var stream = new MemoryStream();
            var person = new Person() { Name = "Hugo" };
            serializer.WriteObject(stream, person);

            stream.Position = 0;
            var newPerson = (Person)serializer.ReadObject(stream);
            Assert.AreEqual(person.Name, newPerson.Name);
        }


        [DataContract]
        private class Person : Model
        {
            [DataMember] private string name = "";
            [DataMember] private string? email;
            [DataMember] private string? phone;

            public string? Name
            {
                get => name;
                set => SetProperty(ref name, value ?? "");
            }

            public string? Email
            {
                get => email;
                set
                {
                    if (email == value) return;
                    email = value;
                    RaisePropertyChanged();
                }
            }

            public string? Phone
            {
                get => phone;
                set
                {
                    if (phone == value) return;
                    phone = value;
                    RaisePropertyChanged(nameof(Phone));
                }
            }

            public void RaiseAllChanged() => RaisePropertyChanged(nameof(Name), nameof(Email), nameof(Phone));

            public void InnerRaisePropertiesChanged(params string[] propertyNames) => RaisePropertyChanged(propertyNames);
        }
    }
}
