using Microsoft.EntityFrameworkCore;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.SqlServer;

public class CartRepository : ICartRepository
{
    private readonly MotorcycleShopDbContext _context;

    // Конструктор принимает DbContext через dependency injection
    public CartRepository(MotorcycleShopDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CartItem>> GetAllItemsAsync()
    {
        // ВАЖНО: CartItem имеет навигационное свойство Motorcycle
        // Используем Include для его загрузки:
        return await _context.CartItems
            .Include(ci => ci.Motorcycle)
            .ToListAsync();
    }

    public async Task<CartItem?> GetByIdAsync(int id)
    {
        // FindAsync() асинхронно ищет сущность по первичному ключу
        // Используем Include для загрузки связанной Motorcycle
        return await _context.CartItems
            .Include(ci => ci.Motorcycle)
            .FirstOrDefaultAsync(ci => ci.Id == id);
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
            // Update() помечает сущность как измененную, и при SaveChanges() будет сформирован SQL UPDATE
            _context.CartItems.Update(existingItem);
        }
        else
        {
            // Для корректной работы с внешними ключами, добавляем только ID мотоцикла
            // Сначала убедимся, что мотоцикл существует в БД
            var existingMotorcycle = await _context.Motorcycles.FindAsync(motorcycle.Id);
            if (existingMotorcycle == null)
                throw new ArgumentException($"Motorcycle with ID {motorcycle.Id} does not exist.");

            // Otherwise, add a new item to the cart
            var newItem = new CartItem
            {
                Motorcycle = existingMotorcycle, // Используем существующий объект из БД
                Quantity = quantity,
                UnitPrice = existingMotorcycle.Price
            };
            // Add() добавляет сущность в отслеживание EF Core
            _context.CartItems.Add(newItem);
            existingItem = newItem;
        }

        // SaveChangesAsync() асинхронно генерирует и выполняет SQL
        await _context.SaveChangesAsync();
        return existingItem;
    }

    public async Task<CartItem> UpdateItemAsync(CartItem cartItem)
    {
        // Сначала находим существующую сущность
        var existing = await _context.CartItems.FindAsync(cartItem.Id);
        if (existing == null)
            throw new InvalidOperationException($"CartItem with ID {cartItem.Id} not found");

        // Копируем все свойства (кроме Id) из cartItem в existing
        existing.CopyFrom(cartItem);
        // SaveChangesAsync() генерирует SQL UPDATE только для изменённых полей
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> RemoveItemAsync(int id)
    {
        var cartItem = await _context.CartItems.FindAsync(id);
        if (cartItem == null)
            return false;

        // Remove() помечает сущность для удаления
        _context.CartItems.Remove(cartItem);
        // SaveChangesAsync() асинхронно генерирует и выполняет SQL DELETE
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ClearCartAsync()
    {
        var cartItems = await _context.CartItems.ToListAsync();
        // RemoveRange() удаляет несколько сущностей
        _context.CartItems.RemoveRange(cartItems);
        // SaveChangesAsync() асинхронно генерирует и выполняет SQL DELETE для всех элементов
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<decimal> GetTotalAmountAsync()
    {
        // AsQueryable() создаёт LINQ-запрос (ещё не выполнен)
        var cartItems = await _context.CartItems.ToListAsync();
        // Вычисляем сумму на клиенте
        return cartItems.Sum(item => item.Price);
    }

    public async Task<int> GetItemCountAsync()
    {
        // AsQueryable() создаёт LINQ-запрос (ещё не выполнен)
        var cartItems = await _context.CartItems.ToListAsync();
        // Вычисляем сумму на клиенте
        return cartItems.Sum(item => item.Quantity);
    }
}