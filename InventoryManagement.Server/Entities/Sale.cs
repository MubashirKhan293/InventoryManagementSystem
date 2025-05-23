namespace InventoryManagement.Server.Entities
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime SaleDate { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<SaleItem> Items { get; set; }
    }
}
