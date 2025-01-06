using Hotel.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Hotel.Core;
using Hotel.Repositories;

namespace Hotel.MVVM.Viewmodel.PopUp
{
    internal class GuestPickerViewModel : ViewModel
    {
        private readonly IGuestsRepository _guestRepository;

        public ObservableCollection<Guest> Guests { get; set; }
        public Guest SelectedGuest { get; set; }

        public RelayCommand PickGuestCommand { get; }

        public GuestPickerViewModel(IGuestsRepository guestRepository)
        {
            _guestRepository = guestRepository;
            Guests = new ObservableCollection<Guest>(_guestRepository.GetGuests());

            PickGuestCommand = new RelayCommand(PickGuest, o => true);
        }

        private void PickGuest(object parameter)
        {
            var guest = parameter as Guest;
            SelectedGuest = guest;
            var window = Application.Current.Windows.OfType<Window>()
                .FirstOrDefault(w => w.DataContext == this);

            if (window != null)
            {
                window.DialogResult = true;
                window.Close();
            }
        }
    }
}
