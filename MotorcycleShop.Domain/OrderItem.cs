using System;

namespace MotorcycleShop.Domain
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Motorcycle Motorcycle { get; set; } = new Motorcycle();
        public int Quantity { get; set; } = 1;
        public decimal Price { get; set; } // цена на момент заказа

        public void CopyFrom(OrderItem other)
        {
            // Копируем все свойства, КРОМЕ Id
            Motorcycle = other.Motorcycle;
            Quantity = other.Quantity;
            Price = other.Price;
        }
    }
}