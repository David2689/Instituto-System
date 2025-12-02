using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WebApplication1.Models
{
    // Esta clase le dice a EF cómo crear el AppDbContext cuando haces Add-Migration / Update-Database
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // MISMA cadena de conexión que pusimos en appsettings.json
            optionsBuilder.UseSqlite("Data Source=instituto.db");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
