using StoreApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Xml.Linq;

namespace StoreApp.Database
{
    public class DbConnector
    {
        private const string ConnectionString =
            "Server=localhost\\sqlexpress; Database=StoreDb; Integrated Security=True;";

        public bool TryConnect()
        {
            using (SqlConnection connection =
                new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    connection.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        public List<ProductModel> GetProducts()
        {
            List<ProductModel> products = new List<ProductModel>();

            using (SqlConnection connection =
                new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("select * from products", connection);
                try
                {
                    connection.Open();

                    SqlDataReader dreader = command.ExecuteReader();
                    while (dreader.Read())
                    {
                        ProductModel product = new ProductModel
                        {
                            Id = Int32.Parse(dreader[0].ToString()),
                            Title = dreader[1].ToString(),
                            Price = dreader[2].ToString(),
                            Category = dreader[3].ToString(),
                            Description = dreader[4].ToString(),
                            Image = dreader[5].ToString()
                        };

                        products.Add(product);
                    }
               /*     else
                    {
                        throw new Exception("No Record");
                    }*/
                    dreader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Selecting products failed: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return products;

            }
        }

        public ProductModel GetSingleProduct(int id)
        {
            ProductModel product = null;
            using (SqlConnection connection =
                new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("select * from products where id="+id, connection);
                try
                {
                    connection.Open();

                    SqlDataReader dreader = command.ExecuteReader();
                    if (dreader.Read())
                    {
                        product = new ProductModel
                        {
                            Id = Int32.Parse(dreader[0].ToString()),
                            Title = dreader[1].ToString(),
                            Price = dreader[2].ToString(),
                            Category = dreader[3].ToString(),
                            Description = dreader[4].ToString(),
                            Image = dreader[5].ToString()
                        };
                    }
                    else
                    {
                        throw new Exception("No Record");
                    }
                    dreader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Selecting products failed: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return product;

            }
        }

        public List<ProductModel> SaveProducts(List<ProductModel> products)
        {
            using (SqlConnection connection =
                new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    products.ForEach(product => {
                        SqlCommand command =
                            new SqlCommand(
                                "insert into products values(@Title, @Price, @Category, @Description, @Image)",
                                connection);

                        command.Parameters.AddWithValue("@Title", product.Title); // change to ADD becaouse of poor performance
                        command.Parameters.AddWithValue("@Price", product.Price);
                        command.Parameters.AddWithValue("@Category", product.Category);
                        command.Parameters.AddWithValue("@Description", product.Description);
                        command.Parameters.AddWithValue("@Image", product.Image);

                        command.ExecuteNonQuery();
                    });

                    connection.Close();

                    return GetProducts();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to save a product in DB: " + ex.Message);
                    return null;
                }
            }
        }

        public ProductModel SaveSingleProduct(ProductModel product)
        {
            using (SqlConnection connection =
                new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command =
                            new SqlCommand(
                                "insert into products values(@Title, @Price, @Category, @Description, @Image)",
                                connection);

                    command.Parameters.AddWithValue("@Title", product.Title); // change to ADD becaouse of poor performance
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Category", product.Category);
                    command.Parameters.AddWithValue("@Description", product.Description);
                    command.Parameters.AddWithValue("@Image", product.Image);

                    command.ExecuteNonQuery();

                    connection.Close();

                    /*return GetSingleProduct()*/;
                    return GetProducts().Last();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to save a product in DB: " + ex.Message);
                    return null;
                }
            }
        }

        public ProductModel DeleteProduct(int id)
        {
            using (SqlConnection connection =
                new SqlConnection(ConnectionString))
            {
                try
                {
                    ProductModel product = GetSingleProduct(id); 

                    connection.Open();
                    SqlCommand command =
                            new SqlCommand(
                                "delete from products where id="+id,
                                connection);

                    command.ExecuteNonQuery();

                    connection.Close();

                    /*return GetSingleProduct()*/
                    ;
                    return product;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to save a product in DB: " + ex.Message);
                    return null;
                }
            }
        }
    }
}