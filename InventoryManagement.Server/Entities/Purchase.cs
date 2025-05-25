using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Server.Entities
{
    public class Purchase
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int QuantityPurchased { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        public decimal TotalAmount => QuantityPurchased * UnitPrice;

        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        public string SupplierName { get; set; } = string.Empty;

        // Navigation property
        public virtual Product Product { get; set; } = null!;
    }
}
