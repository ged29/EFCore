using Microsoft.AspNetCore.Mvc;
using SportStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IOrdersRepository ordersRepository;

        public OrdersController(IProductRepository productRepository, IOrdersRepository ordersRepository)
        {
            this.productRepository = productRepository;
            this.ordersRepository = ordersRepository;
        }

        public IActionResult Index() => View(ordersRepository.Orders);

        public IActionResult EditOrder(long id)
        {
            var products = productRepository.Products;
            Order order = id == 0 ? new Order() : ordersRepository.GetOrder(id);

            IDictionary<long, OrderLine> linesMap = order.Lines?.ToDictionary(l => l.ProductId) ?? new Dictionary<long, OrderLine>();

            ViewBag.Lines = products.Select(p => linesMap.ContainsKey(p.Id)
                    ? linesMap[p.Id]
                    : new OrderLine { Product = p, ProductId = p.Id, Quantity = 0 });

            return View(order);
        }

        [HttpPost]
        public IActionResult AddOrUpdateOrder(Order order)
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult DeleteOrder(Order order)
        {
            ordersRepository.DeleteOrder(order);
            return RedirectToAction(nameof(Index));
        }
    }
}
