using Market.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Market.Data.DbContexts;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Debt> Debts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductItem> ProductItems { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
        this.Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasQueryFilter(user => !user.IsDeleted);
        modelBuilder.Entity<Category>().HasQueryFilter(category => !category.IsDeleted);
        modelBuilder.Entity<Order>().HasQueryFilter(order => !order.IsDeleted);
        modelBuilder.Entity<Product>().HasQueryFilter(product => !product.IsDeleted);
        modelBuilder.Entity<ProductItem>().HasQueryFilter(productItem => !productItem.IsDeleted);
        modelBuilder.Entity<Debt>().HasQueryFilter(debt => !debt.IsDeleted);

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("5e8791a2-0ab2-436d-9b19-493336e24d87"),
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin",
                Password = "$2a$11$/BEZ4kXZ9kFaFdMQn6QB6OQ3ZzlVCRVtt.5vNUVorqe/LkM6lL07m",
                Role = Domain.Enums.Role.System
            });

    }
}
