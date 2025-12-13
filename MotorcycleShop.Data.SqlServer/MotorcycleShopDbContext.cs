using Microsoft.EntityFrameworkCore;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.SqlServer
{
    public class MotorcycleShopDbContext : DbContext
    {
        public MotorcycleShopDbContext(DbContextOptions<MotorcycleShopDbContext> options) : base(options)
        {
        }

        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Motorcycle>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Brand).IsRequired().HasMaxLength(100);
                entity.Property(m => m.Model).IsRequired().HasMaxLength(100);
                entity.Property(m => m.EngineCapacity).HasColumnType("decimal(4,2)");
                entity.Property(m => m.Price).HasColumnType("decimal(18,2)");
                entity.Property(m => m.Description).HasMaxLength(1000);
                entity.Property(m => m.ImagePath).HasMaxLength(500);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.CustomerName).IsRequired().HasMaxLength(100);
                entity.Property(o => o.Email).IsRequired().HasMaxLength(100);
                entity.Property(o => o.Phone).HasMaxLength(20);
                entity.Property(o => o.Address).HasMaxLength(500);
                entity.Property(o => o.Comment).HasMaxLength(1000);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);
                entity.Property(oi => oi.Quantity).HasDefaultValue(1);
                entity.Property(oi => oi.Price).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(ci => ci.Id);
                entity.Property(ci => ci.Quantity).HasDefaultValue(1);
                entity.Property(ci => ci.UnitPrice).HasColumnType("decimal(18,2)");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}