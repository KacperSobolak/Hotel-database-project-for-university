using Hotel.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Repositories
{
    interface IGuestsRepository
    {
        IEnumerable<Guest> GetGuests();
        int AddGuest(Guest guest);
        void UpdateGuest(Guest guest);
        void DeleteGuest(int guestId);
        int GetGuestsNumber();
    }
}
