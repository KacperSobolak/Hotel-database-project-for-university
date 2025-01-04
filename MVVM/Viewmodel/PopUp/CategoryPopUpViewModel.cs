﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Hotel.Core;
using Hotel.Core.Validators;
using Hotel.MVVM.Model;

namespace Hotel.MVVM.Viewmodel.PopUp
{
    class CategoryPopUpViewModel : ViewModel
    {
        private Category _category;
        private string _validationError;

        public Category Category
        {
            get => _category;
            set
            {
                _category = value;
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

        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public CategoryPopUpViewModel() : this(new Category()) { }

        public CategoryPopUpViewModel(Category category)
        {
            Category = category;

            ConfirmCommand = new RelayCommand(Confirm, o => ValidateAndConfirm());
            CancelCommand = new RelayCommand(Cancel, o => true);
        }

        private void Confirm(object parameter)
        {
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
            var isValid = CategoryValidator.Validate(Category);
            ValidationError = !isValid ? "Proszę poprawić dane kategorii. Nazwa nie może być pusta, a cena musi być większa od 0." : string.Empty;

            return isValid;
        }

    }
}
