using Auktioner.Models;
using System.Collections.Generic;

namespace Auktioner.ViewModels
{
    public class CategoryViewModel
    {
  
        public IEnumerable<Category> Categories { get; set; }
        public Category category { get; set; }
        public string CategoryName  { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }

    }
}
