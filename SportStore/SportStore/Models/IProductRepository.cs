using System.Collections.Generic;

namespace SportStore.Models
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }

        Product GetProduct(long key);

        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void UpdateProducts(Product[] products);
        void DeleteProduct(Product product);
    }
}
