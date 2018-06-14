using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportStore.Models;

namespace SportStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;

        public HomeController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            ViewBag.UpdateAll = false;
            return View(productRepository.Products);
        }

        public IActionResult UpdateAll()
        {
            ViewBag.UpdateAll = true;
            return View(nameof(Index), productRepository.Products);
        }

        [HttpPost]
        public IActionResult UpdateAll(Product[] products)
        {
            Console.Clear();
            productRepository.UpdateProducts(products);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult UpdateProduct(long key)
        {
            ViewBag.Categories = this.categoryRepository.Categories;
            return View(key == 0 ? new Product() : productRepository.GetProduct(key));
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product product)
        {
            if (product.Id == 0)
            {
                productRepository.AddProduct(product);
            }
            else
            {
                productRepository.UpdateProduct(product);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult DeleteProduct(Product product)
        {
            productRepository.DeleteProduct(product);
            return RedirectToAction(nameof(Index));
        }
    }
}