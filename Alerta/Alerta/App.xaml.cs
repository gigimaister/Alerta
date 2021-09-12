using Alerta.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Alerta
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDk5ODgyQDMxMzkyZTMyMmUzMFFiYkxKMnBVNWZPSTl2V2ZDTHl6cEQzLzdNY1E4eEJVSUwrR3dFYlBKZjA9");
            InitializeComponent();

            MainPage = new NavigationPage(new SplashScreen());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
