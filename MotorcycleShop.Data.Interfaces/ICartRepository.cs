using System.Collections.Generic;
using System.Threading.Tasks;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.Interfaces
{
    public interface ICartRepository
    {
        Task<IEnumerable<CartItem>> GetAllItemsAsync();
        Task<CartItem?> GetByIdAsync(int id);
        Task<CartItem> AddItemAsync(Motorcycle motorcycle, int quantity = 1);
        Task<CartItem> UpdateItemAsync(CartItem cartItem);
        Task<bool> RemoveItemAsync(int id);
        Task<bool> ClearCartAsync();
        Task<decimal> GetTotalAmountAsync();
        Task<int> GetItemCountAsync();
    }
}