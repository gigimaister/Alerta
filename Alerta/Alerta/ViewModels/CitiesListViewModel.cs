using Alerta.Models;
using Alerta.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace Alerta.ViewModels
{
    public class CitiesListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
        public CitiesListViewModel()
        {
            GetCites();
        }


        public async void GetCites() 
        {
            var rslt = await _rest.GetAllCites(Urls.GetAllLocations);
            CitesList = rslt.records;
        }
    }
}
