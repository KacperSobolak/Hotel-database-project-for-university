using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.MVVM.Model
{
    public class Guest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public DateOnly DateOfBirth { get; set; }

        public Guest() { }

        public Guest(Guest other)
        {
            Id = other.Id;
            Name = other.Name;
            Surname = other.Surname;
            Phone = other.Phone;
            Email = other.Email;
            DateOfBirth = other.DateOfBirth;
        }
    }
}
