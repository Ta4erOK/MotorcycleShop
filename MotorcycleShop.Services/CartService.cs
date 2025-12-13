using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;
using MotorcycleShop.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotorcycleShop.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMotorcycleRepository _motorcycleRepository;

        public CartService(ICartRepository cartRepository, IMotorcycleRepository motorcycleRepository)
        {
            _cartRepository = cartRepository;
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task<IEnumerable<CartItem>> GetAllCartItemsAsync()
        {
            return await _cartRepository.GetAllItemsAsync();
        }

        public async Task<CartItem?> GetCartItemByIdAsync(int id)
        {
            return await _cartRepository.GetByIdAsync(id);
        }

        public async Task<CartItem> AddItemToCartAsync(int motorcycleId, int quantity = 1)
        {
            var motorcycle = await _motorcycleRepository.GetByIdAsync(motorcycleId);
            if (motorcycle == null)
                throw new ArgumentException($"Motorcycle with ID {motorcycleId} not found");

            return await _cartRepository.AddItemAsync(motorcycle, quantity);
        }

        public async Task<CartItem> UpdateCartItemAsync(int cartItemId, int quantity)
        {
            if (quantity <= 0)
                return await RemoveItemFromCartAsync(cartItemId) ? null : await _cartRepository.GetByIdAsync(cartItemId);

            var cartItem = await _cartRepository.GetByIdAsync(cartItemId);
            if (cartItem == null)
                throw new ArgumentException($"Cart item with ID {cartItemId} not found");

            cartItem.Quantity = quantity;
            return await _cartRepository.UpdateItemAsync(cartItem);
        }

        public async Task<bool> RemoveItemFromCartAsync(int id)
        {
            return await _cartRepository.RemoveItemAsync(id);
        }

        public async Task<bool> ClearCartAsync()
        {
            return await _cartRepository.ClearCartAsync();
        }

        public async Task<decimal> GetCartTotalAmountAsync()
        {
            return await _cartRepository.GetTotalAmountAsync();
        }

        public async Task<int> GetCartItemsCountAsync()
        {
            return await _cartRepository.GetItemCountAsync();
        }
    }
}