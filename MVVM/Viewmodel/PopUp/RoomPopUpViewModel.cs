using Hotel.Core;
using Hotel.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Hotel.Core.Validators;

namespace Hotel.MVVM.Viewmodel.PopUp
{
    class RoomPopUpViewModel : ViewModel
    {
        private List<Category> Categories { get; set; }
        public ObservableCollection<string> CategoriesNames { get; set; }
        private string _selectedCategory;
        private Room _room;
        private string _validationError;
        private string _buttonName;
        private string _windowName;

        public Room Room
        {
            get => _room;
            set
            {
                _room = value;
                OnPropertyChanged();
            }
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }

        public string ValidationError
        {
            get => _validationError;
            set
            {
                _validationError = value;
                OnPropertyChanged();
            }
        }

        public string ButtonName
        {
            get => _buttonName;
            set
            {
                _buttonName = value;
                OnPropertyChanged();
            }
        }

        public string WindowName
        {
            get => _windowName;
            set
            {
                _windowName = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public RoomPopUpViewModel(string windowName, string buttonName, Room room, IEnumerable<Category> categories)
        {
            WindowName = windowName;
            ButtonName = buttonName;
            Room = room;
            Categories = categories.ToList();
            SetUpCategories();

            ConfirmCommand = new RelayCommand(Confirm, o => ValidateAndConfirm());
            CancelCommand = new RelayCommand(Cancel, o => true);
        }

        private void SetUpCategories()
        {
            CategoriesNames = new ObservableCollection<string>(Categories.Select(c => c.Name));
            SelectedCategory = Categories.FirstOrDefault(c => c.Id == Room.CategoryId)?.Name ?? Categories.First().Name;
        }

        private void Confirm(object parameter)
        {
            Room.CategoryId = Categories.First(c => c.Name == SelectedCategory).Id;
            Room.CategoryName = SelectedCategory;
            if (parameter is Window window)
            {
                window.DialogResult = true;
                window.Close();
            }
        }

        private void Cancel(object parameter)
        {
            if (parameter is Window window)
            {
                window.DialogResult = false;
                window.Close();
            }
        }

        private bool ValidateAndConfirm()
        {
            var isValid = Validator.Validate(Room);
            ValidationError = !isValid ? "Proszę poprawić dane." : string.Empty;

            return isValid;
        }
    }
}
