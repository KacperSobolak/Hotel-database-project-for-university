using System;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Input;
using Hotel.Core;
using Hotel.MVVM.Model;

namespace Hotel.MVVM.Viewmodel.PopUp
{
    internal class BookPopUpViewModel : ViewModel
    {
        private string _roomDetails;

        public string RoomDetails
        {
            get => _roomDetails;
            set
            {
                _roomDetails = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ConfirmCommand { get; }
        public RelayCommand CancelCommand { get; }

        public BookPopUpViewModel(Reservation reservation)
        {
            RoomDetails = $@"Pokój: {reservation.Room.RoomNumber}
                Kategoria: {reservation.Room.CategoryName}
                Gość: {reservation.Guest.Name} {reservation.Guest.Surname}
                Cena: {reservation.TotalPrice}";


            ConfirmCommand = new RelayCommand(Confirm, o => true);
            CancelCommand = new RelayCommand(Cancel, o => true);
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
    }
}