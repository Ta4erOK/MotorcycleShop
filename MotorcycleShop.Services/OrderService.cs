using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;
using MotorcycleShop.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotorcycleShop.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            return await _orderRepository.CreateAsync(order);
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            return await _orderRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerEmailAsync(string email)
        {
            return await _orderRepository.GetByCustomerEmailAsync(email);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return false;

            order.Status = newStatus;
            await _orderRepository.UpdateAsync(order);
            return true;
        }
    }
}