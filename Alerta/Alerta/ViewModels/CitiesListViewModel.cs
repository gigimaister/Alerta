using Alerta.Models;
using Alerta.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace Alerta.ViewModels
{
    public class CitiesListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        #region Fields

        public ICommand LocationAlertSettingsBtn;

        #endregion
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        IRestService _rest = DependencyService.Get<IRestService>();

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

        //Constractor
        public CitiesListViewModel()
        {
            GetCites();
            LocationAlertSettingsBtn = new Command(D);
        }


        public async void GetCites() 
        {
            var rslt = await _rest.GetAllCites(Urls.GetAllLocations);
            CitesList = rslt.result.records;
        }
        void D()
        {
            var e = 0;
        }
        
    }
}
