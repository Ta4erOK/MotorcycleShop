using System.Collections.Generic;
using System.Threading.Tasks;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.Interfaces
{
    public interface IMotorcycleRepository
    {
        Task<IEnumerable<Motorcycle>> GetAllAsync();
        Task<Motorcycle?> GetByIdAsync(int id);
        Task<Motorcycle> AddAsync(Motorcycle motorcycle);
        Task<Motorcycle> UpdateAsync(Motorcycle motorcycle);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Motorcycle>> SearchAsync(string? brand = null, int? year = null,
            decimal? minPrice = null, decimal? maxPrice = null, string? searchTerm = null);
    }
}