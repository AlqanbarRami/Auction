using Auktioner.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Test
{
    public class AuctionsTest
    {
      

        [Fact]
        public void TestEditCategory()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("DefaultConnection").Options;
            using (var appDbContext = new AppDbContext(options))
            {
                CategoryRepository CategoryToEdit = new CategoryRepository(appDbContext);
                var ToEdit = CategoryToEdit.Categories.Where(c => c.CategoryId == 1).FirstOrDefault();
                ToEdit.CategoryName = "Test";
                CategoryToEdit.EditCategory(CategoryToEdit.Categories.Where(c=>c.CategoryId == 1).FirstOrDefault());

                var ActualName = CategoryToEdit.Categories.Where(c => c.CategoryName == "Test").FirstOrDefault().CategoryName;
                var ExpectedName = "Test";

                Assert.Equal(ExpectedName, ActualName);
          
            }
        }

        [Fact]
        public void TestingStartPrice()
        {
            InventoryRepository inventoryRepository = new InventoryRepository();
            Inventory inventory = new Inventory()
            {
                InventoryDecade = 2000,
                StartPrice = 10,
            };

            decimal GetDecade = inventoryRepository.ChangeYearToDecade(inventory.InventoryDecade);
            inventory.StartPrice += inventoryRepository.GetSpecialPrice(GetDecade);
            decimal expectPrice = 32;

            Assert.Equal(expectPrice, inventory.StartPrice);

        }

        [Fact]
        public void TestAddNewCategory()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("DefaultConnection").Options;
            using (var appDbContext = new AppDbContext(options))
            {

                CategoryRepository Categories = new CategoryRepository(appDbContext);
                Category category = new Category { CategoryId = 10, CategoryName = "Test", Description = "Test" };
                Categories.CreateCategory(category); //I have Five before, Now should be 6

                var NumberOfCategories = Categories.Categories.Count();
                var ExpectedNumber = 6;

                Assert.Equal(ExpectedNumber, NumberOfCategories);
                
             

            }
        }

        [Fact]
        public void MakeItemAsSold()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("DefaultConnection").Options;
            using (var appDbContext = new AppDbContext(options))
            {
                InventoryRepository inventory = new InventoryRepository(appDbContext);
                var item = inventory.AllInventory.Where(i => i.InventoryId == 1).FirstOrDefault();
                item.Status = "Closed";
                inventory.EditInventory(item);

                var ActualTotalItemsSold = inventory.AllInventory.Where(i => i.Status == "Closed").Count();
                var ExpectedNumber = 1;

                Assert.Equal(ExpectedNumber, ActualTotalItemsSold);
            }
        }

             [Fact]
            public void MakeItemAsDelivered()
           {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("DefaultConnection").Options;
            using (var appDbContext = new AppDbContext(options))
            {
                InventoryRepository inventory = new InventoryRepository(appDbContext);
                SellingBuyingHistoryRepository sellingBuyingHistoryRepository = new SellingBuyingHistoryRepository(appDbContext);
                var item = inventory.AllInventory.Where(i => i.InventoryId == 1).FirstOrDefault();
                item.Status = "Delivered";
                inventory.EditInventory(item);
                SellingBuyingHistory sellingBuyingHistory = new SellingBuyingHistory
                {
             
                    Price = item.StartPrice

                };
                sellingBuyingHistoryRepository.AddPurchase(sellingBuyingHistory);


                var ActualCountingNumber = sellingBuyingHistoryRepository.AllSellingBuying.Count();
                var ExpectedCountingNumber = 1;

                //Let's check the other table and finding status = Delivered. so important for the project this step
                var ActualNumberAsDelivered = inventory.AllInventory.Where(i => i.Status == "Delivered").Count();
                var ExpectedNumberAsDelivered = 1;

                Assert.Equal(ExpectedCountingNumber, ActualCountingNumber);
                Assert.Equal(ExpectedNumberAsDelivered, ActualNumberAsDelivered);
            }
        }



    }
}
