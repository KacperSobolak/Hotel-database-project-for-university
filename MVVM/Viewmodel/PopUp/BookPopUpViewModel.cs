using System;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Input;
using Hotel.Core;
using Hotel.MVVM.Model;
using Hotel.MVVM.View.PopUp;
using Hotel.Repositories;

namespace Hotel.MVVM.Viewmodel.PopUp
{
    internal class BookPopUpViewModel : ViewModel
    {
        private IAmenitiesRepository _amenitiesRepository;
        private IReservationRepository _reservationRepository;
        private Reservation _reservation;
        public List<AmenityItem> AmenityItems;
        public ObservableCollection<AmenityItem> AmenityItemsToShow { get; set; }
        private string _roomDetails;
        private double _amenitiesPrice;
        private double _totalPrice;
        private string _totalPriceText;

        public string RoomDetails
        {
            get => _roomDetails;
            set
            {
                _roomDetails = value;
                OnPropertyChanged();
            }
        }
            
        public string TotalPriceText
        {
            get => _totalPriceText;
            set
            {
                _totalPriceText = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand AddAmenitiesCommand { get; set; }

        public BookPopUpViewModel(Reservation reservation, IAmenitiesRepository amenitiesRepository, IReservationRepository reservationRepository)
        {
            _reservation = reservation;
            _amenitiesRepository = amenitiesRepository;
            _reservationRepository = reservationRepository;
            AmenityItems = _amenitiesRepository
                .GetAmenitiesAvailableForDateRange(_reservation.StartDate, _reservation.EndDate)
                .Select(amenity => new AmenityItem(amenity, 0)).ToList();
            AmenityItemsToShow = new ObservableCollection<AmenityItem>();

            _amenitiesPrice = 0;
            CalculateTotalPrice();
            RoomDetails = $@"Pokój: {_reservation.Room.RoomNumber}
            Kategoria: {_reservation.Room.CategoryName}
            Gość: {_reservation.Guest.Name} {_reservation.Guest.Surname}
            Data rozpoczęcia: {_reservation.StartDate}
            Data zakończenia: {_reservation.EndDate}
            Cena bazowa pokoju: {_reservation.TotalPrice}";

            _amenitiesRepository = amenitiesRepository;

            ConfirmCommand = new RelayCommand(Confirm, o => true);
            CancelCommand = new RelayCommand(Cancel, o => true);
            AddAmenitiesCommand = new RelayCommand(AddAmenities, o => true);
        }

        private void CalculateTotalPrice()
        {
            _totalPrice = _reservation.TotalPrice + _amenitiesPrice;
            TotalPriceText = $"Cena całkowita: {_totalPrice}";
        }

        private void AddAmenities(object parameter)
        {
            var dialog = new AddAmenitiesPopUpView()
            {
                DataContext = new AddAmenitiesPopUpViewModel(AmenityItems)
            };

            if (dialog.ShowDialog() == true)
            {
                var viewModel = dialog.DataContext as AddAmenitiesPopUpViewModel;
                var amenities = viewModel?.AmenitiesCollection;

                AmenityItems.Clear();
                AmenityItemsToShow.Clear();
                _amenitiesPrice = 0;
                if (amenities != null)
                {
                    foreach (var amenity in amenities)
                    {
                        AmenityItems.Add(amenity);
                        if (amenity.Quantity > 0)
                        {
                            _amenitiesPrice += amenity.Quantity * amenity.Amenity.PricePerNight * 
                                               (_reservation.EndDate.DayNumber - _reservation.StartDate.DayNumber);
                            AmenityItemsToShow.Add(amenity);
                        }
                    }
                }
                CalculateTotalPrice();
            }
        }

        private void Confirm(object parameter)
        {
            _reservation.TotalPrice = _totalPrice;
            var reservationId = _reservationRepository.AddReservation(_reservation);
            foreach (var amenity in AmenityItemsToShow.ToList())
            {
                var amenityPrice = amenity.Quantity * amenity.Amenity.PricePerNight *
                                  (_reservation.EndDate.DayNumber - _reservation.StartDate.DayNumber);
                _amenitiesRepository.AddAmenityToReservation(amenity.Amenity, reservationId, amenity.Quantity, amenityPrice);
            }
            if (parameter is Window window)
            {
                window.DialogResult = true;
                window.Close();
            }
        }

        private void Cancel(object parameter)
        {
            if (parameter is Window window)
            {
                window.DialogResult = false;
                window.Close();
            }
        }
    }
}