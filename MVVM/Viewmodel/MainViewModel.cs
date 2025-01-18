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
        public RelayCommand NavigateToReservationsCommand { get; set; }
        public RelayCommand NavigateToRoomsCommand { get; set; }
        public RelayCommand NavigateToCategoriesCommand { get; set; }
        public RelayCommand NavigateToGuestsCommand { get; set; }
        public RelayCommand NavigateToBookCommand { get; set; }
        public RelayCommand NavigateToAmenitiesCommand { get; set; }
        public RelayCommand NavigateToStatisticsCommand { get; set; }

        public MainViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigateToHomeCommand =
                new RelayCommand(o => { NavigationService.NavigateTo<HomeViewModel>(); }, o => true);
            NavigateToReservationsCommand =
                new RelayCommand(o => { NavigationService.NavigateTo<ReservationsViewModel>(); }, o => true);
            NavigateToRoomsCommand =
                new RelayCommand(o => { NavigationService.NavigateTo<RoomsViewModel>(); }, o => true);
            NavigateToCategoriesCommand =
                new RelayCommand(o => { NavigationService.NavigateTo<CategoriesViewModel>(); }, o => true);
            NavigateToGuestsCommand =
                new RelayCommand(o => { NavigationService.NavigateTo<GuestsViewModel>(); }, o => true);
            NavigateToBookCommand =
                new RelayCommand(o => { NavigationService.NavigateTo<BookViewModel>(); }, o => true);
            NavigateToAmenitiesCommand =
                new RelayCommand(o => { NavigationService.NavigateTo<AmenitiesViewModel>(); }, o => true);
            NavigateToStatisticsCommand =
                new RelayCommand(o => { NavigationService.NavigateTo<StatisticViewModel>(); }, o => true);
        }
    }
}
