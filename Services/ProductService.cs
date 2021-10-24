using System.Collections.Generic;
using App.Models;

namespace App.Service{

    public class ProductService : List<ProductModel>{
        public ProductService()
        {
            this.AddRange(new ProductModel[]{
                new ProductModel(){ID = 1, Name = "Samsung", Price = 100},
                new ProductModel(){ID = 2, Name = "Sony", Price = 200},
                new ProductModel(){ID = 3, Name = "Acer", Price = 300},
                new ProductModel(){ID = 4, Name = "Asus", Price = 400}
            });
        }
    }
}