using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using MvcReprApp.Models;

namespace MvcReprApp
{
    public class DatabaseHelper
    {
        private readonly string _connectionString = "Data Source=products.db";

        public List<CreateProductResponse> GetStudents()
        {
            var products = new List<CreateProductResponse>();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name, Price FROM Products";

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new CreateProductResponse
                    {
                        StatusCode = 0,
                        Product = new() {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Price = reader.GetInt32(2)
                        }
                    });
                }
            }
            return products;
        }

        public bool AddProduct(CreateProductRequest request)
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = "INSERT INTO Products (Name, Price) VALUES (@name, @price)";
                command.Parameters.AddWithValue("@name", request.Name);
                command.Parameters.AddWithValue("@price", request.Price);
                command.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}