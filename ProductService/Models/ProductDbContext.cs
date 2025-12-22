using Microsoft.EntityFrameworkCore;

namespace ProductService.Models
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use separate schema for this microservice
            modelBuilder.HasDefaultSchema("product");

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(p => p.ProductId);

                // ✅ Explicit precision for currency (fixes the warning)
                entity.Property(p => p.Price).HasPrecision(18, 2);

                // (Optional) helpful indexes and defaults
                entity.HasIndex(p => p.ProductName);

                entity.HasIndex(p => p.Category);
                entity.Property(p => p.IsAvailable).HasDefaultValue(true);
                entity.Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(p => p.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            base.OnModelCreating(modelBuilder);

        }
    }
}