using System.Collections.Generic;
using System.Linq;

namespace SportStore.Models
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext dataContext;

        public CategoryRepository(DataContext dataContext) => this.dataContext = dataContext;

        public IEnumerable<Category> Categories => dataContext.Categories.ToArray();

        public void AddCategory(Category category)
        {
            dataContext.Categories.Add(category);
            dataContext.SaveChanges();
        }

        public void DeleteCategory(Category category)
        {
            dataContext.Categories.Remove(category);
            dataContext.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            dataContext.Categories.Update(category);
            dataContext.SaveChanges();
        }
    }
}
