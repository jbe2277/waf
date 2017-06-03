using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Applications.Views;
using System;
using System.Collections.ObjectModel;
using System.Composition;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.System.RemoteSystems;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Jbe.NewsReader.Presentation.Views
{
    [Export(typeof(IDeveloperSettingsView)), Shared]
    public sealed partial class DeveloperSettingsView : UserControl, IDeveloperSettingsView
    {
        private readonly Lazy<DeveloperSettingsViewModel> viewModel;
        private readonly ObservableCollection<RemoteSystem> devices;
        private RemoteSystemWatcher remoteSystemWatcher;


        public DeveloperSettingsView()
        {
            InitializeComponent();
            viewModel = new Lazy<DeveloperSettingsViewModel>(() => (DeveloperSettingsViewModel)DataContext);
            devices = new ObservableCollection<RemoteSystem>();
            devicesList.ItemsSource = devices;
            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
        }

        
        public DeveloperSettingsViewModel ViewModel => viewModel.Value;


        private async void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (remoteSystemWatcher == null)
            {
                var accessStatus = await RemoteSystem.RequestAccessAsync();
                if (accessStatus == RemoteSystemAccessStatus.Allowed)
                {
                    remoteSystemStatusText.Text = "Go";
                    remoteSystemWatcher = RemoteSystem.CreateWatcher();

                    remoteSystemWatcher.RemoteSystemAdded += RemoteSystemAdded;
                }
            }
            remoteSystemWatcher?.Start();
        }

        private void UnloadedHandler(object sender, RoutedEventArgs e)
        {
            remoteSystemWatcher?.Stop();
        }

        private async void RemoteSystemAdded(RemoteSystemWatcher sender, RemoteSystemAddedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                remoteSystemStatusText.Text = "Found";
                devices.Add(args.RemoteSystem);
            });
        }

        private async void SendRemoteMessageClick(object sender, RoutedEventArgs e)
        {
            var remoteSystem = devicesList.SelectedItem as RemoteSystem;
            if (remoteSystem == null) return;

            var connectionRequest = new RemoteSystemConnectionRequest(remoteSystem);
            using (var connection = new AppServiceConnection { AppServiceName = "Jbe.NewsReader.MyAppService", PackageFamilyName = Package.Current.Id.FamilyName }) //, User = User.GetFromId })
            {
                var connectionStatus = await connection.OpenRemoteAsync(connectionRequest);
                if (connectionStatus == AppServiceConnectionStatus.Success)
                {
                    var parameters = new ValueSet() { { "value", 42 } };
                    var response = await connection.SendMessageAsync(parameters);
                    if (response.Status == AppServiceResponseStatus.Success)
                    {
                        resultText.Text = response.Message["result"].ToString();
                    }
                }
            }
        }
    }
}
