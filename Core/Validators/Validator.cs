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
            else
            {
                throw new ArgumentException("Invalid entity type");
            }
        }
    }
}
