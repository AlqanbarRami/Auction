using Auktioner.Models;
using Auktioner.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Security.Claims;

namespace Auktioner.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryRepository inventoryRepository;
        private readonly ISellerBuyerRepository sellerBuyerRepository;



        public InventoryController(IInventoryRepository inventoryRepository, ISellerBuyerRepository sellerBuyerRepository)
        {
            this.inventoryRepository = inventoryRepository;
            this.sellerBuyerRepository = sellerBuyerRepository;
      
         
        }

        public IActionResult Inventarie()
        {
            InventoryViewModel inventoryViewModel = new InventoryViewModel();
            if (inventoryRepository.AllInventory.FirstOrDefault() != null)
            {
                inventoryViewModel.inventories = inventoryRepository.AllInventory.Where(i=>i.Status == "Auction started").OrderBy(i=>i.InventoryId);
                inventoryViewModel.CategoryName = inventoryRepository.AllInventory.FirstOrDefault()?.Category.CategoryName;
                inventoryViewModel.SellerBuyers = sellerBuyerRepository.AllBids;
              
            }
            return View(inventoryViewModel);
        }

    
        [Authorize]
        [HttpPost]
        public IActionResult AddBid(string inventoryId , string SellerId, string BuyerId,decimal BidPrice)
        {
            decimal maxprice = 0;
            var orginalPrice = inventoryRepository.AllInventory.FirstOrDefault(i => i.SpecialId == inventoryId).StartPrice;
            SellerBuyer sellerBuyer = new SellerBuyer();
            sellerBuyer.InventoryId = inventoryId;
            sellerBuyer.SellerId = SellerId;
            sellerBuyer.BuyerId = BuyerId;
            if (sellerBuyerRepository.AllBids.FirstOrDefault(b=>b.InventoryId == inventoryId) != null)
            {
                var price = sellerBuyerRepository.AllBids.Where(b => b.InventoryId == inventoryId).Select(b => b.BidPrice).ToList();
                 maxprice = price.Max();
                sellerBuyer.BidPrice = maxprice + BidPrice;
                sellerBuyerRepository.AddBid(sellerBuyer);

            }
            else
            {
                sellerBuyer.BidPrice = BidPrice + orginalPrice;
                sellerBuyerRepository.AddBid(sellerBuyer);

            }

            return RedirectToAction("Inventarie");
        }

        




    }
}
