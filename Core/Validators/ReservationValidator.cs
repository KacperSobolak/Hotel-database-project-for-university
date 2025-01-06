using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.MVVM.Model;

namespace Hotel.Core.Validators
{
    internal static class ReservationValidator
    {
        public static bool Validate(Reservation reservation)
        {
            if (reservation.StartDate >= reservation.EndDate)
            {
                return false;
            }
            return true;
        }
    }
}
