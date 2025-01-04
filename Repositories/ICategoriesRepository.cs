using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.MVVM.Model;

namespace Hotel.Repositories
{
    public interface ICategoriesRepository
    {
        IEnumerable<Category> GetCategories();
        int AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
    }
}
