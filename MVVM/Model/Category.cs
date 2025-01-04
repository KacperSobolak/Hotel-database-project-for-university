using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.MVVM.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double PricePerAdultPerNight { get; set; }

        public Category() { }

        public Category(Category other)
        {
            Id = other.Id;
            Name = other.Name;
            Description = other.Description;
            PricePerAdultPerNight = other.PricePerAdultPerNight;
        }
    }
}
