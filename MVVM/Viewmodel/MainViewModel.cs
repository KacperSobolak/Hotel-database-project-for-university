using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.Core;
using Hotel.Services;

namespace Hotel.MVVM.Viewmodel
{
    internal class MainViewModel : ViewModel
    {
        private INavigationService _navigationService;

        public INavigationService NavigationService
        {
            get => _navigationService;
            set
            {
                _navigationService = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand NavigateToHomeCommand { get; set; }
        public RelayCommand NavigateToReservationCommand { get; set; }

        public MainViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigateToHomeCommand = new RelayCommand(o => { NavigationService.NavigateTo<HomeViewModel>(); }, o => true);
            NavigateToReservationCommand = new RelayCommand(o => { NavigationService.NavigateTo<ReservationsViewModel>(); }, o => true);
        }
    }
}
