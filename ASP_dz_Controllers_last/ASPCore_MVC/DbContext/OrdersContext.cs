using ASPCore_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPCore_MVC.DbContext
{
    public class OrdersContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public OrdersContext(DbContextOptions<OrdersContext> options) : base(options) { }
        public DbSet<Order>? Orders { get; set; }
    }
}
