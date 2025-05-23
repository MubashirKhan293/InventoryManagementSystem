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
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Product entity
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Product>()
                .Property(p => p.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<Product>()
                .Property(p => p.UnitPrice)
                .HasColumnType("decimal(18,2)");

            // Configure Sale entity
            modelBuilder.Entity<Sale>()
                .Property(s => s.CustomerName)
                .HasMaxLength(100);

            modelBuilder.Entity<Sale>()
                .Property(s => s.TotalAmount)
                .HasColumnType("decimal(18,2)");

            // Configure SaleItem entity
            modelBuilder.Entity<SaleItem>()
                .Property(si => si.UnitPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<SaleItem>()
                .Property(si => si.TotalPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<SaleItem>()
                .HasOne(si => si.Sale)
                .WithMany(s => s.Items)
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SaleItem>()
                .HasOne(si => si.Product)
                .WithMany()
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Purchase entity
            modelBuilder.Entity<Purchase>()
                .Property(p => p.SupplierName)
                .HasMaxLength(100);

            modelBuilder.Entity<Purchase>()
                .Property(p => p.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Purchase>()
                .Property(p => p.Status)
                .HasMaxLength(20);

            // Configure PurchaseItem entity
            modelBuilder.Entity<PurchaseItem>()
                .Property(pi => pi.UnitPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<PurchaseItem>()
                .Property(pi => pi.TotalPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<PurchaseItem>()
                .HasOne(pi => pi.Purchase)
                .WithMany(p => p.Items)
                .HasForeignKey(pi => pi.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseItem>()
                .HasOne(pi => pi.Product)
                .WithMany()
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
