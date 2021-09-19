using Alerta.Http;
using Alerta.Models;
using Alerta.Views;
using System;
using System.Collections.Generic;
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
            
            LoadDataAsync();
          
        }
        async void  LoadDataAsync()
        {
            RestService restService = new RestService();
            var result = await restService.Get<RestService>(Urls.GetAllLocations);
        }

        //Menu Clicked
        void HamburgerButton_Clicked(object sender, EventArgs e)
        {
            navigationDrawer.ToggleDrawer();
        }

        //On Menu Item Clicked
        private async void ListView_ItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItemIndex == (int)MainMenu.settings)
            {              
                await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage());
                navigationDrawer.ToggleDrawer();
                listView.SelectedItem = null;
            }         
        }
    }
}
