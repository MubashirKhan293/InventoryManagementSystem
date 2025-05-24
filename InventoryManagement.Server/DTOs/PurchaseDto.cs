namespace InventoryManagement.Server.DTOs
{
    public class PurchaseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int QuantityPurchased { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string SupplierName { get; set; } = string.Empty;
    }

    public class CreatePurchaseDto
    {
        public int ProductId { get; set; }
        public int QuantityPurchased { get; set; }
        public decimal UnitPrice { get; set; }
        public string SupplierName { get; set; } = string.Empty;
    }
}
