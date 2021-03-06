﻿using BizDbAccess;
using BizLogic.Orders;
using BizLogic.Orders.Concrete;
using DataLayer;
using DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using ServiceLayer.BizRunners;
using ServiceLayer.CheckoutServices.Concrete;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.OrderServices.Concrete
{
    public class PlaceOrderService
    {
        private readonly CheckoutCookie checkoutCookie;
        private readonly RunnerWriteDb<PlaceOrderInDto, Order> runner;

        public IImmutableList<ValidationResult> Errors => runner.Errors;

        public PlaceOrderService(
            IRequestCookieCollection cookiesIn,
            IResponseCookies cookiesOut,
            DataContext dataContext)
        {
            IPlaceOrderDbAccess dbAccess = new PlaceOrderDbAccess(dataContext);
            IPlaceOrderAction placeOrderAction = new PlaceOrderAction(dbAccess);
            runner = new RunnerWriteDb<PlaceOrderInDto, Order>(placeOrderAction, dataContext);
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
