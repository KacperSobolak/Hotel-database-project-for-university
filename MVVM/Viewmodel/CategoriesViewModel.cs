using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using Hotel.Core;
using Hotel.MVVM.Model;
using Hotel.MVVM.View.PopUp;
using Hotel.MVVM.Viewmodel.PopUp;
using Hotel.Repositories;

namespace Hotel.MVVM.Viewmodel
{
    public class CategoriesViewModel : ViewModel
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public ObservableCollection<Category> Categories { get; set; }
        public RelayCommand AddCategoryCommand { get; set; }
        public RelayCommand EditCategoryCommand { get; set; }

        public CategoriesViewModel(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
            Categories = new ObservableCollection<Category>(_categoriesRepository.GetCategories());

            AddCategoryCommand = new RelayCommand(o => AddCategory(), o => true);
            EditCategoryCommand = new RelayCommand(EditCategory, o => true);
        }

        private void AddCategory()
        {
            var dialog = new CategoryPopUpView()
            {
                DataContext = new CategoryPopUpViewModel("Dodaj")
            };

            if (dialog.ShowDialog() == true)
            {
                var viewModel = dialog.DataContext as CategoryPopUpViewModel;
                var newCategory = viewModel?.Category;

                if (newCategory != null)
                {
                    var id = _categoriesRepository.AddCategory(newCategory);
                    newCategory.Id = id;
                    Categories.Add(newCategory);
                }
            }
        }

        private void EditCategory(object parameter)
        {
            var categoryToEdit = parameter as Category;
            if (categoryToEdit == null)
                return;

            var dialog = new CategoryPopUpView()
            {
                DataContext = new CategoryPopUpViewModel("Edytuj", new Category(categoryToEdit))
            };

            if (dialog.ShowDialog() == true)
            {
                var viewModel = dialog.DataContext as CategoryPopUpViewModel;
                var updatedCategory = viewModel?.Category;

                if (updatedCategory != null)
                {
                    _categoriesRepository.UpdateCategory(updatedCategory);

                    var index = Categories.IndexOf(categoryToEdit);
                    Categories[index] = updatedCategory;
                }
            }
        }
    }
}
