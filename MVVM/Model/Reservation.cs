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

        public int RoomId { get; set; }
        public Room? Room { get; set; } 

        public int GuestId { get; set; }
        public Guest? Guest { get; set; }  
        public int TotalPrice { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public Reservation() { }

        public Reservation(Reservation other)
        {
            Id = other.Id;
            RoomId = other.RoomId;
            Room = other.Room;
            GuestId = other.GuestId;
            Guest = other.Guest;
            TotalPrice = other.TotalPrice;
            StartDate = other.StartDate;
            EndDate = other.EndDate;
        }
    }
}
