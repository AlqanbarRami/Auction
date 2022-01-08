using System.Collections.Generic;

namespace Auktioner.Models
{
    public interface IInventoryRepository 
    {
        IEnumerable<Inventory> AllInventory { get; }
        void AddInventory(Inventory inventory);

        void EditInventory(Inventory inventory);
        void RemoveInventory(Inventory inventory);
        public string GetSpecialId();

        public decimal GetSpecialPrice(decimal decade);

        public decimal ChangeYearToDecade(int year);
    }


}
