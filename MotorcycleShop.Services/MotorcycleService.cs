using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;
using MotorcycleShop.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotorcycleShop.Services
{
    public class MotorcycleService : IMotorcycleService
    {
        private readonly IMotorcycleRepository _motorcycleRepository;

        public MotorcycleService(IMotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task<IEnumerable<Motorcycle>> GetAllMotorcyclesAsync()
        {
            return await _motorcycleRepository.GetAllAsync();
        }

        public async Task<Motorcycle?> GetMotorcycleByIdAsync(int id)
        {
            return await _motorcycleRepository.GetByIdAsync(id);
        }

        public async Task<Motorcycle> CreateMotorcycleAsync(Motorcycle motorcycle)
        {
            return await _motorcycleRepository.AddAsync(motorcycle);
        }

        public async Task<Motorcycle> UpdateMotorcycleAsync(Motorcycle motorcycle)
        {
            return await _motorcycleRepository.UpdateAsync(motorcycle);
        }

        public async Task<bool> DeleteMotorcycleAsync(int id)
        {
            return await _motorcycleRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Motorcycle>> SearchMotorcyclesAsync(string? brand = null, int? year = null,
            decimal? minPrice = null, decimal? maxPrice = null, string? searchTerm = null)
        {
            return await _motorcycleRepository.SearchAsync(brand, year, minPrice, maxPrice, searchTerm);
        }
    }
}