using System.ComponentModel.DataAnnotations;

namespace Auktioner.Models
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        [Display(Name ="Item Id")]
        public string SpecialId { get; set; }
        [Display(Name = "Title")]
        [Required]
        public string InventoryName { get; set; }

        [Display(Name = "Image")]
        public string InventoryImage { get; set; }
        [Required]
        [Display(Name = "Year")]
        public int InventoryDecade { get; set; }
        [Display(Name = "Price")]
        [Required]
        public decimal StartPrice { get; set; }
        [Display(Name = "Final Price")]
        public decimal FinalPrice { get; set; }
        [Required]
        public string Description { get; set; }
       
        public string Status { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

    }
}
