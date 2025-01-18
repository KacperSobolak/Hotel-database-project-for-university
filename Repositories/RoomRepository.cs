using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.MVVM.Model;
using Npgsql;

namespace Hotel.Repositories
{
    class RoomRepository : IRoomRepository
    {
        private readonly string _connectionString;

        public RoomRepository()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["HotelDatabase"].ConnectionString;
        }

        public IEnumerable<Room> GetAllRooms()
        {
            var rooms = new List<Room>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM project.room_details order by room_number";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rooms.Add(new Room
                            {
                                Id = reader.GetInt32(0), // room_id
                                RoomNumber = reader.GetInt32(1),
                                CategoryId = reader.GetInt32(2),
                                CategoryName = reader.GetString(3),
                                MaxAdults = reader.GetInt32(4),
                            });
                        }
                    }
                }
            }

            return rooms;
        }

        public int GetRoomsNumber()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM project.rooms";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public int AddRoom(Room room)
        {
            var id = -1;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                var query = "INSERT INTO project.rooms (room_number, category_id, max_adults) " +
                            "VALUES (@RoomNumber, @CategoryId, @MaxAdults) RETURNING id;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);
                    command.Parameters.AddWithValue("@CategoryId", room.CategoryId);
                    command.Parameters.AddWithValue("@MaxAdults", room.MaxAdults);

                    // Get the generated id of the room
                    id = (int)command.ExecuteScalar();
                }
            }

            return id;
        }

        public void UpdateRoom(Room room)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                var query = "UPDATE project.rooms " +
                            "SET room_number = @RoomNumber, category_id = @CategoryId, max_adults = @MaxAdults " +
                            "WHERE id = @Id;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", room.Id);
                    command.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);
                    command.Parameters.AddWithValue("@CategoryId", room.CategoryId);
                    command.Parameters.AddWithValue("@MaxAdults", room.MaxAdults);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteRoom(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                var query = "DELETE FROM project.rooms WHERE id = @Id;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
