using StoreApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreApp.Services
{
    public interface IProductService
    {
        Task<ProductModel> AddProduct(ProductModel product);
        Task<List<ProductModel>> DownloadProductList();
        Task<ProductModel> RemoveProduct(int id);
    }
}