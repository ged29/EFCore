using DataLayer.Entities;
using System.Collections.Immutable;

namespace BizLogic.Orders
{
    public class Part1ToPart2Dto
    {
        public IImmutableList<OrderLineItem> LineItems { get; private set; }

        public Order Order { get; private set; }

        public Part1ToPart2Dto(IImmutableList<OrderLineItem> lineItems, Order order)
        {
            LineItems = lineItems;
            Order = order;
        }
    }
}