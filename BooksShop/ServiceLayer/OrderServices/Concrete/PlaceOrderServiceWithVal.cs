using BizDbAccess;
using BizLogic.Orders;
using BizLogic.Orders.Concrete;
using DataLayer;
using DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using ServiceLayer.BizRunners;
using ServiceLayer.CheckoutServices.Concrete;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServiceLayer.OrderServices.Concrete
{
    public class PlaceOrderServiceWithVal
    {
        private readonly CheckoutCookie checkoutCookie;
        private readonly RunnerWriteDbWithValidation<PlaceOrderInDto, Order> runner;

        public IImmutableList<ValidationResult> Errors => runner.Errors;

        public PlaceOrderServiceWithVal(
            IRequestCookieCollection cookiesIn,
            IResponseCookies cookiesOut,
            DataContext dataContext)
        {
            IPlaceOrderDbAccess dbAccess = new PlaceOrderDbAccess(dataContext);
            IPlaceOrderAction placeOrderAction = new PlaceOrderAction(dbAccess);
            runner = new RunnerWriteDbWithValidation<PlaceOrderInDto, Order>(placeOrderAction, dataContext);
            checkoutCookie = new CheckoutCookie(cookiesIn, cookiesOut);
        }

        public int PlaceOrder(bool acceptTAndCs)
        {
            var checkoutService = new CheckoutCookieService(checkoutCookie.GetValue());
            var placeOrderInDto = new PlaceOrderInDto(acceptTAndCs, checkoutService.UserId, checkoutService.LineItems);
            var order = runner.RunAction(placeOrderInDto);

            if (runner.HasErrors) return 0;

            checkoutService.ClearAllLineItems();
            checkoutCookie.AddOrUpdateCookie(checkoutService.EncodeForCookie());

            return order.OrderId;
        }
    }
}
