using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Hotel.Core;
using Hotel.MVVM.Model;
using Hotel.MVVM.View.PopUp;
using Hotel.MVVM.Viewmodel.PopUp;
using Hotel.Repositories;

namespace Hotel.MVVM.Viewmodel
{
    internal class AmenitiesViewModel : ViewModel
    {
        private readonly IAmenitiesRepository _amenitiesRepository;

        public ObservableCollection<Amenities> Amenities { get; set; }
        public RelayCommand AddAmenityCommand { get; set; }
        public RelayCommand EditAmenityCommand { get; set; }
        public RelayCommand DeleteAmenityCommand { get; set; }

        public AmenitiesViewModel(IAmenitiesRepository amenitiesRepository)
        {
            _amenitiesRepository = amenitiesRepository;
                
            OnEnter();

            AddAmenityCommand = new RelayCommand(o => AddAmenity(), o => true);
            EditAmenityCommand = new RelayCommand(EditAmenity, o => true);
            DeleteAmenityCommand = new RelayCommand(DeleteAmenity, o => true);
        }

        public override void OnEnter()
        {
            Amenities = new ObservableCollection<Amenities>(_amenitiesRepository.GetAllAmenities());
        }

        private void AddAmenity()
        {
            var dialog = new AmenitiesPopUpView()
            {
                DataContext = new PopUpViewModel<Amenities>("Dodawanie udogodnienia", "Dodaj")
            };

            if (dialog.ShowDialog() == true)
            {
                var viewModel = dialog.DataContext as PopUpViewModel<Amenities>;
                var newAmenity = viewModel?.Element;

                if (newAmenity != null)
                {
                    var id = _amenitiesRepository.AddAmenities(newAmenity);
                    newAmenity.Id = id;
                    Amenities.Add(newAmenity);
                }
            }
        }

        private void EditAmenity(object parameter)
        {
            var amenitiesToEdit = parameter as Amenities;
            if (amenitiesToEdit == null)
                return;

            var dialog = new AmenitiesPopUpView()
            {
                DataContext = new PopUpViewModel<Amenities>("Edytowanie elementu", "Edytuj", new Amenities(amenitiesToEdit))
            };

            if (dialog.ShowDialog() == true)
            {
                var viewModel = dialog.DataContext as PopUpViewModel<Amenities>;
                var updatedAmenities = viewModel?.Element;

                if (updatedAmenities != null)
                {
                    _amenitiesRepository.UpdateAmenities(updatedAmenities);

                    var index = Amenities.IndexOf(amenitiesToEdit);
                    Amenities[index] = updatedAmenities;
                }
            }
        }

        private void DeleteAmenity(object parameter)
        {
            var result = MessageBox.Show("Czy chcesz usunąć udogodnienie? Jeśli to zrobisz to zostaną usunięte ze wszystkich rezerwacji", "Usunięcie udogodnienia", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;

            var amenitiesToDelete = parameter as Amenities;
            if (amenitiesToDelete == null)
                return;


            _amenitiesRepository.DeleteAmenities(amenitiesToDelete.Id);
            Amenities.Remove(amenitiesToDelete);
        }
    }
}
