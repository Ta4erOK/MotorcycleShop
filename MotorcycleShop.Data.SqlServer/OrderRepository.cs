using Microsoft.EntityFrameworkCore;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.SqlServer;

public class OrderRepository : IOrderRepository
{
    private readonly MotorcycleShopDbContext _context;

    // Конструктор принимает DbContext через dependency injection
    public OrderRepository(MotorcycleShopDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        // ВАЖНО: Order имеет навигационное свойство Items (OrderItem[])
        // Используем Include для их загрузки:
        return await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Motorcycle) // Также загружаем мотоциклы для каждого OrderItem
            .ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        // FindAsync() асинхронно ищет сущность по первичному ключу
        // Используем Include для загрузки связанных Items и Motorcycle для каждого OrderItem
        return await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Motorcycle)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order> CreateAsync(Order order)
    {
        // Add() добавляет сущность в отслеживание EF Core
        _context.Orders.Add(order);
        // SaveChangesAsync() асинхронно генерирует и выполняет SQL INSERT
        // EF Core автоматически присваивает Id после сохранения
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> UpdateAsync(Order order)
    {
        // Сначала находим существующую сущность
        var existing = await _context.Orders.FindAsync(order.Id);
        if (existing == null)
            throw new InvalidOperationException($"Order with ID {order.Id} not found");

        // Копируем все свойства (кроме Id) из order в existing
        existing.CopyFrom(order);
        // SaveChangesAsync() генерирует SQL UPDATE только для изменённых полей
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return false;

        // Remove() помечает сущность для удаления
        _context.Orders.Remove(order);
        // SaveChangesAsync() асинхронно генерирует и выполняет SQL DELETE
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Order>> GetByCustomerEmailAsync(string email)
    {
        // AsQueryable() создаёт LINQ-запрос (ещё не выполнен)
        var query = _context.Orders
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Motorcycle)
            .AsQueryable();

        // Добавляем фильтр — формируется SQL WHERE
        query = query.Where(o => o.Email.ToLower().Equals(email.ToLower()));

        // ToListAsync() асинхронно выполняет запрос к БД и возвращает результат
        return await query.ToListAsync();
    }
}