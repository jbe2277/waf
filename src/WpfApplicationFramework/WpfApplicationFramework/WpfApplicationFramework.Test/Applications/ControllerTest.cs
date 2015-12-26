using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Foundation;
using System.Waf.Applications;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Waf.UnitTesting;

namespace Test.Waf.Applications
{
    [TestClass]
    public class ControllerTest
    {
        // We have to do every test twice because the Controller and ViewModel class contains the same
        // weak event pattern implementation.
        
        [TestMethod]
        public void AddWeakEventListenerTest() 
        {
            // Add a weak event listener and check if the eventhandler is called
            DocumentManager documentManager = new DocumentManager();
            DocumentController controller = new DocumentController(documentManager);
            Assert.IsFalse(controller.DocumentsHasChanged);
            Assert.IsFalse(controller.ActiveDocumentHasChanged);
            documentManager.Open();
            Assert.IsTrue(controller.DocumentsHasChanged);
            Assert.IsTrue(controller.ActiveDocumentHasChanged);

            // Remove the weak event listener and check that the eventhandler is not anymore called
            controller.RemoveWeakEventListeners();
            controller.DocumentsHasChanged = false;
            controller.ActiveDocumentHasChanged = false;
            documentManager.Open();
            Assert.IsFalse(controller.DocumentsHasChanged);
            Assert.IsFalse(controller.ActiveDocumentHasChanged);

            // Remove again the same weak event listeners although they are not anymore registered
            controller.RemoveWeakEventListeners();

            // Check that the garbage collector is able to collect the controller although the service lives longer
            controller.AddWeakEventListeners();
            documentManager.Open();
            Assert.IsTrue(controller.DocumentsHasChanged);
            Assert.IsTrue(controller.ActiveDocumentHasChanged);
            WeakReference weakController = new WeakReference(controller);
            controller = null;
            GC.Collect();

            Assert.IsFalse(weakController.IsAlive);
        }

        [TestMethod]
        public void AddWeakEventListenerTest2()
        {
            // Add a weak event listener and check if the eventhandler is called
            DocumentManager documentManager = new DocumentManager();
            ShellViewModel viewModel = new ShellViewModel(new MockView(), documentManager);
            Assert.IsFalse(viewModel.DocumentsHasChanged);
            Assert.IsFalse(viewModel.ActiveDocumentHasChanged);
            documentManager.Open();
            Assert.IsTrue(viewModel.DocumentsHasChanged);
            Assert.IsTrue(viewModel.ActiveDocumentHasChanged);

            // Remove the weak event listener and check that the eventhandler is not anymore called
            viewModel.RemoveWeakEventListeners();
            viewModel.DocumentsHasChanged = false;
            viewModel.ActiveDocumentHasChanged = false;
            documentManager.Open();
            Assert.IsFalse(viewModel.DocumentsHasChanged);
            Assert.IsFalse(viewModel.ActiveDocumentHasChanged);

            // Remove again the same weak event listeners although they are not anymore registered
            viewModel.RemoveWeakEventListeners();

            // Check that the garbage collector is able to collect the controller although the service lives longer
            viewModel.AddWeakEventListeners();
            documentManager.Open();
            Assert.IsTrue(viewModel.DocumentsHasChanged);
            Assert.IsTrue(viewModel.ActiveDocumentHasChanged);
            WeakReference weakController = new WeakReference(viewModel);
            viewModel = null;
            GC.Collect();

            Assert.IsFalse(weakController.IsAlive);
        }

        [TestMethod]
        public void ArgumentNullTest()
        {
            DocumentManager documentManager = new DocumentManager();
            DocumentController controller = new DocumentController(documentManager);
            ShellViewModel shellViewModel = new ShellViewModel(new MockView(), documentManager);
            
            AssertHelper.ExpectedException<ArgumentNullException>(() => controller.AddWeakEventListener((INotifyPropertyChanged)null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => controller.AddWeakEventListener(documentManager, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => controller.RemoveWeakEventListener((INotifyPropertyChanged)null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => controller.RemoveWeakEventListener(documentManager, null));

            AssertHelper.ExpectedException<ArgumentNullException>(() => controller.AddWeakEventListener((INotifyCollectionChanged)null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => controller.AddWeakEventListener((INotifyCollectionChanged)documentManager.Documents, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => controller.RemoveWeakEventListener((INotifyCollectionChanged)null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => controller.RemoveWeakEventListener((INotifyCollectionChanged)documentManager.Documents, null));

            AssertHelper.ExpectedException<ArgumentNullException>(() => shellViewModel.AddWeakEventListener((INotifyPropertyChanged)null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => shellViewModel.AddWeakEventListener(documentManager, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => shellViewModel.RemoveWeakEventListener((INotifyPropertyChanged)null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => shellViewModel.RemoveWeakEventListener(documentManager, null));

            AssertHelper.ExpectedException<ArgumentNullException>(() => shellViewModel.AddWeakEventListener((INotifyCollectionChanged)null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => shellViewModel.AddWeakEventListener((INotifyCollectionChanged)documentManager.Documents, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => shellViewModel.RemoveWeakEventListener((INotifyCollectionChanged)null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => shellViewModel.RemoveWeakEventListener((INotifyCollectionChanged)documentManager.Documents, null));

            AssertHelper.ExpectedException<ArgumentNullException>(() => new PropertyChangedEventListener(null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => new PropertyChangedEventListener(documentManager, null));

            AssertHelper.ExpectedException<ArgumentNullException>(() => new CollectionChangedEventListener(null, null));
            AssertHelper.ExpectedException<ArgumentNullException>(() => new CollectionChangedEventListener((INotifyCollectionChanged)documentManager.Documents, null));
        }

#pragma warning disable 618
        private class DocumentController : Controller
        {
            private readonly IDocumentManager documentManager;


            public DocumentController(IDocumentManager documentManager)
            {
                this.documentManager = documentManager;
                AddWeakEventListeners();
            }


            public bool DocumentsHasChanged { get; set; }
            public bool ActiveDocumentHasChanged { get; set; }


            public void AddWeakEventListeners()
            {
                AddWeakEventListener(documentManager.Documents, DocumentsCollectionChanged);
                AddWeakEventListener(documentManager, DocumentManagerPropertyChanged);
            }

            public void RemoveWeakEventListeners()
            {
                RemoveWeakEventListener(documentManager.Documents, DocumentsCollectionChanged);
                RemoveWeakEventListener(documentManager, DocumentManagerPropertyChanged);
            }

            public new void AddWeakEventListener(INotifyPropertyChanged source, PropertyChangedEventHandler handler)
            {
                base.AddWeakEventListener(source, handler);
            }

            public new void RemoveWeakEventListener(INotifyPropertyChanged source, PropertyChangedEventHandler handler)
            {
                base.RemoveWeakEventListener(source, handler);
            }

            public new void AddWeakEventListener(INotifyCollectionChanged source, NotifyCollectionChangedEventHandler handler)
            {
                base.AddWeakEventListener(source, handler);
            }

            public new void RemoveWeakEventListener(INotifyCollectionChanged source, NotifyCollectionChangedEventHandler handler)
            {
                base.RemoveWeakEventListener(source, handler);
            }

            private void DocumentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                DocumentsHasChanged = true;
            }

            private void DocumentManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "ActiveDocument")
                {
                    ActiveDocumentHasChanged = true;
                }
            }
        }
#pragma warning restore 618

#pragma warning disable 618
        private class ShellViewModel : ViewModel
        {
            private readonly IDocumentManager documentManager;


            public ShellViewModel(IView view, IDocumentManager documentManager) : base(view)
            {
                this.documentManager = documentManager;
                AddWeakEventListeners();
            }


            public bool DocumentsHasChanged { get; set; }
            public bool ActiveDocumentHasChanged { get; set; }

            
            public void AddWeakEventListeners()
            {
                AddWeakEventListener(documentManager.Documents, DocumentsCollectionChanged);
                AddWeakEventListener(documentManager, DocumentManagerPropertyChanged);
            }

            public void RemoveWeakEventListeners()
            {
                RemoveWeakEventListener(documentManager.Documents, DocumentsCollectionChanged);
                RemoveWeakEventListener(documentManager, DocumentManagerPropertyChanged);
            }

            public new void AddWeakEventListener(INotifyPropertyChanged source, PropertyChangedEventHandler handler)
            {
                base.AddWeakEventListener(source, handler);
            }

            public new void RemoveWeakEventListener(INotifyPropertyChanged source, PropertyChangedEventHandler handler)
            {
                base.RemoveWeakEventListener(source, handler);
            }

            public new void AddWeakEventListener(INotifyCollectionChanged source, NotifyCollectionChangedEventHandler handler)
            {
                base.AddWeakEventListener(source, handler);
            }

            public new void RemoveWeakEventListener(INotifyCollectionChanged source, NotifyCollectionChangedEventHandler handler)
            {
                base.RemoveWeakEventListener(source, handler);
            }

            private void DocumentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                DocumentsHasChanged = true;
            }

            private void DocumentManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "ActiveDocument")
                {
                    ActiveDocumentHasChanged = true;
                }
            }
        }
#pragma warning restore 618

        private class MockView : IView
        {
            public object DataContext { get; set; }
        }

        private interface IDocumentManager : INotifyPropertyChanged
        {
            ObservableCollection<object> Documents { get; }
            
            object ActiveDocument { get; }
        }

        private class DocumentManager : Model, IDocumentManager
        {
            private readonly ObservableCollection<object> documents = new ObservableCollection<object>();
            private object activeDocument;


            public ObservableCollection<object> Documents { get { return documents; } }

            public object ActiveDocument 
            { 
                get { return activeDocument; } 
                private set { SetProperty(ref activeDocument, value); }
            }


            public void Open()
            {
                Documents.Add(new object());
                ActiveDocument = Documents.Last();
            }
        }
    }
}
