using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Auktioner.Models
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly AppDbContext appDbContext;

        public InventoryRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public InventoryRepository()
        {
        }

        public IEnumerable<Inventory> AllInventory
        {
            get
            {
                return appDbContext.Inventories.Include(c => c.Category);
            }
        }

        public void AddInventory(Inventory inventory)
        {
            appDbContext.Inventories.Add(inventory);
            appDbContext.SaveChanges();
        }

  
        public void EditInventory(Inventory inventory)
        {
            appDbContext.Inventories.Update(inventory);
            appDbContext.SaveChanges();
        }


        public string GetSpecialId()
        {
            Random rnd = new Random();
            string numbers = "";
            for (int j = 0; j < 6; j++)
            {
                numbers += rnd.Next(6);
            }
            Random random = new Random();
            int length = 3;
            var Alpha = "";
            for (var i = 0; i < length; i++)
            {
                Alpha += ((char)(random.Next(1, 26) + 64)).ToString().ToUpper();
            }
            string SpecId = Alpha + " " + numbers;
            return SpecId;  
        }

        public decimal GetSpecialPrice(decimal decade)
        {
            decimal price = decade * 10;
            return price;
        }

        public decimal ChangeYearToDecade(int year)
        {
            DateTime currentYear = DateTime.Now;
            decimal CalYears = currentYear.Year - year;
            return (CalYears / 10);
        }

        public void RemoveInventory(Inventory inventory)
        {
            appDbContext.Remove(inventory);
            appDbContext.SaveChanges();
        }
    }
}
