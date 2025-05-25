using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Server.Entities
{
    public class Sale
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int QuantitySold { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        public decimal TotalAmount => QuantitySold * UnitPrice;

        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        public string CustomerName { get; set; } = string.Empty;

        // Navigation property
        public virtual Product Product { get; set; } = null!;
    }
}
