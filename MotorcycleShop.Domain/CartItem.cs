using System;

namespace MotorcycleShop.Domain
{
    public class CartItem
    {
        public int Id { get; set; }
        public Motorcycle Motorcycle { get; set; } = new Motorcycle();
        public int Quantity { get; set; } = 1;
        public decimal Price => Motorcycle.Price * Quantity; // цена на момент добавления в корзину
    }
}