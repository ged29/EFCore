using DataLayer;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.OrderServices.Concrete;

namespace EfCoreInAction.Controllers
{
    public class OrdersController : BaseTraceController
    {
        private readonly DataContext dataContext;

        public OrdersController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public IActionResult Index()
        {
            var service = new DisplayOrdersService(dataContext);

            SetupTraceInfo();
            return View(service.GetUsersOrders(HttpContext.Request.Cookies));
        }

        public IActionResult ConfirmOrder(int orderId)
        {
            var service = new DisplayOrdersService(dataContext);

            SetupTraceInfo();
            return View(service.GetOrderDetail(orderId));
        }
    }
}
