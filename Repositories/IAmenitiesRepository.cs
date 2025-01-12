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
        int AddAmenities(Amenities amenities);
        void UpdateAmenities(Amenities amenities);
        void DeleteAmenities(Amenities amenities);
    }
}
