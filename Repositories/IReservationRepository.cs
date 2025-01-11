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
        int AddReservation(Reservation reservation);
        int FindAvailableRoom(Reservation reservation, Category category, int numberOfAdults);
        void UpdateReservation(Reservation reservation);
        void DeleteReservation(int id);
    }
}
