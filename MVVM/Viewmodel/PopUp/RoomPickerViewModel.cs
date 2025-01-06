using Hotel.Core;
using Hotel.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Hotel.Repositories;

namespace Hotel.MVVM.Viewmodel.PopUp
{
    internal class RoomPickerViewModel
    {
        private readonly IRoomRepository _roomsRepository;

        public ObservableCollection<Room> Rooms { get; set; }
        public Room SelectedRoom { get; set; }

        public RelayCommand PickRoomCommand { get; }

        public RoomPickerViewModel(IRoomRepository roomsRepository)
        {
            _roomsRepository = roomsRepository;
            Rooms = new ObservableCollection<Room>(_roomsRepository.GetAllRooms());

            PickRoomCommand = new RelayCommand(PickRoom, o => true);
        }

        private void PickRoom(object parameter)
        {
            var room = parameter as Room;
            SelectedRoom = room;

            var window = Application.Current.Windows.OfType<Window>()
                .FirstOrDefault(w => w.DataContext == this);

            if (window != null)
            {
                window.DialogResult = true;
                window.Close();
            }
        }
    }
}
