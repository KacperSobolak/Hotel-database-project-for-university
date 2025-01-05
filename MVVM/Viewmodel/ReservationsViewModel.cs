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
    internal class ReservationsViewModel : ViewModel
    {
        private readonly IReservationRepository _reservationRepository;
        public ObservableCollection<Reservation> Reservations { get; set; }

        public RelayCommand AddReservationCommand { get; set; }
        public RelayCommand EditReservationCommand { get; set; }

        public ReservationsViewModel(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
            LoadReservations();
        }

        private void LoadReservations()
        {
            var reservationList = _reservationRepository.GetAllReservations();
            Reservations = new ObservableCollection<Reservation>(reservationList);

            AddReservationCommand = new RelayCommand(o => AddReservation(), o => true);
            EditReservationCommand = new RelayCommand(EditReservation, o => true);
        }

        private void AddReservation()
        {
            throw new NotImplementedException();
            //var newReservation = new Reservation();
            //var dialog = new ReservationPopUpView()
            //{
            //    DataContext = new ReservationPopUpViewModel("Dodawanie rezerwacji", "Dodaj", newReservation)
            //};

            //if (dialog.ShowDialog() == true)
            //{
            //    var viewModel = dialog.DataContext as ReservationPopUpViewModel;
            //    var reservation = viewModel?.Reservation;

            //    if (reservation != null)
            //    {
            //        var newReservationId = _reservationRepository.AddReservation(reservation);
            //        reservation.Id = newReservationId;
            //        Reservations.Add(reservation);
            //    }
            //}
        }

        private void EditReservation(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
