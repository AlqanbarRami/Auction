using Auktioner.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Auktioner.Models
{
    public class CategoryRepository : ICategoryRepository   
    {
        private readonly AppDbContext appDbContext;

        public CategoryRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public IEnumerable<Category> Categories
        {
            get
            {
                return appDbContext.Categories;
            }
        }

        public void CreateCategory(Category category)
        {
      
                appDbContext.Categories.Add(category);
                appDbContext.SaveChanges();
            }

        public void DeleteCategory(Category category)
        {
            appDbContext.Categories.Remove(category);
            appDbContext.SaveChanges();
        }

        public void EditCategory(Category category)
        {
            appDbContext.Categories.Update(category);
            appDbContext.SaveChanges();
        }

    }
}
