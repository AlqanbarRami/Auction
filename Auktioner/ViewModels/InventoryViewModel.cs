using Auktioner.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Auktioner.ViewModels
{
    public class InventoryViewModel
    {
        public Inventory Inventory  { get; set; }
        public IEnumerable<Inventory> inventories { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<SellerBuyer> SellerBuyers { get; set; }


    }
}
