using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;
using Waf.InformationManager.Infrastructure.Modules.Applications.ViewModels;
using Waf.InformationManager.Infrastructure.Modules.Applications.Views;

namespace Waf.InformationManager.Infrastructure.Modules.Presentation.Views
{
    [Export(typeof(IShellView))]
    public partial class ShellWindow : Window, IShellView
    {
        private readonly Lazy<ShellViewModel> viewModel;
        private readonly List<Control> dynamicToolBarItems;

        
        public ShellWindow()
        {
            InitializeComponent();
            viewModel = new Lazy<ShellViewModel>(() => ViewHelper.GetViewModel<ShellViewModel>(this));
            dynamicToolBarItems = new List<Control>();
            Loaded += LoadedHandler;
        }


        public double VirtualScreenWidth { get { return SystemParameters.VirtualScreenWidth; } }

        public double VirtualScreenHeight { get { return SystemParameters.VirtualScreenHeight; } }

        public bool IsMaximized
        {
            get { return WindowState == WindowState.Maximized; }
            set
            {
                if (value)
                {
                    WindowState = WindowState.Maximized;
                }
                else if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
            }
        }

        private ShellViewModel ViewModel { get { return viewModel.Value; } }


        public void AddToolBarCommands(IReadOnlyList<ToolBarCommand> commands)
        {
            int index = 0;
            foreach (var command in commands)
            {
                AccessText accessText = new AccessText() { Text = command.Text };
                Button button = new Button() { Content = accessText, Command = command.Command };
                if (!string.IsNullOrEmpty(command.ToolTip)) { button.ToolTip = command.ToolTip; }

                toolBar.Items.Insert(index, button);
                dynamicToolBarItems.Add(button);
                index += 1;
            }
        }

        public void ClearToolBarCommands()
        {
            dynamicToolBarItems.ForEach(x => toolBar.Items.Remove(x));
            dynamicToolBarItems.Clear();
        }
        
        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            var firstNode = ViewModel.NavigationService.NavigationNodes.FirstOrDefault();
            if (firstNode != null) { firstNode.IsSelected = true; }
        }
    }
}
