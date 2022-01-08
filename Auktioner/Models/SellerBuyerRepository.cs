using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Auktioner.Models
{
    public class SellerBuyerRepository : ISellerBuyerRepository
    {
        private readonly AppDbContext appDbContext;

        public SellerBuyerRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public IEnumerable<SellerBuyer> AllBids
        {
            get
            {
                return appDbContext.SellerBuyers.Include(b=>b.Inventory);
            }
        }
   

        public void AddBid(SellerBuyer sellerBuyer)
        {
            appDbContext.SellerBuyers.Add(sellerBuyer);
            appDbContext.SaveChanges();
        }

        public void RemoveAllBidsForItem(SellerBuyer sellerBuyer)
        {
            appDbContext.SellerBuyers.Remove(sellerBuyer);
            appDbContext.SaveChanges();
        }
    }
}
