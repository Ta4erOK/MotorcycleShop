using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.InMemory
{
    public class InMemoryCartRepository : ICartRepository
    {
        private readonly List<CartItem> _cartItems;
        private int _nextId = 1;

        public InMemoryCartRepository()
        {
            _cartItems = new List<CartItem>();
        }

        public Task<IEnumerable<CartItem>> GetAllItemsAsync()
        {
            return Task.FromResult((IEnumerable<CartItem>)_cartItems.ToList());
        }

        public Task<CartItem?> GetByIdAsync(int id)
        {
            var cartItem = _cartItems.FirstOrDefault(item => item.Id == id);
            return Task.FromResult(cartItem);
        }

        public Task<CartItem> AddItemAsync(Motorcycle motorcycle, int quantity = 1)
        {
            // Проверяем, есть ли уже этот мотоцикл в корзине
            var existingItem = _cartItems.FirstOrDefault(item => item.Motorcycle.Id == motorcycle.Id);
            
            if (existingItem != null)
            {
                // Если мотоцикл уже в корзине, увеличиваем количество
                existingItem.Quantity += quantity;
                return Task.FromResult(existingItem);
            }
            else
            {
                // Иначе добавляем новый элемент
                var cartItem = new CartItem
                {
                    Id = _nextId++,
                    Motorcycle = motorcycle,
                    Quantity = quantity
                };
                _cartItems.Add(cartItem);
                return Task.FromResult(cartItem);
            }
        }

        public Task<CartItem> UpdateItemAsync(CartItem cartItem)
        {
            var index = _cartItems.FindIndex(item => item.Id == cartItem.Id);
            if (index != -1)
            {
                _cartItems[index] = cartItem;
            }
            return Task.FromResult(cartItem);
        }

        public Task<bool> RemoveItemAsync(int id)
        {
            var cartItem = _cartItems.FirstOrDefault(item => item.Id == id);
            if (cartItem != null)
            {
                _cartItems.Remove(cartItem);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> ClearCartAsync()
        {
            _cartItems.Clear();
            return Task.FromResult(true);
        }

        public Task<decimal> GetTotalAmountAsync()
        {
            var total = _cartItems.Sum(item => item.Price);
            return Task.FromResult(total);
        }

        public Task<int> GetItemCountAsync()
        {
            var count = _cartItems.Sum(item => item.Quantity);
            return Task.FromResult(count);
        }
    }
}