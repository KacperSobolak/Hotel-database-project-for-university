using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.Core;
using Hotel.MVVM.Model;
using Hotel.MVVM.View.PopUp;
using Hotel.MVVM.Viewmodel.PopUp;
using Hotel.Repositories;

namespace Hotel.MVVM.Viewmodel
{
    class GuestsViewModel : ViewModel
    {
        private readonly IGuestsRepository _guestsRepository;

        public ObservableCollection<Guest> Guests { get; set; }

        public RelayCommand AddGuestCommand { get; set; }
        public RelayCommand EditGuestCommand { get; set; }

        public GuestsViewModel(IGuestsRepository guestsRepository)
        {
            _guestsRepository = guestsRepository;
            Guests = new ObservableCollection<Guest>(_guestsRepository.GetGuests());

            AddGuestCommand = new RelayCommand(o => AddGuest(), o => true);
            EditGuestCommand = new RelayCommand(EditGuest, o => true);
        }

        private void AddGuest()
        {
            var dialog = new GuestPopUpView()
            {
                DataContext = new PopUpViewModel<Guest>("Dodawanie użytkownika", "Dodaj")
            };

            if (dialog.ShowDialog() == true)
            {
                var viewModel = dialog.DataContext as PopUpViewModel<Guest>;
                var newGuest = viewModel?.Element;

                if (newGuest != null)
                {
                    var id = _guestsRepository.AddGuest(newGuest);
                    newGuest.Id = id;
                    Guests.Add(newGuest);
                }
            }
        }

        private void EditGuest(object parameter)
        {
            var guestToEdit = parameter as Guest;
            if (guestToEdit == null)
                return;

            var dialog = new GuestPopUpView()
            {
                DataContext = new PopUpViewModel<Guest>("Edytowanie gościa", "Edytuj", new Guest(guestToEdit))
            };

            if (dialog.ShowDialog() == true)
            {
                var viewModel = dialog.DataContext as PopUpViewModel<Guest>;
                var updatedGuest = viewModel?.Element;

                if (updatedGuest != null)
                {
                    _guestsRepository.UpdateGuest(updatedGuest);

                    var index = Guests.IndexOf(guestToEdit);
                    Guests[index] = updatedGuest;
                }
            }
        }
    }
}
