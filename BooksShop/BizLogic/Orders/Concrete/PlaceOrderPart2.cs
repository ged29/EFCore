using BizDbAccess;
using BizLogic.GenericInterfaces;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BizLogic.Orders.Concrete
{
    public class PlaceOrderPart2 : BizActionErrors, IPlaceOrderPart2
    {
        private readonly IPlaceOrderDbAccess dbAccess;

        public PlaceOrderPart2(IPlaceOrderDbAccess dbAccess)
        {
            this.dbAccess = dbAccess;
        }

        public Order Action(Part1ToPart2Dto dto)
        {
            var bookIds = dto.LineItems.Select(lineItem => lineItem.BookId);
            var booksDict = dbAccess.FindBooksByIdsWithPriceOffers(bookIds);
            dto.Order.LineItems = FormLineItemsWithErrorChecking(dto.LineItems, booksDict);

            return HasErrors ? null : dto.Order;
        }

        public List<LineItem> FormLineItemsWithErrorChecking(
            IEnumerable<OrderLineItem> lineItems,
            IDictionary<int, Book> booksDict)
        {
            var result = new List<LineItem>();
            byte line = 1;

            foreach (var lineItem in lineItems)
            {
                if (!booksDict.ContainsKey(lineItem.BookId))
                {
                    throw new InvalidOperationException($"Could not find the {i} book you wanted to order. Please remove that book and try again.");
                }

                var book = booksDict[lineItem.BookId];
                var bookPrice = book.Promotion?.NewPrice ?? book.Price;

                if (book.PublishedOn > DateTime.UtcNow)
                {
                    AddError($"Sorry, the book '{book.Title}' is not yet in print.");
                }
                else if (bookPrice <= 0)
                {
                    AddError($"Sorry, the book '{book.Title}' is not for sale.");
                }
                else
                {
                    result.Add(new LineItem
                    {
                        NumBooks = lineItem.NumBooks,
                        BookPrice = bookPrice,
                        ChosenBook = book,
                        LineNum = line++
                    });
                }
            }

            return result;
        }
    }
}