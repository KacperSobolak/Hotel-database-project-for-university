using System.Windows;
using Hotel.Core;
using Hotel.Core.Validators;

namespace Hotel.MVVM.Viewmodel.PopUp
{
    class PopUpViewModel<T> : ViewModel where T : new()
    {
        private T _element;
        private string _validationError;
        private string _buttonName;
        private string _windowName;

        public T Element
        {
            get => _element;
            set
            {
                _element = value;
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

        public PopUpViewModel(string windowName, string buttonName) : this(windowName, buttonName, new T()) { }

        public PopUpViewModel(string windowName, string buttonName, T element)
        {
            WindowName = windowName;
            ButtonName = buttonName;
            Element = element;

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
            var isValid = Validator.Validate(Element);
            ValidationError = !isValid ? "Proszę poprawić dane." : string.Empty;

            return isValid;
        }

    }
}
