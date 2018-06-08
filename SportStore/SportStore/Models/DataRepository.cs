using System.Collections.Generic;
using System.Linq;

namespace SportStore.Models
{
    public class DataRepository : IRepository
    {
        private DataContext dataContext;

        public DataRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public IEnumerable<Product> Products => dataContext.Products.ToArray();

        public Product GetProduct(long key)
        {
            return dataContext.Products.Find(key);
        }

        public void AddProduct(Product product)
        {
            dataContext.Products.Add(product);
            dataContext.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            Product p = GetProduct(product.Id);
            p.Name = product.Name;
            p.Category = product.Category;
            p.PurchasePrice = product.PurchasePrice;
            p.RetailPrice = product.RetailPrice;
            //dataContext.Products.Update(product);
            dataContext.SaveChanges();
        }

        public void UpdateProducts(Product[] products)
        {
            IDictionary<long, Product> requestedProducts = products.ToDictionary(p => p.Id);
            IEnumerable<Product> baseline = dataContext.Products.Where(p => requestedProducts.Keys.Contains(p.Id));

            foreach (Product baselineProduct in baseline)
            {
                var requestedProduct = requestedProducts[baselineProduct.Id];
                baselineProduct.Name = requestedProduct.Name;
                baselineProduct.Category = requestedProduct.Category;
                baselineProduct.PurchasePrice = requestedProduct.PurchasePrice;
                baselineProduct.RetailPrice = requestedProduct.RetailPrice;
            }

            dataContext.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            dataContext.Products.Remove(product);
            dataContext.SaveChanges();
        }
    }
}
