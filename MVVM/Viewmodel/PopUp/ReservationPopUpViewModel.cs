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
        private Reservation _reservation;
        private string _guestName;
        private string _validationError;
        private string _buttonName;
        private string _windowName;

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

        public ReservationPopUpViewModel(string windowName, string buttonName, Reservation reservation, IGuestsRepository guestRepository)
        {
            WindowName = windowName;
            ButtonName = buttonName;
            Reservation = reservation;
            _guestRepository = guestRepository;
            GuestName = reservation.Guest != null ? $"{reservation.Guest.Name} {reservation.Guest.Surname}" : "Nie wybrano";

            ConfirmCommand = new RelayCommand(Confirm, o => ValidateAndConfirm());
            CancelCommand = new RelayCommand(Cancel, o => true);
            PickGuestCommand = new RelayCommand(o => PickGuest(), o => true);
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

        private void Confirm(object parameter)
        {
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

        private bool ValidateAndConfirm()
        {
            var isValid = Validator.Validate(Reservation);
            ValidationError = !isValid ? "Please fix the data." : string.Empty;

            return isValid;
        }
    }
}
