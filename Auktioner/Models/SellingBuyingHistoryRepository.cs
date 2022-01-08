using System.Collections.Generic;

namespace Auktioner.Models
{
    public class SellingBuyingHistoryRepository : ISellingBuyingHistoryRepsotiry
    {
        private readonly AppDbContext appDbContext;

        public SellingBuyingHistoryRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public IEnumerable<SellingBuyingHistory> AllSellingBuying
        {
            get
            {
                return appDbContext.sellingBuyingHistories;
            }
        }


        public void AddPurchase(SellingBuyingHistory sellingBuyingHistory)
        {
            appDbContext.sellingBuyingHistories.Add(sellingBuyingHistory);
            appDbContext.SaveChanges();
        }
    }
}
