using System.Windows;
using MotorcycleShop.Data.InMemory;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.UI.ViewModels;

namespace MotorcycleShop.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private OrderViewModel _viewModel;

        public OrderWindow()
        {
            InitializeComponent();

            // В продвинутом приложении зависимости будут внедряться через DI контейнер
            var cartRepository = new InMemoryCartRepository();
            var orderRepository = new InMemoryOrderRepository();

            _viewModel = new OrderViewModel(cartRepository, orderRepository, this);
            DataContext = _viewModel;
        }

        // Конструктор для передачи репозитория корзины извне
        public OrderWindow(ICartRepository cartRepository)
        {
            InitializeComponent();

            var orderRepository = new InMemoryOrderRepository();

            _viewModel = new OrderViewModel(cartRepository, orderRepository, this);
            DataContext = _viewModel;
        }
    }
}