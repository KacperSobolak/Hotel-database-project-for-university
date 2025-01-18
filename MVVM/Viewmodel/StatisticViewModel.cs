using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.Core;
using Hotel.Repositories;

namespace Hotel.MVVM.Viewmodel
{
    internal class StatisticViewModel : ViewModel
    {
        private IGuestsRepository _guestsRepository;
        private IRoomRepository _roomRepository;
        private IReservationRepository _reservationRepository;

        private int _totalRooms;
        private int _totalGuests;
        private int _totalBookings;
        private double _totalRevenue;
        private int _totalPastBooking;

        public int TotalRooms
        {
            get { return _totalRooms; }
            set
            {
                _totalRooms = value;
                OnPropertyChanged();
            }
        }

        public int TotalGuests
        {
            get { return _totalGuests; }
            set
            {
                _totalGuests = value;
                OnPropertyChanged();
            }
        }

        public int TotalBookings
        {
            get { return _totalBookings; }
            set
            {
                _totalBookings = value;
                OnPropertyChanged();
            }
        }

        public double TotalRevenue
        {
            get { return _totalRevenue; }
            set
            {
                _totalRevenue = value;
                OnPropertyChanged();
            }
        }

        public int TotalPastBooking
        {
            get { return _totalPastBooking; }
            set
            {
                _totalPastBooking = value;
                OnPropertyChanged();
            }
        }

        public StatisticViewModel(IGuestsRepository guestsRepository, IRoomRepository roomRepository, IReservationRepository reservationRepository)
        {
            _guestsRepository = guestsRepository;
            _roomRepository = roomRepository;
            _reservationRepository = reservationRepository;
        }

        public override void OnEnter()
        {
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            TotalGuests = _guestsRepository.GetGuestsNumber();
            TotalRooms = _roomRepository.GetRoomsNumber();
            TotalBookings = _reservationRepository.GetReservationsNumber();
            TotalRevenue = _reservationRepository.GetReservationsRevenue();
            TotalPastBooking = _reservationRepository.GetPastReservationsNumber();
        }

        public StatisticViewModel()
        {

        }
    }
}
