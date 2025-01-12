using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.MVVM.Model;

public class AmenityItem
{
    public Amenities Amenity { get; set; }
    public int Quantity { get; set; }

    public AmenityItem(Amenities amenity, int quantity)
    {
        Amenity = amenity;
        Quantity = quantity;
    }
}
