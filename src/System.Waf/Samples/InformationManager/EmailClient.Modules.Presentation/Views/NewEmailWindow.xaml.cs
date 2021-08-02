using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.Views
{
    [Export(typeof(INewEmailView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class NewEmailWindow : INewEmailView
    {
        public NewEmailWindow()
        {
            InitializeComponent();
            Loaded += LoadedHandler;
        }

        public void Show(object owner)
        {
            Owner = owner as Window;
            Show();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e) => toBox.Focus();

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            // This is a workaround. Without this line the main window might hide behind another running application.
            Application.Current.MainWindow.Activate();
        }

        private void SendButtonClick(object sender, RoutedEventArgs e)
        {
            // The Send button is a toolbar button which doesn't take the focus. Move the focus so that the Binding updates the source before calling the SendCommand.
            CommitChanges();
        }

        private static void CommitChanges()
        {
            var element = Keyboard.FocusedElement as FrameworkElement;
            element?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            element?.Focus();
        }
    }
}
