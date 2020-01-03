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
            void CollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
            {
                collectionChangedCalled++;
            }
            int countPropertyChangedCalled = 0;
            void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == nameof(readOnlyList.Count))
                {
                    countPropertyChangedCalled++;
                }
            }

            readOnlyList.CollectionChanged += CollectionChangedHandler;
            readOnlyList.PropertyChanged += PropertyChangedHandler;
            list.Add("Hello");
            Assert.AreEqual(1, collectionChangedCalled);
            Assert.AreEqual(1, countPropertyChangedCalled);

            readOnlyList.CollectionChanged -= CollectionChangedHandler;
            readOnlyList.PropertyChanged -= PropertyChangedHandler;
            list.Add("World");
            Assert.AreEqual(1, collectionChangedCalled);
            Assert.AreEqual(1, countPropertyChangedCalled);
        }
    }
}
