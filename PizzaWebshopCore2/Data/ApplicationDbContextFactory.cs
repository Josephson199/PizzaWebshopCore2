using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PizzaWebshopCore2.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Pizzeria;Trusted_Connection=True;");
            var db = new ApplicationDbContext(builder.Options);
            return db;
        }
    }
}
