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
    public class RoomsViewModel : ViewModel
    {
        private readonly IRoomRepository _roomRepository;
        private readonly ICategoriesRepository _categoryRepository;

        public ObservableCollection<Room> Rooms { get; set; }

        public RelayCommand AddRoomCommand { get; set; }
        public RelayCommand EditRoomCommand { get; set; }
        public RelayCommand DeleteRoomCommand { get; set; }

        public RoomsViewModel(IRoomRepository roomRepository, ICategoriesRepository categoriesRepository)
        {
            _roomRepository = roomRepository;
            _categoryRepository = categoriesRepository;

            OnEnter();

            AddRoomCommand = new RelayCommand(o => AddRoom(), o => true);
            EditRoomCommand = new RelayCommand(EditRoom, o => true);
            DeleteRoomCommand = new RelayCommand(DeleteRoom, o => true);
        }

        public override void OnEnter()
        {
            Rooms = new ObservableCollection<Room>(_roomRepository.GetAllRooms());
        }

        private void AddRoom()
        {
            var newRoom = new Room();
            var dialog = new RoomPopUpView()
            {
                DataContext = new RoomPopUpViewModel("Dodawanie pokoju", "Dodaj", newRoom, _categoryRepository.GetCategories())
            };

            try
            {
                if (dialog.ShowDialog() == true)
                {
                    var viewModel = dialog.DataContext as RoomPopUpViewModel;
                    var room = viewModel?.Room;

                    if (room != null)
                    {
                        var newRoomId = _roomRepository.AddRoom(room);
                        room.Id = newRoomId;
                        Rooms.Add(room);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Nie udało się dodać pokoju, nie poprawne dane" + e.Message);
            }
        }

        public void EditRoom(object parameter)
        {
            var roomToEdit = parameter as Room;
            if (roomToEdit == null)
                return;

            var dialog = new RoomPopUpView()
            {
                DataContext = new RoomPopUpViewModel("Edycja pokoju", "Zapisz", new Room(roomToEdit), _categoryRepository.GetCategories())
            };

            try
            {
                if (dialog.ShowDialog() == true)
                {
                    var viewModel = dialog.DataContext as RoomPopUpViewModel;
                    var room = viewModel?.Room;

                    if (room != null)
                    {
                        _roomRepository.UpdateRoom(room);
                        var index = Rooms.IndexOf(roomToEdit);
                        Rooms[index] = room;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Nie udało się dodać pokoju, nie poprawne dane" + e.Message);
            }
        }

        private void DeleteRoom(object parameter)
        {
            var result = MessageBox.Show("Czy chcesz usunąć pokój, zostaną również usunięte wszystkie rezerwację, które są z nim związanane?", "Usunięcie pokoju", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;

            var roomToDelete = parameter as Room;
            if (roomToDelete == null)
                return;

            _roomRepository.DeleteRoom(roomToDelete.Id);
            Rooms.Remove(roomToDelete);
        }
    }
}
