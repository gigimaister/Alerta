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

        #region Fields

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SetLocationPreferenseCommand { private set; get; }

        //For Http Req
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
        //GET 
        public async void GetCites()
        {
            var rslt = await _rest.GetAllCites(Urls.GetAllLocations);
            CitesList = rslt.result.records;
             CitesList = Methods.SetHebrewParent(CitesList);
        }
        //Pref Button
        void SetLocationPreferense()
        {
            var e = SelectedIndices;
        }

        #endregion

    }
}
