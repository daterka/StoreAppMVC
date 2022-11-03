using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using StoreApp.Models;
using StoreApp.Services;
using StoreApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StoreApp.Controllers
{
    public class ProductController : Controller
    {
        private IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /*[ActionName("GetAllProducts")]*/
        /*[Route("products")]*/
        [HttpGet]
        [OutputCache(Duration = 20)]
        public ActionResult GetAllProducts()
        {
            List<Models.ProductModel> productsList = _productService.DownloadProductList().Result;

            if (productsList.Count == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            }

            return View("ProductsView", productsList);
        }

        /*[HttpDelete]
        public JsonResult DeleteProductById(int id)
        {

            *//*return new HttpStatusCodeResult(HttpStatusCode.OK, "aasd");*//*
            return Json(new
            {
                response =  $"Product with id = {id} successfully deleted."
            });
            *//*return Json(new
            {
                response =  $"Product with id = {id} successfully deleted."
            }, JsonRequestBehavior.AllowGet);*//*
        }*/

        [HttpDelete]
        public ActionResult RemoveProduct(int id)
        {

            ProductModel product = _productService.RemoveProduct(id).Result;

            if (product == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            }

            RemoveProductViewModel result = new RemoveProductViewModel
            {
                Message = string.Format("{0} has been successfully removed.", product.Title),
                Product = product
            };
            return Json(result);
        }

        [HttpPost]
        public ActionResult AddProduct(ProductModel product)
        {

            ProductModel newProduct = _productService.AddProduct(product).Result;

            if (newProduct.Id == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError, "There was an error during saving new Product.");
            }

            AddProductResponseModel result = new AddProductResponseModel
            {
                Message = string.Format("{0} has been successfully saved.", newProduct.Title),
                Product = newProduct
            };

            return Json(result);
        }
    }
}