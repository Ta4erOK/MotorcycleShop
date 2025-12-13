using MotorcycleShop.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotorcycleShop.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByCustomerEmailAsync(string email);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);
    }
}