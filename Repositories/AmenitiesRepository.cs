using System;
using System.Collections;
using System.Collections.Generic;
using Hotel.MVVM.Model;
using Npgsql;

namespace Hotel.Repositories
{
    public class AmenitiesRepository : IAmenitiesRepository
    {
        private readonly string _connectionString;

        public AmenitiesRepository()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["HotelDatabase"].ConnectionString;
        }

        public IEnumerable<Amenities> GetAllAmenities()
        {
            var amenities = new List<Amenities>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT id, name, description, price_per_night, max_available_per_day FROM project.amenities";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            amenities.Add(new Amenities
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                                PricePerNight = reader.GetDouble(3),
                                MaxAvailablePerDay = reader.GetInt32(4)
                            });
                        }
                    }
                }
            }

            return amenities;
        }

        public IEnumerable<Amenities> GetAmenitiesAvailableForDateRange(DateOnly startDate, DateOnly endDate)
        {
            var amenities = new List<Amenities>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("SELECT * FROM project.get_available_amenities(@StartDate, @EndDate)", connection))
                {
                    command.Parameters.AddWithValue("@StartDate", NpgsqlTypes.NpgsqlDbType.Date, startDate);
                    command.Parameters.AddWithValue("@EndDate", NpgsqlTypes.NpgsqlDbType.Date, endDate);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            amenities.Add(new Amenities
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("amenity_id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Description = reader.GetString(reader.GetOrdinal("description")),
                                MaxAvailablePerDay = reader.GetInt32(reader.GetOrdinal("available_quantity")),
                                PricePerNight = reader.GetDouble(reader.GetOrdinal("price_per_night")),
                            });
                        }
                    }
                }
            }

            return amenities;
        }

        public int AddAmenities(Amenities amenities)
        {
            int id = -1;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO project.amenities (name, description, price_per_night, max_available_per_day) " +
                               "VALUES (@name, @description, @pricePerNight, @maxAvailablePerDay) returning id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", amenities.Name);
                    command.Parameters.AddWithValue("@description", (object)amenities.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@pricePerNight", amenities.PricePerNight);
                    command.Parameters.AddWithValue("@maxAvailablePerDay", amenities.MaxAvailablePerDay);
                    id = (int)command.ExecuteScalar();

                    command.ExecuteNonQuery();
                }
            }

            return id;
        }

        public void AddAmenityToReservation(Amenities amenity, int reservationId, int quantity, double price)
        {
            const string query = @"INSERT INTO project.reservation_amenities (reservation_id, amenity_id, quantity, total_price)
                                 VALUES (@ReservationId, @AmenityId, @Quantity, @Price);";

            using (var connection = new NpgsqlConnection(_connectionString)) 
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ReservationId", reservationId);
                    command.Parameters.AddWithValue("@AmenityId", amenity.Id);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@Price", price);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateAmenities(Amenities amenities)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE project.amenities SET name = @name, description = @description, price_per_night = @pricePerNight, " +
                               "max_available_per_day = @maxAvailablePerDay WHERE id = @id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", amenities.Name);
                    command.Parameters.AddWithValue("@description", (object)amenities.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@pricePerNight", amenities.PricePerNight);
                    command.Parameters.AddWithValue("@maxAvailablePerDay", amenities.MaxAvailablePerDay);
                    command.Parameters.AddWithValue("@id", amenities.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteAmenities(Amenities amenities)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM project.amenities WHERE id = @id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", amenities.Id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
