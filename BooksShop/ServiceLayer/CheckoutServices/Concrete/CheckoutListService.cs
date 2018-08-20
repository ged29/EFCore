using BizLogic.Orders;
using DataLayer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace ServiceLayer.CheckoutServices.Concrete
{
    public class CheckoutListService
    {
        private readonly DataContext dataContext;
        private readonly IRequestCookieCollection cookiesIn;

        public CheckoutListService(DataContext dataContext, IRequestCookieCollection cookiesIn)
        {
            this.dataContext = dataContext;
            this.cookiesIn = cookiesIn;
        }

        public ImmutableList<CheckoutItemDto> GetCheckoutList()
        {
            var checkoutCookieService = new CheckoutCookieService(cookiesIn);
            var lineItems = checkoutCookieService.LineItems;

            return GetCheckoutList(lineItems);
        }

        public ImmutableList<CheckoutItemDto> GetCheckoutList(IImmutableList<OrderLineItem> lineItems)
        {
            var result = new List<CheckoutItemDto>();

            foreach (var lineItem in lineItems)
            {
                var item = dataContext.Books.Select(book => new CheckoutItemDto
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    AuthorsName = string.Join(", ", book.AuthorsLink.OrderBy(link => link.Order).Select(link => link.Author.Name)),
                    BookPrice = book.Promotion == null ? book.Price : book.Promotion.NewPrice,
                    ImageUrl = book.ImageUrl,
                    NumBooks = lineItem.NumBooks
                }).Single(book => book.BookId == lineItem.BookId);

                result.Add(item);
            }

            return result.ToImmutableList();
        }
    }
}
