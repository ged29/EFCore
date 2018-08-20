using BizDbAccess;
using BizLogic.GenericInterfaces;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BizLogic.Orders.Concrete
{
    public class PlaceOrderAction : BizActionErrors, IPlaceOrderAction
    {
        private readonly IPlaceOrderDbAccess dbAccess;

        public PlaceOrderAction(IPlaceOrderDbAccess dbAccess)
        {
            this.dbAccess = dbAccess;
        }

        public Order Action(PlaceOrderInDto dto)
        {
            if (!dto.AcceptTAndCs)
            {
                AddError("You must accept the T&Cs to place an order.");
                return null;
            }

            if (dto.LineItems.Count == 0)
            {
                AddError("No items in your basket.");
                return null;
            }

            var bookIds = dto.LineItems.Select(lineItem => lineItem.BookId);
            var booksDict = dbAccess.FindBooksByIdsWithPriceOffers(bookIds);
            var order = new Order
            {
                CustomerName = dto.UserId,
                LineItems = FormLineItemsWithErrorChecking(dto.LineItems, booksDict)
            };

            if (!HasErrors)
            {
                dbAccess.Add(order);
            }

            return HasErrors ? null : order;
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