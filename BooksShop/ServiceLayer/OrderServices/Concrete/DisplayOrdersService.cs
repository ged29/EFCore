using DataLayer;
using DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using ServiceLayer.CheckoutServices;
using ServiceLayer.CheckoutServices.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLayer.OrderServices.Concrete
{
    public class DisplayOrdersService
    {
        private DataContext dataContext;

        public DisplayOrdersService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public OrderListDto GetOrderDetail(int orderId)
        {
            var result = Select(dataContext.Orders).SingleOrDefault(order => order.OrderId == orderId);
            return result ?? throw new NullReferenceException($"Could not find the order with id of {orderId}.");
        }

        public IList<OrderListDto> GetUsersOrders(IRequestCookieCollection cookiesIn)
        {
            var checkoutCookieService = new CheckoutCookieService(cookiesIn);
            var userId = checkoutCookieService.UserId;
            var orders = dataContext.Orders.Where(order => order.CustomerName == userId).OrderByDescending(order => order.DateOrderedUtc);
            return Select(orders).ToList();
        }

        private IQueryable<OrderListDto> Select(IQueryable<Order> orders)
        {
            return orders.Select(order =>
            new OrderListDto
            {
                OrderId = order.OrderId,
                DateOrderedUtc = order.DateOrderedUtc,
                LineItems = order.LineItems.Select(lineItem =>
                new CheckoutItemDto
                {
                    BookId = lineItem.BookId,
                    Title = lineItem.ChosenBook.Title,
                    ImageUrl = lineItem.ChosenBook.ImageUrl,
                    AuthorsName = string.Join(", ", lineItem.ChosenBook.AuthorsLink.OrderBy(link => link.Order).Select(link => link.Author.Name)),
                    BookPrice = lineItem.BookPrice,
                    NumBooks = lineItem.NumBooks
                })
            });
        }
    }
}