using System;
using System.Linq;
using System.Waf.Applications;
using System.Waf.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;
using System.IO;

namespace Test.Waf.Applications
{
    [TestClass]
    public class RecentFileListTest
    {
        [TestMethod]
        public void SetMaxFilesNumber()
        {
            var recentFileList = new RecentFileList();
            recentFileList.AddFile("Doc4");
            recentFileList.AddFile("Doc3");
            recentFileList.AddFile("Doc2");
            recentFileList.AddFile("Doc1");
            AssertHelper.SequenceEqual(new[] { "Doc1", "Doc2", "Doc3", "Doc4" }, recentFileList.RecentFiles.Select(f => f.Path));
            
            // Set a lower number than items are in the list => expect that the list is truncated.
            recentFileList.MaxFilesNumber = 3;
            Assert.AreEqual(3, recentFileList.MaxFilesNumber);
            AssertHelper.SequenceEqual(new[] { "Doc1", "Doc2", "Doc3" }, recentFileList.RecentFiles.Select(f => f.Path));

            AssertHelper.ExpectedException<ArgumentException>(() => recentFileList.MaxFilesNumber = -3);
        }

        [TestMethod]
        public void AddFiles()
        {
            var recentFileList = new RecentFileList() { MaxFilesNumber = 3 };

            AssertHelper.ExpectedException<ArgumentException>(() => recentFileList.AddFile(null!));

            // Add files to an empty list
            recentFileList.AddFile("Doc3");
            AssertHelper.SequenceEqual(new[] { "Doc3" }, recentFileList.RecentFiles.Select(f => f.Path));
            recentFileList.AddFile("Doc2");
            AssertHelper.SequenceEqual(new[] { "Doc2", "Doc3" }, recentFileList.RecentFiles.Select(f => f.Path));
            recentFileList.AddFile("Doc1");
            AssertHelper.SequenceEqual(new[] { "Doc1", "Doc2", "Doc3" }, recentFileList.RecentFiles.Select(f => f.Path));

            // Add a file to a full list
            recentFileList.AddFile("Doc4");
            AssertHelper.SequenceEqual(new[] { "Doc4", "Doc1", "Doc2" }, recentFileList.RecentFiles.Select(f => f.Path));

            // Add a file that already exists in the list
            recentFileList.AddFile("Doc2");
            AssertHelper.SequenceEqual(new[] { "Doc2", "Doc4", "Doc1" }, recentFileList.RecentFiles.Select(f => f.Path));
        }

        [TestMethod]
        public void AddFilesAndPinThem()
        {
            var recentFileList = new RecentFileList() { MaxFilesNumber = 3 };

            // Add files to an empty list
            recentFileList.AddFile("Doc3");
            recentFileList.AddFile("Doc2");
            recentFileList.AddFile("Doc1");
            AssertHelper.SequenceEqual(new[] { "Doc1", "Doc2", "Doc3" }, recentFileList.RecentFiles.Select(f => f.Path));

            // Pin first file
            recentFileList.RecentFiles.First(r => r.Path == "Doc3").IsPinned = true;
            AssertHelper.SequenceEqual(new[] { "Doc3", "Doc1", "Doc2" }, recentFileList.RecentFiles.Select(f => f.Path));

            // Add a file to a full list
            recentFileList.AddFile("Doc4");
            AssertHelper.SequenceEqual(new[] { "Doc3", "Doc4", "Doc1" }, recentFileList.RecentFiles.Select(f => f.Path));

            // Add a file that already exists in the list
            recentFileList.AddFile("Doc1");
            AssertHelper.SequenceEqual(new[] { "Doc3", "Doc1", "Doc4" }, recentFileList.RecentFiles.Select(f => f.Path));

            // Pin all files
            recentFileList.RecentFiles.First(r => r.Path == "Doc4").IsPinned = true;
            AssertHelper.SequenceEqual(new[] { "Doc4", "Doc3", "Doc1" }, recentFileList.RecentFiles.Select(f => f.Path));
            recentFileList.RecentFiles.First(r => r.Path == "Doc1").IsPinned = true;
            AssertHelper.SequenceEqual(new[] { "Doc1", "Doc4", "Doc3" }, recentFileList.RecentFiles.Select(f => f.Path));

            // Add a file to a full pinned list
            recentFileList.AddFile("Doc5");
            AssertHelper.SequenceEqual(new[] { "Doc1", "Doc4", "Doc3" }, recentFileList.RecentFiles.Select(f => f.Path));

            // Add a file that already exists in the list
            recentFileList.AddFile("Doc4");
            AssertHelper.SequenceEqual(new[] { "Doc4", "Doc1", "Doc3" }, recentFileList.RecentFiles.Select(f => f.Path));

            // Unpin files
            recentFileList.RecentFiles.First(r => r.Path == "Doc4").IsPinned = false;
            AssertHelper.SequenceEqual(new[] { "Doc1", "Doc3", "Doc4" }, recentFileList.RecentFiles.Select(f => f.Path));
            recentFileList.RecentFiles.First(r => r.Path == "Doc1").IsPinned = false;
            AssertHelper.SequenceEqual(new[] { "Doc3", "Doc1", "Doc4" }, recentFileList.RecentFiles.Select(f => f.Path));
        }

        [TestMethod]
        public void Remove()
        {
            var recentFileList = new RecentFileList();

            AssertHelper.ExpectedException<ArgumentNullException>(() => recentFileList.Remove(null!));

            recentFileList.AddFile("Doc1");
            recentFileList.AddFile("Doc2");
            recentFileList.AddFile("Doc3");

            RecentFile lastAdded = recentFileList.RecentFiles[0];

            recentFileList.Remove(recentFileList.RecentFiles.Last());
            AssertHelper.SequenceEqual(new[] { "Doc3", "Doc2" }, recentFileList.RecentFiles.Select(f => f.Path));
            recentFileList.Remove(recentFileList.RecentFiles[0]);
            AssertHelper.SequenceEqual(new[] { "Doc2" }, recentFileList.RecentFiles.Select(f => f.Path));
            recentFileList.Remove(recentFileList.RecentFiles[0]);
            Assert.IsTrue(!recentFileList.RecentFiles.Any());

            // Try to delete a RecentFile object which was already deleted.
            AssertHelper.ExpectedException<ArgumentException>(() => recentFileList.Remove(lastAdded));
        }

        [TestMethod]
        public void Clear()
        {
            var recentFileList = new RecentFileList();

            AssertHelper.ExpectedException<ArgumentNullException>(() => recentFileList.Remove(null!));

            recentFileList.AddFile("Doc1");
            recentFileList.AddFile("Doc2");
            recentFileList.AddFile("Doc3");

            recentFileList.Clear();
            Assert.AreEqual(0, recentFileList.RecentFiles.Count);
        }

        [TestMethod]
        public void XmlSerializing()
        {
            var serializer = new XmlSerializer(typeof(RecentFileList));

            // Serialize an empty list            
            using var stream1 = new MemoryStream();
            var recentFileList1 = new RecentFileList();
            serializer.Serialize(stream1, recentFileList1);
            stream1.Position = 0;
            var recentFileList2 = (RecentFileList)serializer.Deserialize(stream1)!;
            Assert.AreEqual(recentFileList1.RecentFiles.Count, recentFileList2.RecentFiles.Count);
            AssertHelper.SequenceEqual(recentFileList1.RecentFiles.Select(f => f.Path), recentFileList2.RecentFiles.Select(f => f.Path));

            // Serialize a list with items
            using var stream2 = new MemoryStream();
            recentFileList2.AddFile("Doc3");
            recentFileList2.AddFile("Doc2");
            recentFileList2.AddFile("Doc1");
            serializer.Serialize(stream2, recentFileList2);
            stream2.Position = 0;
            var recentFileList3 = (RecentFileList)serializer.Deserialize(stream2)!;
            AssertHelper.SequenceEqual(recentFileList2.RecentFiles.Select(f => f.Path), recentFileList3.RecentFiles.Select(f => f.Path));

            // Set MaxFilesNumber to a lower number
            recentFileList3.MaxFilesNumber = 2;
            AssertHelper.SequenceEqual(new[] { "Doc1", "Doc2" }, recentFileList3.RecentFiles.Select(f => f.Path));

            // Check error handling of the serializable implementation
            IXmlSerializable serializable = recentFileList3;
            Assert.IsNull(serializable.GetSchema());
            AssertHelper.ExpectedException<ArgumentNullException>(() => serializable.ReadXml(null!));
            AssertHelper.ExpectedException<ArgumentNullException>(() => serializable.WriteXml(null!));
        }

        [TestMethod]
        public void Load()
        {
            var recentFileList = new RecentFileList() { MaxFilesNumber = 3 };
            recentFileList.AddFile("Doc3");
            recentFileList.AddFile("Doc2");
            recentFileList.AddFile("Doc1");

            AssertHelper.ExpectedException<ArgumentNullException>(() => recentFileList.Load(null!));

            // Load an empty recent file list
            recentFileList.Load(new RecentFile[] { });
            Assert.IsFalse(recentFileList.RecentFiles.Any());

            recentFileList.Load(new[] 
            {
                new RecentFile("NewDoc1") { IsPinned = true },
                new RecentFile("NewDoc2"),
                new RecentFile("NewDoc3"),
                new RecentFile("NewDoc4")
            });
            AssertHelper.SequenceEqual(new[] { "NewDoc1", "NewDoc2", "NewDoc3" }, recentFileList.RecentFiles.Select(f => f.Path));
            AssertHelper.SequenceEqual(new[] { true, false, false }, recentFileList.RecentFiles.Select(f => f.IsPinned));
        }
    }
}
