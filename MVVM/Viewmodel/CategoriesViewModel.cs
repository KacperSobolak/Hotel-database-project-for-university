using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.Core;
using Hotel.MVVM.Model;
using Hotel.Repositories;

namespace Hotel.MVVM.Viewmodel
{
    public class CategoriesViewModel : ViewModel
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public ObservableCollection<Category> Categories { get; set; }

        public CategoriesViewModel(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
            Categories = new ObservableCollection<Category>(_categoriesRepository.GetCategories());
        }
    }
}
