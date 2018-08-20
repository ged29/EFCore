using DataLayer;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BizDbAccess
{
    public class PlaceOrderDbAccess : IPlaceOrderDbAccess
    {
        private readonly DataContext dataContext;

        public PlaceOrderDbAccess(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void Add(Order newOrder)
        {
            dataContext.Add(newOrder);
        }

        public IDictionary<int, Book> FindBooksByIdsWithPriceOffers(IEnumerable<int> bookIds)
        {
            return dataContext.Books
                .Where(book => bookIds.Contains(book.BookId))
                .Include(book => book.Promotion)
                .ToDictionary(book => book.BookId);
        }
    }
}
