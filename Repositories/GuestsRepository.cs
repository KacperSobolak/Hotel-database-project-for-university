using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.MVVM.Model;
using Npgsql;

namespace Hotel.Repositories
{
    class GuestsRepository : IGuestsRepository
    {
        private readonly string _connectionString;

        public GuestsRepository()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["HotelDatabase"].ConnectionString;
        }

        public IEnumerable<Guest> GetGuests()
        {
            var guests = new List<Guest>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM project.guests";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            guests.Add(new Guest
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Surname = reader.GetString(2),
                                Phone = reader.GetString(3),
                                Email = reader.IsDBNull(4) ? "Nie podano" : reader.GetString(4),
                                DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime(5)),
                            });
                        }
                    }
                }
            }

            return guests;
        }

        public int AddGuest(Guest guest)
        {
            int i = -1;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO project.guests (name, surname, phone_number, email, date_of_birth) VALUES (@name, @surname, @phone, @email, @date_of_birth) returning id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", guest.Name);
                    command.Parameters.AddWithValue("@surname", guest.Surname);
                    command.Parameters.AddWithValue("@phone", guest.Phone);
                    command.Parameters.AddWithValue("@email", guest.Email);
                    command.Parameters.AddWithValue("@date_of_birth", guest.DateOfBirth);
                    i = (int)command.ExecuteScalar();
                }
            }
            return i;
        }

        public void UpdateGuest(Guest guest)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE project.guests SET name = @name, surname = @surname, phone_number = @phone, email = @email, date_of_birth = @date_of_birth WHERE id = @id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", guest.Name);
                    command.Parameters.AddWithValue("@surname", guest.Surname);
                    command.Parameters.AddWithValue("@phone", guest.Phone);
                    command.Parameters.AddWithValue("@email", guest.Email);
                    command.Parameters.AddWithValue("@date_of_birth", guest.DateOfBirth);
                    command.Parameters.AddWithValue("@id", guest.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteGuest(int guestId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM project.guests WHERE id = @id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", guestId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public int GetGuestsNumber()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM project.guests";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
    }
}
