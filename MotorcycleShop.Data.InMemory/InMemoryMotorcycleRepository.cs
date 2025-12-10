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
            // Инициализация расширенными тестовыми данными
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
                },
                new Motorcycle
                {
                    Id = _nextId++,
                    Brand = "Ducati",
                    Model = "Scrambler",
                    Year = 2023,
                    EngineCapacity = 803,
                    Mileage = 3500,
                    Color = "Yellow",
                    Price = 11000,
                    Description = "Стильный и надежный мотоцикл для путешествий и города.",
                    ImagePath = ""
                },
                new Motorcycle
                {
                    Id = _nextId++,
                    Brand = "BMW",
                    Model = "G 310 R",
                    Year = 2022,
                    EngineCapacity = 313,
                    Mileage = 6000,
                    Color = "White",
                    Price = 5500,
                    Description = "Легкий и маневренный мотоцикл для начинающих.",
                    ImagePath = ""
                },
                new Motorcycle
                {
                    Id = _nextId++,
                    Brand = "Suzuki",
                    Model = "GSX-8R",
                    Year = 2023,
                    EngineCapacity = 776,
                    Mileage = 1500,
                    Color = "Red/White",
                    Price = 9500,
                    Description = "Современный спортбайк с отличной эргономикой.",
                    ImagePath = ""
                },
                new Motorcycle
                {
                    Id = _nextId++,
                    Brand = "KTM",
                    Model = "790 Duke",
                    Year = 2022,
                    EngineCapacity = 790,
                    Mileage = 4000,
                    Color = "Orange",
                    Price = 10200,
                    Description = "Мощный и динамичный мотоцикл для агрессивной езды.",
                    ImagePath = ""
                },
                new Motorcycle
                {
                    Id = _nextId++,
                    Brand = "Triumph",
                    Model = "Street Triple R",
                    Year = 2021,
                    EngineCapacity = 765,
                    Mileage = 7000,
                    Color = "Firestorm Red",
                    Price = 12000,
                    Description = "Высокая производительность и комфорт в одном мотоцикле.",
                    ImagePath = ""
                },
                new Motorcycle
                {
                    Id = _nextId++,
                    Brand = "Aprilia",
                    Model = "RS 660",
                    Year = 2023,
                    EngineCapacity = 659,
                    Mileage = 2500,
                    Color = "Naked Black",
                    Price = 14000,
                    Description = "Спортбайк с отличной управляемостью и мощностью.",
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

            // Фильтрация по бренду - только если бренд не пустой
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(m => m.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase));
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

            // Поиск по тексту - если он указан
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(m =>
                    m.Brand.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    m.Model.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    m.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            return Task.FromResult((IEnumerable<Motorcycle>)query.ToList());
        }
    }
}