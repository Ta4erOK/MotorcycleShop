using System;
using System.Collections.Generic;
using System.Linq;

namespace MotorcycleShop.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal TotalAmount => Items.Sum(item => item.Price * item.Quantity);

        public void CopyFrom(Order other)
        {
            // Копируем все свойства, КРОМЕ Id
            CustomerName = other.CustomerName;
            Email = other.Email;
            Phone = other.Phone;
            Address = other.Address;
            Comment = other.Comment;
            OrderDate = other.OrderDate;
            Status = other.Status;
            Items = other.Items; // Примечание: копируем ссылку на список, а не сам список
        }
    }

    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
}