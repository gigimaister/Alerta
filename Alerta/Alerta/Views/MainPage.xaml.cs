using System;
using System.Collections.Generic;
using GProof.Alerta.Models;
using Xamarin.Forms;

namespace GProof.Alerta.Views
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
