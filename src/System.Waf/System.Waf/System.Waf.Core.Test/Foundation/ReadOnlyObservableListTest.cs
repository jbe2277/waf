using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Waf.Foundation;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class ReadOnlyObservableListTest
    {
        [TestMethod]
        public void ChangedEventsTest()
        {
            var list = new ObservableCollection<string>();
            var readOnlyList = new ReadOnlyObservableList<string>(list);

            int collectionChangedCalled = 0;
            NotifyCollectionChangedEventHandler collectionChangedHandler = (sender, e) =>
            {
                collectionChangedCalled++;
            };
            int countPropertyChangedCalled = 0;
            PropertyChangedEventHandler propertyChangedHandler = (sender, e) =>
            {
                if (e.PropertyName == nameof(readOnlyList.Count))
                {
                    countPropertyChangedCalled++;
                }
            };

            readOnlyList.CollectionChanged += collectionChangedHandler;
            readOnlyList.PropertyChanged += propertyChangedHandler;
            list.Add("Hello");
            Assert.AreEqual(1, collectionChangedCalled);
            Assert.AreEqual(1, countPropertyChangedCalled);

            readOnlyList.CollectionChanged -= collectionChangedHandler;
            readOnlyList.PropertyChanged -= propertyChangedHandler;
            list.Add("World");
            Assert.AreEqual(1, collectionChangedCalled);
            Assert.AreEqual(1, countPropertyChangedCalled);
        }
    }
}
