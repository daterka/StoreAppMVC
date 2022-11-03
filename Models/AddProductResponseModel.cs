using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace StoreApp.Models
{
    public class AddProductResponseModel
    {
        public string Message { set; get; }
        public ProductModel Product { set; get; }  
    }
}