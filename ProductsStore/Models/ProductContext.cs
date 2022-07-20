using Microsoft.EntityFrameworkCore;

namespace ProductsStore.Models
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserFeedback> UserFeedbacks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new Role {Id = 1, Name = "admin"});
            modelBuilder.Entity<Role>().HasData(new Role {Id = 2, Name = "user"});
            modelBuilder.Entity<User>().HasData(new User {Id = 1, UserName = "Admin", Email = "admin@admin.com", Password = "1234", RoleId = 1});
        }

    }
}