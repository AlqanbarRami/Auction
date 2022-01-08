using Auktioner.ViewModels;
using System.Collections.Generic;

namespace Auktioner.Models
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> Categories { get; }
        void CreateCategory(Category category);
        void EditCategory(Category categoryViewModel);
        void DeleteCategory(Category category); 
    }
}
