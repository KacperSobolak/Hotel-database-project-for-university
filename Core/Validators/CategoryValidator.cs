using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.MVVM.Model;

namespace Hotel.Core.Validators
{
    public static class CategoryValidator
    {
        public static bool Validate(Category category)
        {
            if (string.IsNullOrEmpty(category.Name))
            {
                return false;
            }

            if (string.IsNullOrEmpty(category.Description))
            {
                return false;
            }

            if (category.PricePerAdultPerNight < 0)
            {
                return false;
            }

            return true;
        }
    }
}
