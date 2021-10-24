using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Policy;
using App.Models;
using App.Service;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers{

    public class FirstController :Controller{
        private readonly ProductService _productService;
        public FirstController(ProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index(){
            return View();
        }

        public IActionResult Bird(){
            string filePath = Path.Combine(Startup.ContentRootPath,"Files", "Bird.jpg");
            byte[] bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes,"image/jpg");
        }

        public IActionResult ViewProduct(int? id ){
            ProductModel product = _productService.Where(p => p.ID == id).FirstOrDefault();
            if(product == null){
                TempData["StatusMessage"] = "Cannot find product";
                return RedirectToAction("Index","Home");
            }
            ViewData["product"] = product;
            return View();
            
        }
    }

}