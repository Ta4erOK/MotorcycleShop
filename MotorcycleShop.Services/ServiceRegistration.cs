using Microsoft.Extensions.DependencyInjection;
using MotorcycleShop.Services;
using MotorcycleShop.Services.Interfaces;

namespace MotorcycleShop.Services
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IMotorcycleService, MotorcycleService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICartService, CartService>();

            return services;
        }
    }
}