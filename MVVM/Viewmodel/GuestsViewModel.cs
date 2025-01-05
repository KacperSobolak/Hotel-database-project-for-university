using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.Core;
using Hotel.MVVM.Model;
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
            throw new NotImplementedException();
        }

        private void EditGuest(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
