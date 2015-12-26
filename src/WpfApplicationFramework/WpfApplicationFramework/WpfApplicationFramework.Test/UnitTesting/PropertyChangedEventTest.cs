using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.Waf.Foundation;
using System.Waf.UnitTesting;

namespace Test.Waf.UnitTesting
{
    [TestClass]
    public class PropertyChangedEventTest
    {
        [TestMethod]
        public void PropertyChangedEventTest1()
        {
            var person = new Person();
            AssertHelper.PropertyChangedEvent(person, x => x.Name, () => person.Name = "Luke");
            AssertHelper.PropertyChangedEvent(person, x => x.Name, () => person.Name = "Han", 1, ExpectedChangedCountMode.Exact);
            AssertHelper.PropertyChangedEvent(person, x => x.Name, () => person.Name = "Luke", 0, ExpectedChangedCountMode.AtLeast);
            AssertHelper.PropertyChangedEvent(person, x => x.Name, () => person.Name = "Han", 2, ExpectedChangedCountMode.AtMost);


            AssertHelper.ExpectedException<ArgumentNullException>(
                () => AssertHelper.PropertyChangedEvent((Person)null, x => x.Name, () => person.Name = "Han"));
            
            AssertHelper.ExpectedException<ArgumentNullException>(
                () => AssertHelper.PropertyChangedEvent(person, null, () => person.Name = "Han"));

            AssertHelper.ExpectedException<ArgumentException>(() =>
                AssertHelper.PropertyChangedEvent(person, x => x.Name.Length, () => person.Name = "Luke"));

            AssertHelper.ExpectedException<ArgumentNullException>(
                () => AssertHelper.PropertyChangedEvent(person, x => x.Name, null));
           
            AssertHelper.ExpectedException<ArgumentOutOfRangeException>(
                () => AssertHelper.PropertyChangedEvent(person, x => x.Name, () => person.Name = "Han", -1, ExpectedChangedCountMode.Exact));
        }

        [TestMethod]
        public void PropertyChangedEventTest2()
        {
            SpecialPerson specialPerson = new SpecialPerson();
            AssertHelper.ExpectedException<AssertException>(() =>
                AssertHelper.PropertyChangedEvent(specialPerson, x => x.Name, () => specialPerson.Name = "Luke"));

            AssertHelper.ExpectedException<AssertException>(() =>
                AssertHelper.PropertyChangedEvent(specialPerson, x => x.Age, () => specialPerson.Age = 31));

            AssertHelper.ExpectedException<AssertException>(() =>
                AssertHelper.PropertyChangedEvent(specialPerson, x => x.Weight, () => specialPerson.Weight = 80));


            Person person = new Person();
            AssertHelper.ExpectedException<AssertException>(() =>
                AssertHelper.PropertyChangedEvent(person, x => x.Name, () => person.Name = "Luke", 0, ExpectedChangedCountMode.Exact));

            AssertHelper.ExpectedException<AssertException>(() =>
                AssertHelper.PropertyChangedEvent(person, x => x.Name, () => person.Name = "Han", 2, ExpectedChangedCountMode.Exact));

            AssertHelper.ExpectedException<AssertException>(() =>
                AssertHelper.PropertyChangedEvent(person, x => x.Name, () => person.Name = "Luke", 2, ExpectedChangedCountMode.AtLeast));

            AssertHelper.ExpectedException<AssertException>(() =>
                AssertHelper.PropertyChangedEvent(person, x => x.Name, () => person.Name = "Han", 0, ExpectedChangedCountMode.AtMost));
        }

        [TestMethod]
        public void WrongEventSenderTest()
        {
            SpecialPerson person = new SpecialPerson();
            AssertHelper.ExpectedException<AssertException>(() =>
                AssertHelper.PropertyChangedEvent(person, x => x.Name, () => person.RaiseWrongNamePropertyChanged()));
        }

        [TestMethod]
        public void WrongExpressionTest()
        {
            Person person = new Person();
            
            AssertHelper.ExpectedException<ArgumentException>(() =>
                AssertHelper.PropertyChangedEvent(person, x => x, () => person.Name = "Luke"));

            AssertHelper.ExpectedException<ArgumentException>(() =>
                AssertHelper.PropertyChangedEvent(person, x => x.ToString(), () => person.Name = "Luke"));

            AssertHelper.ExpectedException<ArgumentException>(() =>
                AssertHelper.PropertyChangedEvent(person, x => Math.Abs(1), () => person.Name = "Luke"));
        }



        private class Person : Model
        {
            private string name;


            public string Name
            {
                get { return name; }
                set
                {
                    if (name != value)
                    {
                        name = value;
                        RaisePropertyChanged("Name");
                    }
                }
            }
        }

        private class SpecialPerson : INotifyPropertyChanged
        {
            private string name;
            private double weight;


            public event PropertyChangedEventHandler PropertyChanged;


            public string Name
            {
                get { return name; }
                set
                {
                    if (name != value)
                    {
                        name = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("WrongName"));
                    }
                }
            }

            public int Age { get; set; }

            public double Weight
            {
                get { return weight; }
                set
                {
                    if (weight != value)
                    {
                        weight = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("Weight"));
                        OnPropertyChanged(new PropertyChangedEventArgs("Weight"));
                    }
                }
            }


            public void RaiseWrongNamePropertyChanged()
            {
                if (PropertyChanged != null) { PropertyChanged(null, new PropertyChangedEventArgs("Name")); }
            }

            protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                if (PropertyChanged != null) { PropertyChanged(this, e); }
            }
        }
    }
}
