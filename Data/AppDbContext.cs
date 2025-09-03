using APITest.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace APITest.Data
{
    public class AppDbContext : IdentityDbContext<AppUSER>
    {
        public AppDbContext( DbContextOptions<AppDbContext> options) : base(options)
        {
          
        }
        public DbSet<Models.Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payment { get; set; }

    }
}
