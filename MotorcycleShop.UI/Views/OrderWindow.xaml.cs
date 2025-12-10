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
        private readonly ICartRepository _cartRepository;

        public OrderWindow()
        {
            InitializeComponent();

            // В продвинутом приложении зависимости будут внедряться через DI контейнер
            _cartRepository = new InMemoryCartRepository();
            var orderRepository = new InMemoryOrderRepository();
            
            var viewModel = new OrderViewModel(_cartRepository, orderRepository);
            DataContext = viewModel;
        }

        // Конструктор для передачи репозитория корзины извне
        public OrderWindow(ICartRepository cartRepository)
        {
            InitializeComponent();
            
            _cartRepository = cartRepository;
            var orderRepository = new InMemoryOrderRepository();
            
            var viewModel = new OrderViewModel(_cartRepository, orderRepository);
            DataContext = viewModel;
        }

        private void GoBackCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}