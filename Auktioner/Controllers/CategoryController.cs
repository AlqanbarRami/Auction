using Auktioner.Models;
using Auktioner.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace Auktioner.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IInventoryRepository inventoryRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ISellerBuyerRepository sellerBuyerRepository;

        public CategoryController(ICategoryRepository categoryRepository, IInventoryRepository inventoryRepository, ISellerBuyerRepository sellerBuyerRepository)
        {
            this.categoryRepository = categoryRepository;
            this.inventoryRepository = inventoryRepository;
            this.sellerBuyerRepository = sellerBuyerRepository;
        }
        public IActionResult Index()
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel();
            categoryViewModel.Categories = categoryRepository.Categories;
            return View(categoryViewModel);
        }

        public IActionResult IemtsInCategory(int CategoryId)
        {
            InventoryViewModel inventoryViewModel = new InventoryViewModel();
            if (inventoryRepository.AllInventory.FirstOrDefault() != null)
            {
                inventoryViewModel.inventories = inventoryRepository.AllInventory.Where(i => i.CategoryId == CategoryId).OrderBy(i => i.InventoryId);
                inventoryViewModel.CategoryName = inventoryRepository.AllInventory.FirstOrDefault()?.Category.CategoryName;
                inventoryViewModel.SellerBuyers = sellerBuyerRepository.AllBids;

            }
            return View(inventoryViewModel);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult AddCategory()
        {
            return View();  
        }

        [HttpPost]
        public IActionResult AddCategory(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.CreateCategory(categoryViewModel.category);
                return RedirectToAction("Index");
            }
            return View(); 
        }
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateCategory(int CategoryId)
        {
           
            Category categories = categoryRepository.Categories.FirstOrDefault(c => c.CategoryId == CategoryId);

            var categoryViewModel = new CategoryViewModel
            {          
                
                CategoryId = categories.CategoryId,
                CategoryName = categories.CategoryName,
                Description = categories.Description,
              
            };

            return View(categoryViewModel);
        }
       

        [HttpPost]
        public IActionResult UpdateCategory(CategoryViewModel categoryViewModel)
        {
            var cat = new Category { CategoryId = categoryViewModel.CategoryId , CategoryName = categoryViewModel.CategoryName, Description = categoryViewModel.Description   };
            if (ModelState.IsValid)
            {
                categoryRepository.EditCategory(cat);
                return RedirectToAction("Index");
            }
            return View(categoryViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteCategory(int CategoryId)
        {
            var CategoryToDelete = categoryRepository.Categories.FirstOrDefault(c => c.CategoryId == CategoryId);
            if (CategoryToDelete != null)
            {
                categoryRepository.DeleteCategory(CategoryToDelete);
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }
    }
}
