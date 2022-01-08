using System.Collections.Generic;

namespace Auktioner.Models
{
    public interface ISellingBuyingHistoryRepsotiry
    {
        public IEnumerable<SellingBuyingHistory> AllSellingBuying { get; }
        public void AddPurchase(SellingBuyingHistory sellingBuyingHistory);

    }
}
