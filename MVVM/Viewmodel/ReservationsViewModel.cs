﻿using System.Collections.ObjectModel;
using System.Windows;
using Hotel.Core;
using Hotel.MVVM.Model;
using Hotel.MVVM.View.PopUp;
using Hotel.MVVM.Viewmodel.PopUp;
using Hotel.Repositories;

namespace Hotel.MVVM.Viewmodel
{
    internal class ReservationsViewModel : ViewModel
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IGuestsRepository _guestsRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IAmenitiesRepository _amenitiesRepository;
        private bool _hideOldReservations;
        public ObservableCollection<Reservation> Reservations { get; set; }
        public bool HideOldReservations
        {
            get => _hideOldReservations;
            set
            {
                _hideOldReservations = value;
                OnPropertyChanged();
                LoadReservations();
            }
        }

        public RelayCommand AddReservationCommand { get; set; }
        public RelayCommand EditReservationCommand { get; set; }
        public RelayCommand ShowAmenitiesCommand { get; set; }
        public RelayCommand DeleteReservationCommand { get; set; }

        public ReservationsViewModel(IReservationRepository reservationRepository, IGuestsRepository guestsRepository, IRoomRepository roomRepository, IAmenitiesRepository amenitiesRepository)
        {
            _reservationRepository = reservationRepository;
            _guestsRepository = guestsRepository;
            _roomRepository = roomRepository;
            _amenitiesRepository = amenitiesRepository;

            OnEnter();

            AddReservationCommand = new RelayCommand(o => AddReservation(), o => true);
            EditReservationCommand = new RelayCommand(EditReservation, o => true);
            ShowAmenitiesCommand = new RelayCommand(ShowAmenities, o => true);
            DeleteReservationCommand = new RelayCommand(DeleteReservation, o => true);
        }

        public override void OnEnter()
        {
            Reservations = new ObservableCollection<Reservation>();
            LoadReservations();
        }

        private void LoadReservations()
        {
            var reservationList = HideOldReservations ? _reservationRepository.GetAllActualReservations() : _reservationRepository.GetAllReservations();
            Reservations.Clear();
            foreach (var reservation in reservationList)
            {
                Reservations.Add(reservation);
            }
        }

        private void ShowAmenities(object parameter)
        {
            var reservation = parameter as Reservation;
            if (reservation == null)
                return;

            var amenities = _reservationRepository.GetAmenitiesForReservation(reservation.Id);

            if (amenities.Any())
            {
                var dialog = new ShowAmenitiesView()
                {
                    DataContext = new ShowAmenitiesViewModel(amenities)
                };

                dialog.ShowDialog();
            }
            else
            {
                MessageBox.Show("Nie ma żadnych udogodnień");
            }
        }

        private void AddReservation()
        {
            var newReservation = new Reservation();
            var dialog = new ReservationPopUpView()
            {
                DataContext = new ReservationPopUpViewModel("Dodawanie rezerwacji", "Dodaj", newReservation, _guestsRepository, _roomRepository, _reservationRepository, _amenitiesRepository)
            };
            try
            {
                if (dialog.ShowDialog() == true)
                {
                    var viewModel = dialog.DataContext as ReservationPopUpViewModel;
                    var reservation = viewModel?.Reservation;

                    if (reservation != null)
                    {
                        var newReservationId = _reservationRepository.AddReservation(reservation);
                        reservation.Id = newReservationId;
                        Reservations.Add(reservation);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Nie udało się dodać rezerwacji, nie poprawne dane" + e.Message);
            }
        }

        private void EditReservation(object parameter)
        {
            var reservationToEdit = parameter as Reservation;
            if (reservationToEdit == null)
                return;

            var dialog = new ReservationPopUpView()
            {
                DataContext = new ReservationPopUpViewModel("Edytowanie rezerwacji", "Edytuj", new Reservation(reservationToEdit), _guestsRepository, _roomRepository, _reservationRepository, _amenitiesRepository)
            };

            try
            {
                if (dialog.ShowDialog() == true)
                {
                    var viewModel = dialog.DataContext as ReservationPopUpViewModel;
                    var updatedReservation = viewModel?.Reservation;

                    if (updatedReservation != null)
                    {
                        _reservationRepository.UpdateReservation(updatedReservation);
                        var index = Reservations.IndexOf(reservationToEdit);
                        Reservations[index] = updatedReservation;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Nie udało się edytować rezerwacji, nie poprawne dane" + e.Message);
            }
        }

        private void DeleteReservation(object parameter)
        {
            var reservationToDelete = parameter as Reservation;
            if (reservationToDelete == null)
                return;

            var result = MessageBox.Show("Czy na pewno chcesz usunąć rezerwację?", "Usuwanie rezerwacji", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _reservationRepository.DeleteReservation(reservationToDelete.Id);
                Reservations.Remove(reservationToDelete);
            }
        }
    }
}
