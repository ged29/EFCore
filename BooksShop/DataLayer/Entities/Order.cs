using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderNumber => $"SO{OrderId:D6}";

        public Guid CustomerName { get; set; }
        public DateTime DateOrderedUtc { get; set; }
        public ICollection<LineItem> LineItems { get; set; }

        public Order()
        {
            DateOrderedUtc = DateTime.UtcNow;
        }
    }
}