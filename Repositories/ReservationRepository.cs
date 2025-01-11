using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.MVVM.Model;
using Npgsql;

namespace Hotel.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly string _connectionString;

        public ReservationRepository()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["HotelDatabase"].ConnectionString;
        }

        public IEnumerable<Reservation> GetAllReservations()
        {
            var reservations = new List<Reservation>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new NpgsqlCommand("SELECT r.id, r.room_id, r.guest_id, r.start_date, r.end_date, r.total_price, " +
                                            "ro.room_number, ro.max_adults, rc.name AS category_name, rc.price_per_night_per_adult, " +
                                            "g.name AS guest_name, g.surname AS guest_surname " +
                                            "FROM project.reservations r " +
                                            "JOIN project.rooms ro ON r.room_id = ro.id " +
                                            "JOIN project.room_categories rc ON ro.category_id = rc.id " +
                                            "JOIN project.guests g ON r.guest_id = g.id", connection);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservations.Add(new Reservation
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            StartDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("start_date"))), 
                            EndDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("end_date"))),  
                            TotalPrice = reader.GetDouble(reader.GetOrdinal("total_price")),
                            Room = new Room
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("room_id")),
                                RoomNumber = reader.GetInt32(reader.GetOrdinal("room_number")),
                                MaxAdults = reader.GetInt32(reader.GetOrdinal("max_adults")),
                                CategoryName = reader.GetString(reader.GetOrdinal("category_name"))
                            },
                            Guest = new Guest
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("guest_id")),
                                Name = reader.GetString(reader.GetOrdinal("guest_name")),
                                Surname = reader.GetString(reader.GetOrdinal("guest_surname"))
                            }
                        });
                    }
                }
            }

            return reservations;
        }

        public int AddReservation(Reservation reservation)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                var cmd = new NpgsqlCommand("INSERT INTO project.reservations (room_id, guest_id, start_date, end_date, total_price) " +
                                            "VALUES (@room_id, @guest_id, @start_date, @end_date, @total_price) RETURNING id", connection);

                cmd.Parameters.AddWithValue("room_id", reservation.Room.Id);
                cmd.Parameters.AddWithValue("guest_id", reservation.Guest.Id);
                cmd.Parameters.AddWithValue("start_date", reservation.StartDate.ToDateTime(TimeOnly.MinValue));
                cmd.Parameters.AddWithValue("end_date", reservation.EndDate.ToDateTime(TimeOnly.MinValue));
                cmd.Parameters.AddWithValue("total_price", reservation.TotalPrice);

                var newId = (int)cmd.ExecuteScalar();  
                return newId;
            }
        }

        public int FindAvailableRoom(Reservation reservation, Category category, int numberOfAdults)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            string query = @"
                SELECT room_id, room_number, category_name, max_adults, price_per_night, total_price
                FROM project.find_available_room(@start_date, @end_date, @category_id_param, @number_of_adults);";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@start_date", reservation.StartDate);
            command.Parameters.AddWithValue("@end_date", reservation.EndDate);
            command.Parameters.AddWithValue("@category_id_param", category.Id);
            command.Parameters.AddWithValue("@number_of_adults", numberOfAdults);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                reservation.Room = new Room
                {
                    Id = reader.GetInt32(reader.GetOrdinal("room_id")),
                    RoomNumber = reader.GetInt32(reader.GetOrdinal("room_number")),
                    CategoryId = category.Id,
                    CategoryName = reader.GetString(reader.GetOrdinal("category_name")),
                    MaxAdults = reader.GetInt32(reader.GetOrdinal("max_adults"))
                };

                reservation.TotalPrice = reader.GetDouble(reader.GetOrdinal("total_price"));
                return reservation.Room.Id;
            }

            return -1;
        }

        public void UpdateReservation(Reservation reservation)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                var cmd = new NpgsqlCommand("UPDATE project.reservations SET room_id = @room_id, guest_id = @guest_id, " +
                                            "start_date = @start_date, end_date = @end_date, total_price = @total_price " +
                                            "WHERE id = @id", connection);

                cmd.Parameters.AddWithValue("id", reservation.Id);
                cmd.Parameters.AddWithValue("room_id", reservation.Room.Id);
                cmd.Parameters.AddWithValue("guest_id", reservation.Guest.Id);
                cmd.Parameters.AddWithValue("start_date", reservation.StartDate.ToDateTime(TimeOnly.MinValue));
                cmd.Parameters.AddWithValue("end_date", reservation.EndDate.ToDateTime(TimeOnly.MinValue));
                cmd.Parameters.AddWithValue("total_price", reservation.TotalPrice);

                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteReservation(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                var cmd = new NpgsqlCommand("DELETE FROM project.reservations WHERE id = @id", connection);
                cmd.Parameters.AddWithValue("id", id);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
