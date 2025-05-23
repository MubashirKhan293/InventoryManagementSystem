namespace InventoryManagement.Server.DTOs
{
    public class SaleDto
    {
        public int Id { get; set; }
        public DateTime SaleDate { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Notes { get; set; }
        public List<SaleItemDto> Items { get; set; }
    }

    public class CreateSaleDto
    {
        public string CustomerName { get; set; }
        public string Notes { get; set; }
        public List<CreateSaleItemDto> Items { get; set; } = new List<CreateSaleItemDto>();
    }

    public class SaleItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class CreateSaleItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
