using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using MvcReprApp.Models;

namespace MvcReprApp.Database
{
    public class DatabaseHelper
    {
        private readonly string _connectionString = "Data Source=products.db";

        static private void EnsureCreated()
        {
            using var context = new ProductsContext();
            context.Database.EnsureCreated();
            if (!context.Products.Any())
            {
                context.Products.Add(new Product { Name = "meat", Price = 22 });
                context.Products.Add(new Product { Name = "bread", Price = 20 });
                context.SaveChanges();
            }
        }

        public List<CreateProductResponse> GetProducts()
        {
            var products = new List<CreateProductResponse>();

            EnsureCreated();
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
                        Product = new()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Price = reader.GetInt32(2)
                        }
                    });
                }
            }
            return products;
        }

        public CreateProductResponse? GetProductById(int id)
        {
            EnsureCreated();
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT Id, Name, Price FROM Products WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new()
                {
                    StatusCode = 0,
                    Product = new()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Price = reader.GetInt32(2)
                    }
                };
            }
            return null;
        }

        public long AddProduct(CreateProductRequest request)
        {
            try
            {
                EnsureCreated();
                using var connection = new SQLiteConnection(_connectionString);
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText =
                    "INSERT INTO Products (Name, Price) VALUES (@name, @price)";
                command.Parameters.AddWithValue("@name", request.Name);
                command.Parameters.AddWithValue("@price", request.Price);
                command.ExecuteNonQuery();

                command.CommandText = "SELECT last_insert_rowid();";
                return (long)command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        public bool UpdateProductById(int id, CreateProductRequest product)
        {
            try
            {
                EnsureCreated();
                using var connection = new SQLiteConnection(_connectionString);
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE Products
                    SET Name = @name, Price = @price
                    WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", product.Name);
                command.Parameters.AddWithValue("@price", product.Price);

                int rowsUpdated = command.ExecuteNonQuery();
                return rowsUpdated > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteProductById(int id)
        {
            try
            {
                EnsureCreated();
                using var connection = new SQLiteConnection(_connectionString);
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Products WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);

                int rowsDeleted = command.ExecuteNonQuery();
                return rowsDeleted > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteProduct(CreateProductRequest request)
        {
            try
            {
                EnsureCreated();
                using var connection = new SQLiteConnection(_connectionString);
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    "DELETE FROM Products WHERE Name = @name AND Price = @price";
                command.Parameters.AddWithValue("@name", request.Name);
                command.Parameters.AddWithValue("@price", request.Price);

                int rowsDeleted = command.ExecuteNonQuery();
                return rowsDeleted > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}