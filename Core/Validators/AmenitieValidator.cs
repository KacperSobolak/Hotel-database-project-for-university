using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.MVVM.Model;

namespace Hotel.Core.Validators
{
    internal static class AmenitieValidator
    {
        public static bool Validate(Amenities amenitie)
        {
            if (amenitie == null)
            {
                return false;
            }

            return true;
        }
    }
}
