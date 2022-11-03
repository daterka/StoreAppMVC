using StoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreApp.ViewModels
{
    public class RemoveProductViewModel
    {
        public string Message { get; set; }
        public ProductModel Product { get; set; } 
    }
}