using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Data.SqlServer;

namespace MotorcycleShop.Data.SqlServer
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddSqlServerDataServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MotorcycleShopDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICartRepository, CartRepository>();

            return services;
        }
    }
}