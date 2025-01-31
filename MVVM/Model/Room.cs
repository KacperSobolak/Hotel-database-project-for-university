﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.MVVM.Model
{
    public class Room
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }          
        public int CategoryId { get; set; }   
        public int MaxAdults { get; set; }          

        public string CategoryName { get; set; }

        public Room() { }

        public Room(Room other)
        {
            Id = other.Id;
            RoomNumber = other.RoomNumber;
            CategoryId = other.CategoryId;
            MaxAdults = other.MaxAdults;
            CategoryName = other.CategoryName;
        }
    }
}
