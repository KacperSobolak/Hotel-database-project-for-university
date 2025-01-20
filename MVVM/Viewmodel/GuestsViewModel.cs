using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        public RelayCommand DeleteGuestCommand { get; set; }

        public GuestsViewModel(IGuestsRepository guestsRepository)
        {
            _guestsRepository = guestsRepository;

            OnEnter();

            AddGuestCommand = new RelayCommand(o => AddGuest(), o => true);
            EditGuestCommand = new RelayCommand(EditGuest, o => true);
            DeleteGuestCommand = new RelayCommand(DeleteGuest, o => true);
        }

        public override void OnEnter()
        {
            Guests = new ObservableCollection<Guest>(_guestsRepository.GetGuests());
        }

        private void AddGuest()
        {
            var dialog = new GuestPopUpView()
            {
                DataContext = new PopUpViewModel<Guest>("Dodawanie użytkownika", "Dodaj")
            };

            try
            {
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
            catch (Exception e)
            {
                MessageBox.Show("Nie udało się dodać gościa, nie poprawne dane" + e.Message);
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

            try
            {
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
            catch (Exception e)
            {
                MessageBox.Show("Nie udało się edytować gościa, nie poprawne dane" + e.Message);
            }
        }

        private void DeleteGuest(object parameter)
        {
            var result = MessageBox.Show("Czy chcesz usunąć gościa, jeśli to zrobisz to zostaną usunięte wszystkie rezerwacje, które są z nim związane?", "Usunięcie gościa", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;

            var guestToDelete = parameter as Guest;
            if (guestToDelete == null)
                return;

            _guestsRepository.DeleteGuest(guestToDelete.Id);
            Guests.Remove(guestToDelete);
        }
    }
}
