namespace Auktioner.Models
{
    public class SellerBuyer
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public string SellerId { get; set; }
        public string InventoryId { get; set; }
        public Inventory Inventory { get; set; }
        public decimal BidPrice { get; set; }
    }
}
