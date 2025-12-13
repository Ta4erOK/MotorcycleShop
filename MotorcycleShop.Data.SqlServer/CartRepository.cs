using Microsoft.EntityFrameworkCore;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.SqlServer
{
    public class CartRepository : ICartRepository
    {
        private readonly MotorcycleShopDbContext _context;

        public CartRepository(MotorcycleShopDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CartItem>> GetAllItemsAsync()
        {
            return await _context.CartItems.ToListAsync();
        }

        public async Task<CartItem?> GetByIdAsync(int id)
        {
            return await _context.CartItems.FindAsync(id);
        }

        public async Task<CartItem> AddItemAsync(Motorcycle motorcycle, int quantity = 1)
        {
            // Check if the motorcycle is already in the cart
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Motorcycle.Id == motorcycle.Id);

            if (existingItem != null)
            {
                // If item exists in cart, increase the quantity
                existingItem.Quantity += quantity;
                _context.CartItems.Update(existingItem);
            }
            else
            {
                // Otherwise, add a new item to the cart
                var newItem = new CartItem
                {
                    Motorcycle = motorcycle,
                    Quantity = quantity,
                    UnitPrice = motorcycle.Price
                };
                _context.CartItems.Add(newItem);
                existingItem = newItem;
            }

            await _context.SaveChangesAsync();
            return existingItem;
        }

        public async Task<CartItem> UpdateItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<bool> RemoveItemAsync(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null) return false;

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCartAsync()
        {
            var cartItems = await _context.CartItems.ToListAsync();
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetTotalAmountAsync()
        {
            var cartItems = await _context.CartItems.ToListAsync();
            return cartItems.Sum(item => item.Price);
        }

        public async Task<int> GetItemCountAsync()
        {
            var cartItems = await _context.CartItems.ToListAsync();
            return cartItems.Sum(item => item.Quantity);
        }
    }
}