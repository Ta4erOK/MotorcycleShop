using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.InMemory
{
    public class InMemoryMotorcycleRepository : IMotorcycleRepository
    {
        private readonly List<Motorcycle> _motorcycles;
        private int _nextId = 1;

        public InMemoryMotorcycleRepository()
        {
            // Инициализация тестовыми данными
            _motorcycles = new List<Motorcycle>
            {
                new Motorcycle
                {
                    Id = _nextId++,
                    Brand = "Harley-Davidson",
                    Model = "Street 750",
                    Year = 2022,
                    EngineCapacity = 749,
                    Mileage = 5000,
                    Color = "Black",
                    Price = 8500,
                    Description = "Надежный и стильный мотоцикл для города и за его пределами.",
                    ImagePath = ""
                },
                new Motorcycle
                {
                    Id = _nextId++,
                    Brand = "Yamaha",
                    Model = "MT-07",
                    Year = 2023,
                    EngineCapacity = 689,
                    Mileage = 2000,
                    Color = "Blue",
                    Price = 7800,
                    Description = "Отличная маневренность и комфорт в городе.",
                    ImagePath = ""
                },
                new Motorcycle
                {
                    Id = _nextId++,
                    Brand = "Honda",
                    Model = "CB650R",
                    Year = 2021,
                    EngineCapacity = 649,
                    Mileage = 8000,
                    Color = "Red",
                    Price = 9200,
                    Description = "Классический naked bike с отличной эргономикой.",
                    ImagePath = ""
                },
                new Motorcycle
                {
                    Id = _nextId++,
                    Brand = "Kawasaki",
                    Model = "Ninja 650",
                    Year = 2020,
                    EngineCapacity = 649,
                    Mileage = 12000,
                    Color = "Green",
                    Price = 8700,
                    Description = "Идеальный выбор для начинающих и опытных райдеров.",
                    ImagePath = ""
                }
            };
        }

        public Task<IEnumerable<Motorcycle>> GetAllAsync()
        {
            return Task.FromResult((IEnumerable<Motorcycle>)_motorcycles.ToList());
        }

        public Task<Motorcycle?> GetByIdAsync(int id)
        {
            var motorcycle = _motorcycles.FirstOrDefault(m => m.Id == id);
            return Task.FromResult(motorcycle);
        }

        public Task<Motorcycle> AddAsync(Motorcycle motorcycle)
        {
            motorcycle.Id = _nextId++;
            _motorcycles.Add(motorcycle);
            return Task.FromResult(motorcycle);
        }

        public Task<Motorcycle> UpdateAsync(Motorcycle motorcycle)
        {
            var index = _motorcycles.FindIndex(m => m.Id == motorcycle.Id);
            if (index != -1)
            {
                _motorcycles[index] = motorcycle;
            }
            return Task.FromResult(motorcycle);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var motorcycle = _motorcycles.FirstOrDefault(m => m.Id == id);
            if (motorcycle != null)
            {
                _motorcycles.Remove(motorcycle);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<IEnumerable<Motorcycle>> SearchAsync(string? brand = null, int? year = null, 
            decimal? minPrice = null, decimal? maxPrice = null, string? searchTerm = null)
        {
            var query = _motorcycles.AsQueryable();

            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(m => m.Brand.ToLower().Contains(brand.ToLower()));
            }

            if (year.HasValue)
            {
                query = query.Where(m => m.Year == year);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(m => m.Price >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(m => m.Price <= maxPrice);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(m => 
                    m.Brand.ToLower().Contains(searchTerm.ToLower()) || 
                    m.Model.ToLower().Contains(searchTerm.ToLower()) || 
                    m.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            return Task.FromResult((IEnumerable<Motorcycle>)query.ToList());
        }
    }
}