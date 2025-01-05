using Hotel.Core;
using Hotel.Core.Validators;
using Hotel.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hotel.MVVM.Viewmodel.PopUp
{
    class GuestPopUpViewModel : ViewModel
    {
        private Guest _guest;
        private string _validationError;
        private string _buttonName;
        private string _windowName;

        public Guest Guest
        {
            get => _guest;
            set
            {
                _guest = value;
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

        public GuestPopUpViewModel(string windowName, string buttonName) : this(windowName, buttonName, new Guest()) { }

        public GuestPopUpViewModel(string windowName, string buttonName, Guest guest)
        {
            WindowName = windowName;
            ButtonName = buttonName;
            Guest = guest;
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
            var isValid = GuestValidator.Validate(Guest);
            ValidationError = !isValid ? "Proszę poprawić dane kategorii. Nazwa nie może być pusta, a cena musi być większa od 0." : string.Empty;

            return isValid;
        }
    }
}
