using System.Collections.Generic;

namespace Auktioner.Models
{
    public interface ISellerBuyerRepository
    {
        IEnumerable<SellerBuyer> AllBids { get; }
        void AddBid(SellerBuyer sellerBuyer);

        void RemoveAllBidsForItem(SellerBuyer sellerBuyer);

  
    }
}
