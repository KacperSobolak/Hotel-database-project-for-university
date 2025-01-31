﻿using System;
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
                var cmd = new NpgsqlCommand("SELECT * FROM project.reservation_details", connection);

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

        public IEnumerable<Reservation> GetAllActualReservations()
        {
            var reservations = new List<Reservation>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new NpgsqlCommand("SELECT * FROM project.reservation_details r WHERE r.end_date >= NOW()", connection);

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

        public int GetReservationsNumber()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM project.reservations";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public IEnumerable<KeyValuePair<string, int>> GetMostPopularRoom()
        {
            var roomsRank = new List<KeyValuePair<string, int>>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new NpgsqlCommand("select * from project.rooms_statistic", connection);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var roomNumber = reader.GetInt32(reader.GetOrdinal("room_number")).ToString();
                        var count = reader.GetInt32(reader.GetOrdinal("reservation_count"));
                        var room = new KeyValuePair<string, int>(roomNumber, count);
                        roomsRank.Add(room);
                    }
                }
            }
            return roomsRank;
        }

        public IEnumerable<KeyValuePair<string, int>> GetMostPopularNumberOfAdults()
        {
            var adultsRank = new List<KeyValuePair<string, int>>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new NpgsqlCommand("select * from project.number_of_adults_statistic", connection);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var numberOfAdults = reader.GetInt32(reader.GetOrdinal("max_adults")).ToString();
                        var count = reader.GetInt32(reader.GetOrdinal("reservation_count"));
                        var number = new KeyValuePair<string, int>(numberOfAdults, count);
                        adultsRank.Add(number);
                    }
                }
            }
            return adultsRank;
        }

        public IEnumerable<KeyValuePair<string, int>> GetMostPopularCategory()
        {
            var categoriesRank = new List<KeyValuePair<string, int>>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new NpgsqlCommand("select * from project.categories_statistic", connection);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var categoryName = reader.GetString(reader.GetOrdinal("category_name"));
                        var count = reader.GetInt32(reader.GetOrdinal("reservation_count"));
                        var category = new KeyValuePair<string, int>(categoryName, count);
                        categoriesRank.Add(category);
                    }
                }
            }
            return categoriesRank;
        }

        public IEnumerable<KeyValuePair<string, int>> GetMostPopularAmenity()
        {
            var amenitiesRank = new List<KeyValuePair<string, int>>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new NpgsqlCommand("select * from project.amenities_statistic", connection);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var amenityName = reader.GetString(reader.GetOrdinal("amenity_name"));
                        var count = reader.GetInt32(reader.GetOrdinal("usage_count"));
                        var amenity = new KeyValuePair<string, int>(amenityName, count);
                        amenitiesRank.Add(amenity);
                    }
                }
            }
            return amenitiesRank;
        }

        public int GetPastReservationsNumber()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM project.reservations r WHERE r.end_date <= NOW();";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public double GetReservationsRevenue()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT COALESCE(SUM(total_price), 0) FROM project.reservations WHERE start_date <= NOW();";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public Reservation GetReservation(int id)
        {
            Reservation reservation = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new NpgsqlCommand("SELECT r.id, r.room_id, r.guest_id, r.start_date, r.end_date, r.total_price, " +
                                            "ro.room_number, ro.max_adults, rc.name AS category_name, rc.price_per_night_per_adult, " +
                                            "g.name AS guest_name, g.surname AS guest_surname " +
                                            "FROM project.reservations r " +
                                            "JOIN project.rooms ro ON r.room_id = ro.id " +
                                            "JOIN project.room_categories rc ON ro.category_id = rc.id " +
                                            "JOIN project.guests g ON r.guest_id = g.id " +
                                            "WHERE r.id = @id", connection);

                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        reservation = new Reservation
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
                        };
                    }
                }
            }

            return reservation;
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

        public IEnumerable<AmenityItem> GetAmenitiesForReservation(int reservationId)
        {
            const string query = @"
        SELECT 
            a.id AS AmenityId,
            a.name AS Name,
            a.description AS Description,
            a.price_per_night AS PricePerNight,
            ra.quantity AS Quantity
        FROM 
            project.reservation_amenities ra
        INNER JOIN 
            project.amenities a ON ra.amenity_id = a.id
        WHERE 
            ra.reservation_id = @ReservationId";

            var amenities = new List<AmenityItem>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ReservationId", reservationId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var amenity = new Amenities
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("AmenityId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                PricePerNight = reader.GetDouble(reader.GetOrdinal("PricePerNight"))
                            };

                            var amenityItem = new AmenityItem(amenity, reader.GetInt32(reader.GetOrdinal("Quantity")));

                            amenities.Add(amenityItem);
                        }
                    }
                }
            }

            return amenities;
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

        public void DeleteAllAmenitiesForReservation(int reservationId)
        {
            const string query = @"
        DELETE FROM project.reservation_amenities
        WHERE reservation_id = @ReservationId";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ReservationId", reservationId);
                    command.ExecuteNonQuery();
                }
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
