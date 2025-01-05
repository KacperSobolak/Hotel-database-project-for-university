using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.MVVM.Model;

namespace Hotel.Core.Validators
{
    public static class RoomValidator
    {
        public static bool Validate(Room room)
        {
            if (room.RoomNumber < 0)
            {
                return false;
            }
            if (room.MaxAdults < 0)
            {
                return false;
            }
            return true;
        }
    }
}
