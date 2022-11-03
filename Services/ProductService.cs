using StoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using StoreApp.Database;
using System.Data.Common;

namespace StoreApp.Services
{
    public class ProductService : IProductService
    {
        private const string URL = "https://fakestoreapi.com/products";
        private const string applicationJsonMediaType = "application/json";

        public async Task<List<ProductModel>> DownloadProductList()
        {
            DbConnector dbConnector = new DbConnector();

            if (dbConnector.TryConnect())
            {
                List<ProductModel> products = dbConnector.GetProducts();

                if(products != null && products.Count() > 0)
                {
                    return products;
                }
            }

            HttpClient httpClient = new HttpClient();

            // Add an Accept header for JSON format.
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(applicationJsonMediaType));

            HttpResponseMessage httpResponse = httpClient.GetAsync(URL, HttpCompletionOption.ResponseHeadersRead).Result; //TODO change method to async

            httpResponse.EnsureSuccessStatusCode();

            if (httpResponse.Content is object && httpResponse.Content.Headers.ContentType.MediaType == applicationJsonMediaType)
            {
                using (Stream contentStream = await httpResponse.Content.ReadAsStreamAsync())
                {
                    using (StreamReader streamReader = new StreamReader(contentStream))
                    {
                        using (JsonTextReader jsonReader = new JsonTextReader(streamReader)) //check with newtonsoft reader
                        {
                            JsonSerializer serializer = new JsonSerializer();

                            try
                            {
                                List<ProductModel> products = serializer.Deserialize<List<ProductModel>>(jsonReader);

                                List<ProductModel> newProducts = dbConnector.SaveProducts(products);

                                return newProducts;
                            }
                            catch (JsonReaderException)
                            {
                                Console.WriteLine("Invalid JSON Format.");
                            }
                        }
                    }
                }
            }
            else
            {
                //throw new Exception($"Invalid HTTP response for {URL} request");
                Console.WriteLine($"Invalid HTTP response for {URL} request");
            }

            return new List<ProductModel>();
        }

        public async Task<ProductModel> RemoveProduct(int id)
        {
            DbConnector dbConnector = new DbConnector();

            if (dbConnector.TryConnect())
            {
                ProductModel product = dbConnector.DeleteProduct(id);

                if (product != null)
                {
                    return product;
                }
            }

            HttpClient httpClient = new HttpClient();

            // Add an Accept header for JSON format.
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(applicationJsonMediaType));

            HttpResponseMessage httpResponse = httpClient.DeleteAsync(string.Format("{0}/{1}", URL, id)).Result; //TODO change method to async

            httpResponse.EnsureSuccessStatusCode();

            if (httpResponse.Content is object && httpResponse.Content.Headers.ContentType.MediaType == applicationJsonMediaType)
            {
                using (Stream contentStream = await httpResponse.Content.ReadAsStreamAsync())
                {
                    using (StreamReader streamReader = new StreamReader(contentStream))
                    {
                        using (JsonTextReader jsonReader = new JsonTextReader(streamReader)) //check with newtonsoft reader
                        {
                            JsonSerializer serializer = new JsonSerializer();

                            try
                            {
                                ProductModel removedProduct = serializer.Deserialize<ProductModel> (jsonReader);
                                return removedProduct;
                            }
                            catch (JsonReaderException)
                            {
                                Console.WriteLine("Invalid JSON Format.");
                            }
                        }
                    }
                }
            }
            else
            {
                //throw new Exception($"Invalid HTTP response for {URL} request");
                Console.WriteLine($"Invalid HTTP response for {URL} request");
            }

            return null;
        }

        public async Task<ProductModel> AddProduct(ProductModel product)
        {
            DbConnector dbConnector = new DbConnector();

            if (dbConnector.TryConnect())
            {
                ProductModel newProduct = dbConnector.SaveSingleProduct(product);
                return newProduct;
            }

            HttpClient httpClient = new HttpClient();

            // Add an Accept header for JSON format.
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(applicationJsonMediaType));

            var val = JsonConvert.SerializeObject(product);

            StringContent stringContent = new StringContent(
                                                JsonConvert.SerializeObject(product),
                                                Encoding.UTF8, 
                                                "application/json");

            HttpResponseMessage httpResponse = httpClient.PostAsync(URL, stringContent).Result; //TODO change method to async

            httpResponse.EnsureSuccessStatusCode();

            if (httpResponse.Content is object && httpResponse.Content.Headers.ContentType.MediaType == applicationJsonMediaType)
            {
                using (Stream contentStream = await httpResponse.Content.ReadAsStreamAsync())
                {
                    using (StreamReader streamReader = new StreamReader(contentStream))
                    {
                        using (JsonTextReader jsonReader = new JsonTextReader(streamReader)) //check with newtonsoft reader
                        {
                            JsonSerializer serializer = new JsonSerializer();

                            try
                            {
                                ProductModel newProduct = serializer.Deserialize<ProductModel>(jsonReader);
                                return newProduct;
                            }
                            catch (JsonReaderException)
                            {
                                Console.WriteLine("Invalid JSON Format.");
                            }
                        }
                    }
                }
            }
            else
            {
                //throw new Exception($"Invalid HTTP response for {URL} request");
                Console.WriteLine($"Invalid HTTP response for {URL} request");
            }

            return null;
        }
    }
}