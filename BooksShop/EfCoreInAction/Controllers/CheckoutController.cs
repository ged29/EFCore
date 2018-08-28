using BizLogic.Orders;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.CheckoutServices.Concrete;
using ServiceLayer.OrderServices.Concrete;
using System.Linq;

namespace EfCoreInAction.Controllers
{
    public class CheckoutController : BaseTraceController
    {
        private readonly DataContext dataContext;

        public CheckoutController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public IActionResult Index()
        {
            var service = new CheckoutListService(dataContext, HttpContext.Request.Cookies);

            SetupTraceInfo();
            return View(service.GetCheckoutList());
        }

        public IActionResult Buy(OrderLineItem itemToBuy)
        {
            var cookie = new CheckoutCookie(HttpContext.Request.Cookies, HttpContext.Response.Cookies);
            var service = new CheckoutCookieService(cookie.GetValue());

            service.AddLineItem(itemToBuy);
            cookie.AddOrUpdateCookie(service.EncodeForCookie());

            SetupTraceInfo();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteLineItem(int lineNum)
        {
            var cookie = new CheckoutCookie(HttpContext.Request.Cookies, HttpContext.Response.Cookies);
            var service = new CheckoutCookieService(cookie.GetValue());

            service.DeleteLineItem(lineNum);
            cookie.AddOrUpdateCookie(service.EncodeForCookie());

            SetupTraceInfo();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult PlaceOrder(bool acceptTAndCs)
        {
            var service = new PlaceOrderService(HttpContext.Request.Cookies, HttpContext.Response.Cookies, dataContext);
            var orderId = service.PlaceOrder(acceptTAndCs);

            if (service.Errors.Count == 0)
            {
                return RedirectToAction("ConfirmOrder", "Orders", new { orderId });
            }

            foreach (var error in service.Errors)
            {
                var memberNames = error.MemberNames.ToList();
                ModelState.AddModelError(memberNames.Any() ? memberNames.First() : "", error.ErrorMessage);
            }

            var listService = new CheckoutListService(dataContext, HttpContext.Request.Cookies);

            SetupTraceInfo();
            return View(listService.GetCheckoutList());
        }
    }
}
