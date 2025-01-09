using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.MVVM.Model
{
    public class Reservation
    {
        public int Id { get; set; } 

        public Room? Room { get; set; } 

        public Guest? Guest { get; set; }  
        public int TotalPrice { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public Reservation() { }

        public Reservation(Reservation other)
        {
            Id = other.Id;
            Room = other.Room;
            Guest = other.Guest;
            TotalPrice = other.TotalPrice;
            StartDate = other.StartDate;
            EndDate = other.EndDate;
        }
    }
}
