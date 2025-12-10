using System.Windows;
using MotorcycleShop.Data.InMemory;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;
using MotorcycleShop.UI.ViewModels;

namespace MotorcycleShop.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для MotorcycleDetailsWindow.xaml
    /// </summary>
    public partial class MotorcycleDetailsWindow : Window
    {
        public MotorcycleDetailsWindow(Motorcycle motorcycle)
        {
            InitializeComponent();

            // В продвинутом приложении зависимости будут внедряться через DI контейнер
            // Создаем репозиторий через DI контейнер или как часть приложения
            // Но для правильной работы передаем репозиторий извне, иначе создаем новый только для теста
            var cartRepository = new InMemoryCartRepository();

            // Для корректной работы используем второй конструктор
            var viewModel = new MotorcycleDetailsViewModel(motorcycle, cartRepository, this);
            DataContext = viewModel;
        }

        // Конструктор с передачей репозитория корзины
        public MotorcycleDetailsWindow(Motorcycle motorcycle, ICartRepository cartRepository)
        {
            InitializeComponent();

            var viewModel = new MotorcycleDetailsViewModel(motorcycle, cartRepository, this);
            DataContext = viewModel;
        }
    }
}