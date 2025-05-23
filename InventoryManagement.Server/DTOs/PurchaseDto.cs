namespace InventoryManagement.Server.DTOs
{
    public class PurchaseDto
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string SupplierName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public List<PurchaseItemDto> Items { get; set; }
    }

    public class CreatePurchaseDto
    {
        public string SupplierName { get; set; }
        public string Notes { get; set; }
        public List<CreatePurchaseItemDto> Items { get; set; }
    }

    public class UpdatePurchaseStatusDto
    {
        public string Status { get; set; }
    }

    public class PurchaseItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class CreatePurchaseItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
