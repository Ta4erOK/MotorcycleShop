using System;

namespace MotorcycleShop.Domain
{
    public class Motorcycle
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal EngineCapacity { get; set; } // объем двигателя в литрах
        public int Mileage { get; set; } // пробег в км
        public string Color { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty; // путь к изображению
    }
}