using System;
using System.ComponentModel.DataAnnotations;

namespace Auktioner.Models
{
    public class SellingBuyingHistory
    {
        public int Id { get; set; }
        public string BuyerName { get; set; }
        public string SellerName { get; set; }
        public string InventoyrName { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }
}
