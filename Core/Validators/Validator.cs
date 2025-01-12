using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.MVVM.Model;

namespace Hotel.Core.Validators
{
    public static class Validator
    {
        public static bool Validate<T>(T entity)
        {
            if (entity is Category category)
            {
                return CategoryValidator.Validate(category);
            }
            else if (entity is Guest guest)
            {
                return GuestValidator.Validate(guest);
            }
            else if (entity is Reservation reservation)
            {
                return ReservationValidator.Validate(reservation);
            }
            else if (entity is Room room)
            {
                return RoomValidator.Validate(room);
            }
            else if (entity is Amenities amenity)
            {
                return AmenitieValidator.Validate(amenity);
            }
            else
            {
                throw new ArgumentException("Invalid entity type");
            }
        }
    }
}
