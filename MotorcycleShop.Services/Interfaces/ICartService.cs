using MotorcycleShop.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotorcycleShop.Services.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<CartItem>> GetAllCartItemsAsync();
        Task<CartItem?> GetCartItemByIdAsync(int id);
        Task<CartItem> AddItemToCartAsync(int motorcycleId, int quantity = 1);
        Task<CartItem> UpdateCartItemAsync(int cartItemId, int quantity);
        Task<bool> RemoveItemFromCartAsync(int id);
        Task<bool> ClearCartAsync();
        Task<decimal> GetCartTotalAmountAsync();
        Task<int> GetCartItemsCountAsync();
    }
}