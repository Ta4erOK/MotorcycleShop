using Microsoft.EntityFrameworkCore;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.SqlServer
{
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly MotorcycleShopDbContext _context;

        public MotorcycleRepository(MotorcycleShopDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Motorcycle>> GetAllAsync()
        {
            return await _context.Motorcycles.ToListAsync();
        }

        public async Task<Motorcycle?> GetByIdAsync(int id)
        {
            return await _context.Motorcycles.FindAsync(id);
        }

        public async Task<Motorcycle> AddAsync(Motorcycle motorcycle)
        {
            _context.Motorcycles.Add(motorcycle);
            await _context.SaveChangesAsync();
            return motorcycle;
        }

        public async Task<Motorcycle> UpdateAsync(Motorcycle motorcycle)
        {
            _context.Motorcycles.Update(motorcycle);
            await _context.SaveChangesAsync();
            return motorcycle;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var motorcycle = await _context.Motorcycles.FindAsync(id);
            if (motorcycle == null) return false;

            _context.Motorcycles.Remove(motorcycle);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Motorcycle>> SearchAsync(string? brand = null, int? year = null, decimal? minPrice = null, decimal? maxPrice = null, string? searchTerm = null)
        {
            var query = _context.Motorcycles.AsQueryable();

            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(m => m.Brand.ToLower().Contains(brand.ToLower()));
            }

            if (year.HasValue)
            {
                query = query.Where(m => m.Year == year.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(m => m.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(m => m.Price <= maxPrice.Value);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(m => 
                    m.Brand.ToLower().Contains(searchTerm.ToLower()) ||
                    m.Model.ToLower().Contains(searchTerm.ToLower()) ||
                    m.Description.ToLower().Contains(searchTerm.ToLower())
                );
            }

            return await query.ToListAsync();
        }
    }
}