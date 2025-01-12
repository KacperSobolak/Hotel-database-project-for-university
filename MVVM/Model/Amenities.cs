using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.MVVM.Model
{
    public class Amenities
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double PricePerNight { get; set; }
        public int MaxAvailablePerDay { get; set; }

        public Amenities() { }

        public Amenities(Amenities other)
        {
            Id = other.Id;
            Name = other.Name;
            Description = other.Description;
            PricePerNight = other.PricePerNight;
            MaxAvailablePerDay = other.MaxAvailablePerDay;
        }
    }
}
