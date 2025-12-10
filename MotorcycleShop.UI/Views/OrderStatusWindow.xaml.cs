using System.Windows;
using MotorcycleShop.Data.InMemory;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.UI.ViewModels;

namespace MotorcycleShop.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для OrderStatusWindow.xaml
    /// </summary>
    public partial class OrderStatusWindow : Window
    {
        public OrderStatusWindow()
        {
            InitializeComponent();

            // В продвинутом приложении зависимости будут внедряться через DI контейнер
            var orderRepository = new InMemoryOrderRepository();
            
            var viewModel = new OrderStatusViewModel(orderRepository);
            DataContext = viewModel;
        }

        private void NewOrderCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}