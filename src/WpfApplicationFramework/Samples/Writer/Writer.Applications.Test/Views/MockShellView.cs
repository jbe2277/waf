using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.Writer.Applications.Views;

namespace Test.Writer.Applications.Views
{
    [Export(typeof(IShellView)), Export]
    public class MockShellView : MockView, IShellView
    {
        public bool IsVisible { get; private set; }

        public double Left { get; set; }
        
        public double Top { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public bool IsMaximized { get; set; }
        
        
        public event CancelEventHandler Closing;

        public event EventHandler Closed;


        public void Show()
        {
            IsVisible = true;
        }

        public void Close()
        {
            CancelEventArgs e = new CancelEventArgs();
            OnClosing(e);
            if (!e.Cancel)
            {
                IsVisible = false;
                OnClosed(EventArgs.Empty);
            }
        }

        public void SetNAForLocationAndSize()
        {
            Top = double.NaN;
            Left = double.NaN;
            Width = double.NaN;
            Height = double.NaN;
        }

        protected virtual void OnClosing(CancelEventArgs e)
        {
            Closing?.Invoke(this, e);
        }

        protected virtual void OnClosed(EventArgs e)
        {
            Closed?.Invoke(this, e);
        }
    }
}
