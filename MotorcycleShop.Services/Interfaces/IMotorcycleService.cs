using MotorcycleShop.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotorcycleShop.Services.Interfaces
{
    public interface IMotorcycleService
    {
        Task<IEnumerable<Motorcycle>> GetAllMotorcyclesAsync();
        Task<Motorcycle?> GetMotorcycleByIdAsync(int id);
        Task<Motorcycle> CreateMotorcycleAsync(Motorcycle motorcycle);
        Task<Motorcycle> UpdateMotorcycleAsync(Motorcycle motorcycle);
        Task<bool> DeleteMotorcycleAsync(int id);
        Task<IEnumerable<Motorcycle>> SearchMotorcyclesAsync(string? brand = null, int? year = null,
            decimal? minPrice = null, decimal? maxPrice = null, string? searchTerm = null);
    }
}