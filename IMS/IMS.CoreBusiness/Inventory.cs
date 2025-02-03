namespace IMS.CoreBusiness
{
    public class Inventory
    {
        public int InventorId { get; set; }
        public string InventoryName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
