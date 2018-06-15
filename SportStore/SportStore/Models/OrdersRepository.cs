using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SportStore.Models
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly DataContext dataContext;

        public OrdersRepository(DataContext dataContext) => this.dataContext = dataContext;

        public IEnumerable<Order> Orders => dataContext.Orders.Include(o => o.Lines).ThenInclude(o => o.Product).ToArray();

        public Order GetOrder(long key) => dataContext.Orders.Include(o => o.Lines).ThenInclude(o => o.Product).First(o => o.Id == key);

        public void AddOrder(Order order)
        {
            dataContext.Orders.Add(order);
            dataContext.SaveChanges();
        }

        public void UpdateOrder(Order order)
        {
            dataContext.Orders.Update(order);
            dataContext.SaveChanges();
        }

        public void DeleteOrder(Order order)
        {
            dataContext.Orders.Remove(order);
            dataContext.SaveChanges();
        }
    }
}
