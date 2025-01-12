using Hotel.Core;
using Hotel.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Hotel.Core.Validators;
using Hotel.MVVM.View.PopUp;
using Hotel.Repositories;

namespace Hotel.MVVM.Viewmodel.PopUp
{
    internal class ReservationPopUpViewModel : ViewModel
    {
        private readonly IGuestsRepository _guestRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IAmenitiesRepository _amenitiesRepository;
        private Reservation _reservation;
        private string _guestName;
        private string _roomNumber;
        private string _validationError;
        private string _buttonName;
        private string _windowName;
        private Reservation _oldReservation;
        private List<AmenityItem> _updatedAmenitiesList;

        public Reservation Reservation
        {
            get => _reservation;
            set
            {
                _reservation = value;
                OnPropertyChanged();
            }
        }

        public string GuestName
        {
            get => _guestName;
            set
            {
                _guestName = value;
                OnPropertyChanged();
            }
        }

        public string RoomNumber
        {
            get => _roomNumber;
            set
            {
                _roomNumber = value;
                OnPropertyChanged();
            }
        }

        public string ValidationError
        {
            get => _validationError;
            set
            {
                _validationError = value;
                OnPropertyChanged();
            }
        }

        public string ButtonName
        {
            get => _buttonName;
            set
            {
                _buttonName = value;
                OnPropertyChanged();
            }
        }

        public string WindowName
        {
            get => _windowName;
            set
            {
                _windowName = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand PickGuestCommand { get; set; }
        public RelayCommand PickRoomCommand { get; set; }
        public RelayCommand EditAmenitiesCommand { get; set; }

        public ReservationPopUpViewModel(string windowName, string buttonName, Reservation reservation, IGuestsRepository guestRepository, IRoomRepository roomRepository, IReservationRepository reservationRepository, IAmenitiesRepository amenitiesRepository)
        {
            WindowName = windowName;
            ButtonName = buttonName;
            Reservation = reservation;
            _guestRepository = guestRepository;
            _roomRepository = roomRepository;
            _reservationRepository = reservationRepository;
            _amenitiesRepository = amenitiesRepository;
            GuestName = reservation.Guest != null ? $"{reservation.Guest.Name} {reservation.Guest.Surname}" : "Nie wybrano";
            RoomNumber = reservation.Room != null ? reservation.Room.RoomNumber.ToString() : "Nie wybrano";
            _oldReservation = new Reservation(reservation);
            _updatedAmenitiesList = new List<AmenityItem>();

            ConfirmCommand = new RelayCommand(Confirm, o => ValidateAndConfirm());
            CancelCommand = new RelayCommand(Cancel, o => true);
            PickGuestCommand = new RelayCommand(o => PickGuest(), o => true);
            PickRoomCommand = new RelayCommand(o => PickRoom(), o => true);
            EditAmenitiesCommand = new RelayCommand(o => EditAmenities(), o => true);
        }

        private void EditAmenities()
        {
            var amenityItemsForReservation= _reservationRepository.GetAmenitiesForReservation(_reservation.Id).ToList();
            var oldAmenitiesPrice = amenityItemsForReservation.Sum(a => a.Amenity.PricePerNight * (_reservation.EndDate.DayNumber - _reservation.StartDate.DayNumber) * a.Quantity);
            var allAmenityItems = _amenitiesRepository.GetAllAmenities();
            foreach (var amenityItem in allAmenityItems)
            {
                if (amenityItemsForReservation.All(x => x.Amenity.Id != amenityItem.Id))
                {
                    amenityItemsForReservation.Add(new AmenityItem(amenityItem, 0));
                }
            }
            var dialog = new AddAmenitiesPopUpView()
            {
                DataContext = new AddAmenitiesPopUpViewModel(amenityItemsForReservation)
            };
            var amenityTotalPrice = 0.0;
            if (dialog.ShowDialog() == true)
            {
                var viewModel = dialog.DataContext as AddAmenitiesPopUpViewModel;
                var amenities = viewModel?.AmenitiesCollection;
                _updatedAmenitiesList.Clear();
                if (amenities != null)
                {
                    foreach (var amenity in amenities)
                    {
                        if (amenity.Quantity > 0)
                        {
                            var amenityPrice = amenity.Quantity * amenity.Amenity.PricePerNight *
                                               (_reservation.EndDate.DayNumber - _reservation.StartDate.DayNumber);
                            amenityTotalPrice += amenityPrice;
                            _updatedAmenitiesList.Add(amenity);
                        }
                    }
                }
            }

            var newReservation = new Reservation(Reservation);
            newReservation.TotalPrice += amenityTotalPrice - oldAmenitiesPrice;
            Reservation = newReservation;
        }

        private void PickGuest()
        {
            var dialog = new GuestPickerView()
            {
                DataContext = new GuestPickerViewModel(_guestRepository)
            };

            if (dialog.ShowDialog() == true)
            {
                var viewModel = dialog.DataContext as GuestPickerViewModel;
                var guest = viewModel?.SelectedGuest;

                if (guest != null)
                {
                    Reservation.Guest = guest;
                    GuestName = $"{guest.Name} {guest.Surname}";
                }
            }
        }

        private void PickRoom()
        {
            var dialog = new RoomPickerView()
            {
                DataContext = new RoomPickerViewModel(_roomRepository)
            };

            if (dialog.ShowDialog() == true)
            {
                var viewModel = dialog.DataContext as RoomPickerViewModel;
                var room = viewModel?.SelectedRoom;

                if (room != null)
                {
                    Reservation.Room = room;
                    RoomNumber = room.RoomNumber.ToString();
                }
            }
        }

        private void Confirm(object parameter)
        {
            _reservationRepository.UpdateReservation(Reservation);
            _reservationRepository.DeleteAllAmenitiesForReservation(Reservation.Id);
            foreach (var amenity in _updatedAmenitiesList)
            {
                var amenityPrice = amenity.Quantity * amenity.Amenity.PricePerNight *
                                   (_reservation.EndDate.DayNumber - _reservation.StartDate.DayNumber);
                _amenitiesRepository.AddAmenityToReservation(amenity.Amenity, Reservation.Id, amenity.Quantity, amenityPrice);
            }
            if (parameter is Window window)
            {
                window.DialogResult = true;
                window.Close();
            }
        }

        private void Cancel(object parameter)
        {
            Reservation = _oldReservation;
            if (parameter is Window window)
            {
                window.DialogResult = false;
                window.Close();
            }
        }

        private bool ValidateAndConfirm()
        {
            var isValid = Validator.Validate(Reservation);
            ValidationError = !isValid ? "Please fix the data." : string.Empty;

            return isValid;
        }
    }
}
