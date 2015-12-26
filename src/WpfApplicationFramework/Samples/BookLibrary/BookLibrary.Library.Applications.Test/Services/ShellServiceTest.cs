using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;
using Waf.BookLibrary.Library.Applications.Services;

namespace Test.BookLibrary.Library.Applications.Services
{
    [TestClass]
    public class ShellServiceTest
    {
        [TestMethod]
        public void SetViewTest()
        {
            ShellService shellService = new ShellService();
            object mockView = new object();

            AssertHelper.PropertyChangedEvent(shellService, x => x.ShellView, () =>
                shellService.ShellView = mockView);
            Assert.AreEqual(mockView, shellService.ShellView);
            
            AssertHelper.PropertyChangedEvent(shellService, x => x.BookListView, () =>
                shellService.BookListView = mockView);
            Assert.AreEqual(mockView, shellService.BookListView);

            AssertHelper.PropertyChangedEvent(shellService, x => x.BookView, () =>
                shellService.BookView = mockView);
            Assert.AreEqual(mockView, shellService.BookView);

            AssertHelper.PropertyChangedEvent(shellService, x => x.PersonListView, () =>
                shellService.PersonListView = mockView);
            Assert.AreEqual(mockView, shellService.PersonListView);

            AssertHelper.PropertyChangedEvent(shellService, x => x.PersonView, () =>
                shellService.PersonView = mockView);
            Assert.AreEqual(mockView, shellService.PersonView);

            bool isReportingEnabled = true;
            Lazy<object> lazyReportingView = new Lazy<object>(() => null);
            
            AssertHelper.PropertyChangedEvent(shellService, x => x.IsReportingEnabled, () =>
                shellService.IsReportingEnabled = isReportingEnabled);
            Assert.AreEqual(isReportingEnabled, shellService.IsReportingEnabled);

            AssertHelper.PropertyChangedEvent(shellService, x => x.LazyReportingView, () =>
                shellService.LazyReportingView = lazyReportingView);
            Assert.AreEqual(lazyReportingView, shellService.LazyReportingView);
        }
    }
}
