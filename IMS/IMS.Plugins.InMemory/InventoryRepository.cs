using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class InventoryRepository : IInventoryRepository
    {
        private List<Inventory> _inventories;

        public InventoryRepository()
        {
            _inventories = new List<Inventory>()
            {
                new Inventory { InventorId = 1, InventoryName = "Bike Seat", Quantity = 10, Price = 2 },
                new Inventory { InventorId = 2, InventoryName = "Bike Body", Quantity = 10, Price = 15 },
                new Inventory { InventorId = 3, InventoryName = "Bike Wheels", Quantity = 20, Price = 8 },
                new Inventory { InventorId = 4, InventoryName = "Bike Pedels", Quantity = 20, Price = 1 },
            };
        }

        public Task AddInventoryAsync(Inventory inventory)
        {
            if (_inventories.Any(i => i.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;
            }

            var maxId = _inventories.Max(i => i.InventorId);
            inventory.InventorId = maxId + 1;

            _inventories.Add(inventory);
            return Task.CompletedTask;
        }

        public async Task<Inventory> GetInventoriesByIdAsync(int iventoryId)
        {
            return await Task.FromResult(_inventories.First(i => i.InventorId == iventoryId));
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await Task.FromResult(_inventories);
            }

            return _inventories.Where(i => i.InventoryName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public Task UpdateInventoryAsync(Inventory inventory)
        {
            if (_inventories.Any(i => i.InventorId == inventory.InventorId && 
                i.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;
            }

            var invToUpdate = _inventories.FirstOrDefault(i => i.InventorId == inventory.InventorId);
            if (invToUpdate is not null)
            {
                invToUpdate.InventoryName = inventory.InventoryName;
                invToUpdate.Quantity = inventory.Quantity;
                invToUpdate.Price = inventory.Price;
            }

            return Task.CompletedTask;
        }
    }
}
