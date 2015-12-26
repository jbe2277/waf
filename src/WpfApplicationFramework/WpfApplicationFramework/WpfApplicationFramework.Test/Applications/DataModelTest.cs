using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Waf.Applications;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Waf.Applications
{
    [TestClass]
    public class DataModelTest
    {
        [TestMethod]
        public void SerializationTest()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (MemoryStream stream = new MemoryStream())
            {
                PersonDataModel person = new PersonDataModel() { Name = "Hugo" };
                formatter.Serialize(stream, person);

                stream.Position = 0;
                PersonDataModel newPerson = (PersonDataModel)formatter.Deserialize(stream);
                Assert.AreEqual(person.Name, newPerson.Name);
            }
        }



        [Serializable]
#pragma warning disable 618
        private class PersonDataModel : DataModel
#pragma warning restore 618
        {
            private string name;

            public string Name
            {
                get { return name; }
                set { SetProperty(ref name, value); }
            }
        }
    }
}
