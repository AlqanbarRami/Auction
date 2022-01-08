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
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly IInventoryRepository inventoryRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ISellingBuyingHistoryRepsotiry sellingBuyingHistoryRepsotiry;
        private readonly UserManager<Customer> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CustomerController(IInventoryRepository inventoryRepository, ICategoryRepository categoryRepository, UserManager<Customer> userManager, IHttpContextAccessor httpContextAccessor,
            ISellingBuyingHistoryRepsotiry sellingBuyingHistoryRepsotiry)
        {
            this.inventoryRepository = inventoryRepository;
            this.categoryRepository = categoryRepository;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            this.sellingBuyingHistoryRepsotiry = sellingBuyingHistoryRepsotiry;

        }

        public IActionResult GetItems()
        {          
            var inventories = inventoryRepository.AllInventory.Where(p => p.CustomerId == httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return View(inventories);
           
        }

  
        public IActionResult GetItemsBought(string customerId)
        {
            var name = userManager.Users.FirstOrDefault(u=>u.Id == customerId).Email;

            var items = sellingBuyingHistoryRepsotiry.AllSellingBuying.Where(s => s.BuyerName == name);

            return View(items);
        }

        public IActionResult NewInventory()
        {
            
            var categories = categoryRepository.Categories;
            if(categories == null)
            {
                RedirectToAction("AddCategory", "Category");
            }
            var inventory = inventoryRepository.AllInventory.Select(p => p.CategoryId);
            InventoryViewModel inventoryViewModel = new InventoryViewModel
            {
                Categories = categories.Select(c => new SelectListItem() { Text = c.CategoryName, Value = c.CategoryId.ToString() }).ToList(),
                CategoryId = categories.FirstOrDefault().CategoryId,

            };
            return View(inventoryViewModel);
        }

        [HttpPost]
        public IActionResult NewInventory(InventoryViewModel inventoryViewModel)
        {
            var getDecade = inventoryRepository.ChangeYearToDecade(inventoryViewModel.Inventory.InventoryDecade);
            inventoryViewModel.Inventory.CategoryId = inventoryViewModel.CategoryId;
            inventoryViewModel.Inventory.SpecialId = inventoryRepository.GetSpecialId();
            inventoryViewModel.Inventory.StartPrice += inventoryRepository.GetSpecialPrice(getDecade);
            inventoryViewModel.Inventory.CustomerId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            inventoryViewModel.Inventory.Status = "Auction started";
            if (ModelState.IsValid)
            {
                inventoryRepository.AddInventory(inventoryViewModel.Inventory);
                return RedirectToAction("Inventarie","Inventory");
            }
            return View(inventoryViewModel);
        }

        public IActionResult UpdateInventory(int InventoryId)
        {
            var categories = categoryRepository.Categories;
            var invetnory = inventoryRepository.AllInventory.FirstOrDefault(p => p.InventoryId == InventoryId);

            var inventoryViewModel = new InventoryViewModel
            {
                Categories = categories.Select(c => new SelectListItem() { Text = c.CategoryName, Value = c.CategoryId.ToString() }).ToList(),
                Inventory = invetnory ,
               

                
            };

            return View(inventoryViewModel);
        }

        [HttpPost]
        public IActionResult UpdateInventory(InventoryViewModel inventoryViewModel)
        {
            var getDecade = inventoryRepository.ChangeYearToDecade(inventoryViewModel.Inventory.InventoryDecade);
            inventoryViewModel.Inventory.CategoryId = inventoryViewModel.CategoryId;
            inventoryViewModel.Inventory.StartPrice += inventoryRepository.GetSpecialPrice(getDecade);

            if (ModelState.IsValid)
            {
                inventoryRepository.EditInventory(inventoryViewModel.Inventory);
                return RedirectToAction("Inventarie", "Inventory");
            }
            return View(inventoryViewModel);
        }

        [HttpPost]
        public IActionResult Delete(int InventoryId)
        {
            var item = inventoryRepository.AllInventory.FirstOrDefault(i => i.InventoryId == InventoryId);
            inventoryRepository.RemoveInventory(item);
            return RedirectToAction("Inventarie", "Inventory");
        
        }
    }
}
