using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Windows;
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
        public RelayCommand DeleteCategoryCommand { get; set; }

        public CategoriesViewModel(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;

            OnEnter();

            AddCategoryCommand = new RelayCommand(o => AddCategory(), o => true);
            EditCategoryCommand = new RelayCommand(EditCategory, o => true);
            DeleteCategoryCommand = new RelayCommand(DeleteCategory, o => true);
        }

        public override void OnEnter()
        {
            Categories = new ObservableCollection<Category>(_categoriesRepository.GetCategories());
        }

        private void AddCategory()
        {
            var dialog = new CategoryPopUpView()
            {
                DataContext = new PopUpViewModel<Category>("Dodawanie kategorii","Dodaj")
            };

            if (dialog.ShowDialog() == true)
            {
                var viewModel = dialog.DataContext as PopUpViewModel<Category>;
                var newCategory = viewModel?.Element;

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
                DataContext = new PopUpViewModel<Category>("Edytowanie elementu","Edytuj", new Category(categoryToEdit))
            };

            if (dialog.ShowDialog() == true)
            {
                var viewModel = dialog.DataContext as PopUpViewModel<Category>;
                var updatedCategory = viewModel?.Element;

                if (updatedCategory != null)
                {
                    _categoriesRepository.UpdateCategory(updatedCategory);

                    var index = Categories.IndexOf(categoryToEdit);
                    Categories[index] = updatedCategory;
                }
            }
        }

        private void DeleteCategory(object parameter)
        {
            var result = MessageBox.Show("Czy chcesz usunąć kategorię, jeśli to zrobisz to zostaną usunięte również wszystkie pokoje z tej kategorii oraz rezerwacje z tych pokoi?", "Usunięcie kategorii", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;

            var categoryToDelete = parameter as Category;
            if (categoryToDelete == null)
                return;

            _categoriesRepository.DeleteCategory(categoryToDelete.Id);
            Categories.Remove(categoryToDelete);
        }

    }
}
