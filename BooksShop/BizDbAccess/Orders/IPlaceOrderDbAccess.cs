using System.Collections.Generic;
using DataLayer.Entities;

namespace BizDbAccess
{
    public interface IPlaceOrderDbAccess
    {
        void Add(Order newOrder);
        IDictionary<int, Book> FindBooksByIdsWithPriceOffers(IEnumerable<int> bookIds);
    }
}