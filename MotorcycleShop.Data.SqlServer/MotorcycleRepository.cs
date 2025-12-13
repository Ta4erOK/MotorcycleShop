using Microsoft.EntityFrameworkCore;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.SqlServer;

public class MotorcycleRepository : IMotorcycleRepository
{
    private readonly MotorcycleShopDbContext _context;

    // Конструктор принимает DbContext через dependency injection
    public MotorcycleRepository(MotorcycleShopDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Motorcycle>> GetAllAsync()
    {
        // ToListAsync() асинхронно выполняет запрос к БД и возвращает результат
        return await _context.Motorcycles.ToListAsync();
    }

    public async Task<Motorcycle?> GetByIdAsync(int id)
    {
        // FindAsync() асинхронно ищет сущность по первичному ключу
        // Сначала проверяет кэш, затем выполняет SELECT
        // Возвращает null, если не найдена
        return await _context.Motorcycles.FindAsync(id);
    }

    public async Task<Motorcycle> AddAsync(Motorcycle motorcycle)
    {
        // Add() добавляет сущность в отслеживание EF Core
        _context.Motorcycles.Add(motorcycle);
        // SaveChangesAsync() асинхронно генерирует и выполняет SQL INSERT
        // EF Core автоматически присваивает Id после сохранения
        await _context.SaveChangesAsync();
        return motorcycle;
    }

    public async Task<Motorcycle> UpdateAsync(Motorcycle motorcycle)
    {
        // Сначала находим существующую сущность
        var existing = await _context.Motorcycles.FindAsync(motorcycle.Id);
        if (existing == null)
            throw new InvalidOperationException($"Motorcycle with ID {motorcycle.Id} not found");

        // Копируем все свойства (кроме Id) из motorcycle в existing
        existing.CopyFrom(motorcycle);
        // SaveChangesAsync() генерирует SQL UPDATE только для изменённых полей
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var motorcycle = await _context.Motorcycles.FindAsync(id);
        if (motorcycle == null)
            return false;

        // Remove() помечает сущность для удаления
        _context.Motorcycles.Remove(motorcycle);
        // SaveChangesAsync() асинхронно генерирует и выполняет SQL DELETE
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Motorcycle>> SearchAsync(string? brand = null, int? year = null,
        decimal? minPrice = null, decimal? maxPrice = null, string? searchTerm = null)
    {
        // AsQueryable() создаёт LINQ-запрос (ещё не выполнен)
        var query = _context.Motorcycles.AsQueryable();

        // Добавляем фильтры — формируется SQL WHERE
        if (!string.IsNullOrEmpty(brand))
            query = query.Where(x => x.Brand.Contains(brand, StringComparison.OrdinalIgnoreCase));

        if (year.HasValue)
            query = query.Where(x => x.Year == year.Value);

        if (minPrice.HasValue)
            query = query.Where(x => x.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(x => x.Price <= maxPrice.Value);

        if (!string.IsNullOrEmpty(searchTerm))
            query = query.Where(x => x.Brand.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                     x.Model.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                     x.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

        // ToListAsync() асинхронно выполняет запрос к БД и возвращает результат
        return await query.ToListAsync();
    }
}