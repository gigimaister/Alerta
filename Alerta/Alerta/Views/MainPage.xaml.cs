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
        }

        //Menu Clicked
        void hamburgerButton_Clicked(object sender, EventArgs e)
        {
            navigationDrawer.ToggleDrawer();
        }


        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
            navigationDrawer.ToggleDrawer();
        }
    }
}
