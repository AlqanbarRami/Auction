using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auktioner.Models
{
    public class Category
    {
        [Display(Name = "Category Id")]
        public int CategoryId { get; set; }
        [Display(Name = "Category Name")]
        [Required]
        public string CategoryName { get; set; }

        public string Description { get; set; }
        List<Inventory> inventories { get; set; }   
    }
}
