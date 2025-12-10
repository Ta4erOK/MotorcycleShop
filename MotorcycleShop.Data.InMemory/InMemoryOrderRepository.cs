using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.InMemory
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders;
        private int _nextId = 1;

        public InMemoryOrderRepository()
        {
            _orders = new List<Order>();
        }

        public Task<IEnumerable<Order>> GetAllAsync()
        {
            return Task.FromResult((IEnumerable<Order>)_orders.ToList());
        }

        public Task<Order?> GetByIdAsync(int id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            return Task.FromResult(order);
        }

        public Task<Order> CreateAsync(Order order)
        {
            order.Id = _nextId++;
            order.OrderDate = DateTime.Now;
            _orders.Add(order);
            return Task.FromResult(order);
        }

        public Task<Order> UpdateAsync(Order order)
        {
            var index = _orders.FindIndex(o => o.Id == order.Id);
            if (index != -1)
            {
                _orders[index] = order;
            }
            return Task.FromResult(order);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order != null)
            {
                _orders.Remove(order);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<IEnumerable<Order>> GetByCustomerEmailAsync(string email)
        {
            var orders = _orders.Where(o => o.Email.ToLower().Equals(email.ToLower())).ToList();
            return Task.FromResult((IEnumerable<Order>)orders);
        }
    }
}