using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.MVVM.Model;

namespace Hotel.Repositories
{
    internal interface IAmenitiesRepository
    {
        IEnumerable<Amenities> GetAllAmenities();
        IEnumerable<Amenities> GetAmenitiesAvailableForDateRange(DateOnly startDate, DateOnly endDate);
        int AddAmenities(Amenities amenities);
        void AddAmenityToReservation(Amenities amenity, int reservationId, int quantity, double amenityPrice);
        void UpdateAmenities(Amenities amenities);
        void DeleteAmenities(int amenitiesId);
    }
}
