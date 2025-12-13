using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Windows;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Data.SqlServer;
using System.IO;

namespace MotorcycleShop.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IMotorcycleRepository _motorcycleRepository = null!;
    private IOrderRepository _orderRepository = null!;
    private ICartRepository _cartRepository = null!;
    private MotorcycleShopDbContext _dbContext = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // 1. Чтение конфигурации из файла
        var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.database.json")
        .Build();

        // 2. Создание DbContext через фабрику
        var factory = new MotorcycleShopDbContextFactory();
        _dbContext = factory.CreateDbContext(configuration);

        // 3. ВАЖНО: Применение миграций автоматически при запуске
        _dbContext.Database.Migrate();

        // 4. Создание репозиториев на основе DbContext
        _motorcycleRepository = new MotorcycleRepository(_dbContext);
        _orderRepository = new OrderRepository(_dbContext);
        _cartRepository = new CartRepository(_dbContext);

        // 5. Заполнение тестовыми данными (только если БД пустая)
        SeedInitData();

        // 6. Запуск главного окна
        var mainWindow = new MainWindow(_motorcycleRepository, _orderRepository, _cartRepository);
        mainWindow.Show();
    }

    private void SeedInitData()
    {
        // Проверяем, есть ли уже данные в БД
        if (_motorcycleRepository.GetAllAsync().Result.Any())
        {
            // Данные уже есть, пропускаем заполнение
            return;
        }

        // Добавляем тестовые мотоциклы
        var testMotorcycles = new List<Domain.Motorcycle>
        {
            new Domain.Motorcycle
            {
                Brand = "Harley-Davidson",
                Model = "Street 750",
                Year = 2022,
                EngineCapacity = 749,
                Mileage = 5000,
                Color = "Black",
                Price = 8999,
                Description = "Классический американский мотоцикл с низкой посадкой и отличной маневренностью.",
                ImagePath = "/Images/harley davidson street 750.jpg"
            },
            new Domain.Motorcycle
            {
                Brand = "BMW",
                Model = "G 310 R",
                Year = 2023,
                EngineCapacity = 313,
                Mileage = 3000,
                Color = "White",
                Price = 5499,
                Description = "Надежный и экономичный мотоцикл для города и небольших поездок.",
                ImagePath = "/Images/bmw g 310r.jpg"
            },
            new Domain.Motorcycle
            {
                Brand = "Ducati",
                Model = "Scrambler",
                Year = 2021,
                EngineCapacity = 803,
                Mileage = 8000,
                Color = "Yellow",
                Price = 11999,
                Description = "Стильный итальянский мотоцикл с уникальным дизайном и отличной управляемостью.",
                ImagePath = "/Images/ducati scrambler.jpg"
            },
            new Domain.Motorcycle
            {
                Brand = "Aprilia",
                Model = "RS 660",
                Year = 2022,
                EngineCapacity = 659,
                Mileage = 2000,
                Color = "Red",
                Price = 14999,
                Description = "Спортбайк с агрессивным дизайном и выдающейся динамикой.",
                ImagePath = "/Images/aprilia rs 660.jpg"
            }
        };

        foreach (var motorcycle in testMotorcycles)
        {
            _motorcycleRepository.AddAsync(motorcycle).Wait();
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        // ВАЖНО: Освобождаем ресурсы DbContext при закрытии приложения
        _dbContext?.Dispose();
        base.OnExit(e);
    }
}

