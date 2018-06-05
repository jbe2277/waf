using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;
using Waf.InformationManager.Infrastructure.Modules.Applications.Views;

namespace Test.InformationManager.Infrastructure.Modules.Applications.Views
{
    [Export(typeof(IShellView)), Export]
    public class MockShellView : MockView, IShellView
    {
        private readonly List<ToolBarCommand> toolBarCommands;

        public MockShellView()
        {
            toolBarCommands = new List<ToolBarCommand>();
        }

        public IReadOnlyList<ToolBarCommand> ToolBarCommands => toolBarCommands;

        public bool IsVisible { get; private set; }

        public double VirtualScreenWidth { get; set; }
        
        public double VirtualScreenHeight { get; set; }
        
        public double Left { get; set; }
        
        public double Top { get; set; }
        
        public double Width { get; set; }
        
        public double Height { get; set; }
        
        public bool IsMaximized { get; set; }
        
        public event EventHandler Closed;

        public void Show()
        {
            IsVisible = true;
        }

        public void Close()
        {
            IsVisible = false;
            OnClosed(EventArgs.Empty);
        }

        public void AddToolBarCommands(IReadOnlyList<ToolBarCommand> commands)
        {
            toolBarCommands.AddRange(commands);    
        }

        public void ClearToolBarCommands()
        {
            toolBarCommands.Clear();
        }

        public void SetNAForLocationAndSize()
        {
            Top = double.NaN;
            Left = double.NaN;
            Width = double.NaN;
            Height = double.NaN;
        }

        protected virtual void OnClosed(EventArgs e)
        {
            Closed?.Invoke(this, e);
        }
    }
}
