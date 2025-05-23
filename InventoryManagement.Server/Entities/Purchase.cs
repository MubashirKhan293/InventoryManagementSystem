namespace InventoryManagement.Server.Entities
{
    public class Purchase
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string SupplierName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public ICollection<PurchaseItem> Items { get; set; }
    
    }
}
