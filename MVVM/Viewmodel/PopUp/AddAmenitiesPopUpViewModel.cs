using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Hotel.Core;
using Hotel.MVVM.Model;

namespace Hotel.MVVM.Viewmodel.PopUp
{
    public class AddAmenitiesPopUpViewModel : ViewModel
    {
        public ObservableCollection<AmenityItem> AmenitiesCollection { get; set; }

        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public AddAmenitiesPopUpViewModel(IEnumerable<AmenityItem> amenityItems)
        {
            AmenitiesCollection = new ObservableCollection<AmenityItem>(amenityItems);

            ConfirmCommand = new RelayCommand(Confirm, o => true);
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
    }
}