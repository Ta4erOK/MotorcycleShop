using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MotorcycleShop.Data.SqlServer;
public class MotorcycleShopDbContextFactory : IDesignTimeDbContextFactory<MotorcycleShopDbContext>
{
    public MotorcycleShopDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.database.json")
        .Build();

        return CreateDbContext(configuration);
    }
    public MotorcycleShopDbContext CreateDbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var optionsBuilder = new DbContextOptionsBuilder<MotorcycleShopDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new MotorcycleShopDbContext(optionsBuilder.Options);
    }
}
