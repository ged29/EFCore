using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace SportStore.Models
{
    public class ProductRepository : IProductRepository
    {
        private DataContext dataContext;

        public ProductRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public IEnumerable<Product> Products => dataContext.Products.Include(p => p.Category).ToArray();

        public Product GetProduct(long id)
        {
            return dataContext.Products.Include(p => p.Category).First(p => p.Id == id);
        }

        public void AddProduct(Product product)
        {
            dataContext.Products.Add(product);
            dataContext.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            Product baseline = dataContext.Products.Find(product.Id);
            baseline.Name = product.Name;
            baseline.PurchasePrice = product.PurchasePrice;
            baseline.RetailPrice = product.RetailPrice;
            baseline.CategoryId = product.CategoryId;

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
