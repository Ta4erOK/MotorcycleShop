using System;

namespace MotorcycleShop.Domain
{
    public class CartItem
    {
        public int Id { get; set; }
        public Motorcycle Motorcycle { get; set; } = new Motorcycle();
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; } // цена за единицу на момент добавления в корзину
        public decimal Price => UnitPrice * Quantity; // общая цена для всех единиц
    }
}