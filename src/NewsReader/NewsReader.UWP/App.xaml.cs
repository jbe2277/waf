using Autofac;
using Waf.NewsReader.Applications;
using Waf.NewsReader.Presentation;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Waf.NewsReader.UWP
{
    public sealed partial class App : Application
    {
        public App()
        {
            Presentation.App.InitializeLogging(Log.Default);

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ApplicationsModule());
            builder.RegisterModule(new PresentationModule());
            builder.RegisterModule(new UwpModule());
            Container = builder.Build();

            InitializeComponent();
        }

        public IContainer Container { get; }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();
                Xamarin.Forms.Forms.Init(e);
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            Window.Current.Activate();
        }
    }
}
