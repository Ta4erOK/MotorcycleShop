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

        public void CopyFrom(CartItem other)
        {
            // Копируем все свойства, КРОМЕ Id
            Motorcycle = other.Motorcycle;
            Quantity = other.Quantity;
            UnitPrice = other.UnitPrice;
        }
    }
}