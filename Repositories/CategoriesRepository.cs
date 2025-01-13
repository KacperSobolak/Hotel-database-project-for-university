using System.Collections.Generic;
using Npgsql;
using Hotel.MVVM.Model;

namespace Hotel.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly string _connectionString;

        public CategoriesRepository()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["HotelDatabase"].ConnectionString;
        }

        public IEnumerable<Category> GetCategories()
        {
            var categories = new List<Category>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM project.room_categories";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new Category
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                PricePerAdultPerNight = reader.GetDouble(3)
                            });
                        }
                    }
                }
            }

            return categories;
        }

        public Category GetCategoryById(int id)
        {
            var category = new Category();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM project.room_categories WHERE id = @id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            category.Id = reader.GetInt32(0);
                            category.Name = reader.GetString(1);
                            category.Description = reader.GetString(2);
                            category.PricePerAdultPerNight = reader.GetDouble(3);
                        }
                    }
                }
            }
            return category;
        }

        public int AddCategory(Category category)
        {
            int id = -1;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO project.room_categories (name, description, price_per_night_per_adult) VALUES (@category, @description, @price) returning id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@category", category.Name);
                    command.Parameters.AddWithValue("@description", category.Description);
                    command.Parameters.AddWithValue("@price", category.PricePerAdultPerNight);
                    id = (int)command.ExecuteScalar();
                }

                return id;
            }
        }

        public void UpdateCategory(Category category)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE project.room_categories SET name = @category, description = @description, price_per_night_per_adult = @price WHERE id = @id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", category.Id);
                    command.Parameters.AddWithValue("@category", category.Name);
                    command.Parameters.AddWithValue("@description", category.Description);
                    command.Parameters.AddWithValue("@price", category.PricePerAdultPerNight);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCategory(int categoryId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM project.room_categories WHERE id = @Id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", categoryId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
