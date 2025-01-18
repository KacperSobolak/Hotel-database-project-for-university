using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.MVVM.Model;

namespace Hotel.Repositories
{
    interface IReservationRepository
    {
        IEnumerable<Reservation> GetAllReservations();
        IEnumerable<Reservation> GetAllActualReservations();
        int AddReservation(Reservation reservation);
        int GetReservationsNumber();
        IEnumerable<KeyValuePair<string, int>> GetMostPopularRoom();                                                                                        
        IEnumerable<KeyValuePair<string, int>> GetMostPopularNumberOfAdults();
        IEnumerable<KeyValuePair<string, int>> GetMostPopularCategory();
        IEnumerable<KeyValuePair<string, int>> GetMostPopularAmenity();
        int GetPastReservationsNumber();
        double GetReservationsRevenue();
        Reservation GetReservation(int id);
        int FindAvailableRoom(Reservation reservation, Category category, int numberOfAdults);
        IEnumerable<AmenityItem> GetAmenitiesForReservation(int reservationId);
        void UpdateReservation(Reservation reservation);
        void DeleteAllAmenitiesForReservation(int reservationId);
        void DeleteReservation(int id);
    }
}
