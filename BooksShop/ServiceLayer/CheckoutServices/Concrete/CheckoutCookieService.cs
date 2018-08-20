using BizLogic.Orders;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ServiceLayer.CheckoutServices.Concrete
{
    public class CheckoutCookieService
    {
        private List<OrderLineItem> lineItems;
        /// <summary>
        /// Because we don't get user to log in we assign them a uniquie GUID and store it in the cookie
        /// </summary>
        public Guid UserId { get; private set; }
        public ImmutableList<OrderLineItem> LineItems => lineItems.ToImmutableList();

        public CheckoutCookieService(string cookieContent)
        {
            DecodeCookieString(cookieContent);
        }

        public CheckoutCookieService(IRequestCookieCollection cookiesIn)
        {
            var cookieHandler = new CheckoutCookie(cookiesIn);
            DecodeCookieString(cookieHandler.GetValue());
        }

        public void AddLineItem(OrderLineItem newItem)
        {
            lineItems.Add(newItem);
        }

        public void UpdateLineItem(int itemIndex, OrderLineItem replacement)
        {
            if (itemIndex < 0 || itemIndex > lineItems.Count)
            {
                throw new InvalidOperationException($"System error. Attempt to remove line item index {itemIndex}. _lineItems.Count = {lineItems.Count}");
            }

            lineItems[itemIndex] = replacement;
        }

        public void DeleteLineItem(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex > lineItems.Count)
            {
                throw new InvalidOperationException($"System error. Attempt to remove line item index {itemIndex}. lineItems.Count = {lineItems.Count}");
            }

            lineItems.RemoveAt(itemIndex);
        }

        public void ClearAllLineItems()
        {
            lineItems.Clear();
        }

        public string EncodeForCookie()
        {
            var sb = new StringBuilder();
            sb.Append(UserId.ToString("N"));

            foreach (var lineItem in lineItems)
            {
                sb.AppendFormat(",{0},{1}", lineItem.BookId, lineItem.NumBooks);
            }
            return sb.ToString();
        }

        private void DecodeCookieString(string cookieContent)
        {
            lineItems = new List<OrderLineItem>();

            if (cookieContent == null)
            {
                UserId = Guid.NewGuid();
                return;
            }
            //Has cookie, so decode it
            var tokens = cookieContent.Split(',');
            UserId = Guid.Parse(tokens[0]);

            for (int i = 0; i < (tokens.Length - 1) / 2; i++)
            {
                lineItems.Add(new OrderLineItem
                {
                    BookId = int.Parse(tokens[i * 2 + 1]),
                    NumBooks = short.Parse(tokens[i * 2 + 2])
                });
            }
        }
    }
}
