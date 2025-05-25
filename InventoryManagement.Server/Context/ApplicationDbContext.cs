using InventoryManagement.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Server.Context
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
             : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            });

            // Sale configuration
            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.SaleDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.CustomerName).HasMaxLength(200);

                entity.HasOne(e => e.Product)
                      .WithMany(p => p.Sales)
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Purchase configuration
            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PurchaseDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.SupplierName).HasMaxLength(200);

                entity.HasOne(e => e.Product)
                      .WithMany(p => p.Purchases)
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
