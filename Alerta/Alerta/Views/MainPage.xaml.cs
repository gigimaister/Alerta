using Alerta.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Alerta
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            hamburgerButton.ImageSource = (FileImageSource)ImageSource.FromFile("hambMenu48.png");

            List<string> list = new List<string>();
            list.Add("היסטוריה");
            list.Add("בדיקה");
            list.Add("הגדרות");
            
            listView.ItemsSource = list;
        }

        //Menu Clicked
        void HamburgerButton_Clicked(object sender, EventArgs e)
        {
            navigationDrawer.ToggleDrawer();
        }

        //On Menu Item Clicked
        private void ListView_ItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new SettingsPage());

            navigationDrawer.ToggleDrawer();
        }
    }
}
