using Auktioner.Models;
using Auktioner.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Auktioner.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ItemsManagementController : Controller
    {
        private readonly UserManager<Customer> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<Customer> signInManager;
        private readonly IInventoryRepository inventoryRepository;
        private readonly ISellerBuyerRepository sellerBuyerRepository;
        private readonly ISellingBuyingHistoryRepsotiry sellingBuyingHistoryRepsotiry;

        public ItemsManagementController(UserManager<Customer> userManager, RoleManager<IdentityRole> roleManager, SignInManager<Customer> signInManager, IInventoryRepository inventoryRepository, ISellerBuyerRepository sellerBuyerRepository, ISellingBuyingHistoryRepsotiry sellingBuyingHistoryRepsotiry)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.inventoryRepository = inventoryRepository;
            this.sellerBuyerRepository = sellerBuyerRepository;
            this.sellingBuyingHistoryRepsotiry = sellingBuyingHistoryRepsotiry;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAllItems()
        {
            var list = inventoryRepository.AllInventory.Where(i => i.Status != "Closed" && i.Status != "Delivered");

            return View(list);
        }

        public IActionResult GetAllBidding(string inventoryId)
        {
            var list = sellerBuyerRepository.AllBids.Where(s => s.InventoryId == inventoryId).OrderByDescending(b=>b.BidPrice);
            return View(list);
        }

        public IActionResult MakeItSold(string inventoryId, decimal finalPrice, string buyerId, string sellerId)
        {
            var getBuyerName = userManager.Users.Where(u=>u.Id == buyerId).FirstOrDefault()?.Email;
            var getSellerName = userManager.Users.Where(u => u.Id == sellerId).FirstOrDefault()?.Email;
            var inventory = inventoryRepository.AllInventory.Where(i => i.SpecialId == inventoryId).FirstOrDefault();
            inventory.Status = "Closed";
            inventory.FinalPrice = finalPrice;
            inventoryRepository.EditInventory(inventory);
            SellingBuyingHistory sellingBuyingHistory = new SellingBuyingHistory();
            sellingBuyingHistory.SellerName = getSellerName;
            sellingBuyingHistory.BuyerName = getBuyerName;
            sellingBuyingHistory.Price = inventory.FinalPrice;
            sellingBuyingHistory.InventoyrName = inventory.InventoryName;
            DateTime currentYear = DateTime.Now;
            sellingBuyingHistory.Date = currentYear;
            sellingBuyingHistoryRepsotiry.AddPurchase(sellingBuyingHistory);
            return RedirectToAction("GetAllItems");
        }

        public IActionResult ItemsSold(string inventoryId)
        {
            var list = inventoryRepository.AllInventory.Where(i => i.Status == "Closed");

            return View(list);
        }

        public IActionResult ItemDelivered(string inventoryId)
        {
            var list = inventoryRepository.AllInventory.Where(i => i.SpecialId == inventoryId).FirstOrDefault();
            var getBids = sellerBuyerRepository.AllBids.Where(b => b.InventoryId == inventoryId);
            foreach(var item in getBids.ToList())
            {
                sellerBuyerRepository.RemoveAllBidsForItem(item);
            }
            list.Status = "Delivered";
            inventoryRepository.EditInventory(list);
            return RedirectToAction("ItemsSold");
        }

        public IActionResult InformationSellerBuyer()    
        {
            var name = "";
            if (inventoryRepository.AllInventory.FirstOrDefault()?.InventoryName != null)
            {
                name = inventoryRepository.AllInventory.Where(i => i.Status == "Delivered").FirstOrDefault()?.InventoryName;
            }
               
                var list2 = sellingBuyingHistoryRepsotiry.AllSellingBuying.Where(s => s.InventoyrName == name).OrderBy(s => s.Id);
                return View(list2);
                
        }



    }
}
