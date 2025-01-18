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
    internal class BookViewModel : ViewModel
    {
        private IReservationRepository _reservationRepository;
        private ICategoriesRepository _categoriesRepository;
        private IGuestsRepository _guestsRepository;
        private IAmenitiesRepository _amenitiesRepository;

        public ObservableCollection<string> CategoriesNames { get; set; }
        private List<Category> Categories { get; set; }
        private string _selectedCategory;
        private Guest? _selectedGuest;
        private string _guestName;
        private DateOnly _startDate;
        private DateOnly _endDate;
        private int _numberOfAdults;

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }

        public Guest? SelectedGuest
        {
            get => _selectedGuest;
            set
            {
                _selectedGuest = value;
                OnPropertyChanged();
            }
        }

        public string GuestName
        {
            get => _guestName;
            set
            {
                _guestName = value;
                OnPropertyChanged();
            }
        }

        public DateOnly StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        public DateOnly EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }

        public int NumberOfAdults
        {
            get => _numberOfAdults;
            set
            {
                _numberOfAdults = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand FindRoomCommand { get; set; }
        public RelayCommand PickGuestCommand { get; set; }

        public BookViewModel(IReservationRepository reservationRepository, ICategoriesRepository categoriesRepository, IGuestsRepository guestsRepository, IAmenitiesRepository amenitiesRepository)
        {
            _reservationRepository = reservationRepository;
            _categoriesRepository = categoriesRepository;
            _guestsRepository = guestsRepository;
            _amenitiesRepository = amenitiesRepository;

            OnEnter();

            FindRoomCommand = new RelayCommand(o => FindRoom(), o => CanCreateReservation());
            PickGuestCommand = new RelayCommand(o => PickGuest(), o => true);
        }

        public override void OnEnter()
        {
            NumberOfAdults = 1;
            SetUpCategories();
            SetUpDates();
        }

        private void SetUpCategories()
        {
            Categories = _categoriesRepository.GetCategories().ToList();
            CategoriesNames = new ObservableCollection<string>(Categories.Select(c => c.Name));
            SelectedCategory = Categories.First().Name;
        }

        private void SetUpDates()
        {
            StartDate = DateOnly.FromDateTime(DateTime.Now);
            EndDate = DateOnly.FromDateTime(DateTime.Now + TimeSpan.FromDays(1));
        }

        private void PickGuest()
        {
            var dialog = new GuestPickerView()
            {
                DataContext = new GuestPickerViewModel(_guestsRepository)
            };

            if (dialog.ShowDialog() == true)
            {
                var viewModel = dialog.DataContext as GuestPickerViewModel;
                var guest = viewModel?.SelectedGuest;

                if (guest != null)
                {
                    SelectedGuest = guest;
                    GuestName = $"{guest.Name} {guest.Surname}";
                }
            }
        }

        private void FindRoom()
        {
            var reservation = new Reservation()
            {
                Guest = SelectedGuest,
                StartDate = StartDate,
                EndDate = EndDate
            };

            var category = Categories.First(c => c.Name == SelectedCategory);

            var id = _reservationRepository.FindAvailableRoom(reservation, category, NumberOfAdults);

            if (id == -1)
            {
                MessageBox.Show("Nie ma dostępnych pokoi, które spełniają podane wymagania");
            }
            else
            {
                var dialog = new BookPopUpView()
                {
                    DataContext = new BookPopUpViewModel(reservation, _amenitiesRepository, _reservationRepository)
                };

                if (dialog.ShowDialog() == true)
                {
                    CleanFields();
                    MessageBox.Show("Dodano rezerwacje");
                }
                else
                {
                    MessageBox.Show("Anulowano rezerwacje");
                }
            }
        }

        private void CleanFields()
        {
            SelectedCategory = Categories.First().Name;
            SelectedGuest = null;
            GuestName = string.Empty;
            SetUpDates();
            NumberOfAdults = 0;
        }

        private bool CanCreateReservation()
        {
            if (StartDate >= EndDate)
            {
                return false;
            }
            if (SelectedGuest == null)
            {
                return false;
            }
            return true;
        }
    }
}
