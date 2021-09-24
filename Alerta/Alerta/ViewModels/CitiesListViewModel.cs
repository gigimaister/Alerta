using Alerta.Models;
using Alerta.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Alerta.ViewModels
{
    public class CitiesListViewModel : INotifyPropertyChanged
    {

        #region Fields

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SetLocationPreferenseCommand { private set; get; }

        //For Http Req
        IRestService _rest = DependencyService.Get<IRestService>();

        //For Geo Location Coordinates
        IGeoLocationService _gps = DependencyService.Get<IGeoLocationService>();

        //Lat Lon Coordinates List
        public Dictionary<string, double> LatLonCoordinates { private set; get; }

        private ObservableCollection<GovCity> citesList;

        public ObservableCollection<GovCity> CitesList
        {
            get => citesList;
            set
            {
                citesList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CitesList"));
            }
        }

        //Contains One Or More City Locations From The ComboBox
        private object selectedIndices;
        public object SelectedIndices
        {
            get => selectedIndices;
            set
            {
                selectedIndices = value;
                NotifyPropertyChanged("SelectedIndices");

            }
        }

        //Loading Spinner
        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value;
                NotifyPropertyChanged("IsBusy");
            }
        }


        #endregion

        //Constractor
        public CitiesListViewModel()
        {
            IsBusy = true;
            GetCurrentGeoLocation();
            GetCites();
            SetLocationPreferenseCommand = new Command(SetLocationPreferense);
            IsBusy = false;
        }

        #region PropertyChanged
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Methods
        //GET Cities
        public async void GetCites()
        {
            var rslt = await _rest.GetAllCites(Urls.GetAllLocations);
            CitesList = rslt.result.records;
             CitesList = Methods.SetHebrewParent(CitesList);
        }
        //GeoLocation Coordinates
        public async void GetCurrentGeoLocation()
        {
            LatLonCoordinates = await _gps.GetCurrentLocation();
            var placemarks = await Geocoding.GetPlacemarksAsync(LatLonCoordinates
                .Where(x => x.Key=="Lat").Select(x => x.Value).FirstOrDefault()
                , LatLonCoordinates.Where(x => x.Key == "Lon").Select(x => x.Value).FirstOrDefault());
            //Need To Add Try Catch Here!
        }
        //Pref Button
        void SetLocationPreferense()
        {
            var e = SelectedIndices;
        }

        #endregion

    }
}
