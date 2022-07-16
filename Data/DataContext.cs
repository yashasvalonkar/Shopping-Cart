using Microsoft.EntityFrameworkCore;

namespace ShoppingAPI.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Shopping> Shoppings { get; set; }
        public DbSet<Cart> Cart { get; set; }

        
    }
}
