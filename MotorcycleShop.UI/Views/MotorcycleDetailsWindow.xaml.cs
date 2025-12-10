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
            var cartRepository = new InMemoryCartRepository();

            var viewModel = new MotorcycleDetailsViewModel(motorcycle, cartRepository);
            DataContext = viewModel;
        }

        // Конструктор с передачей репозитория корзины
        public MotorcycleDetailsWindow(Motorcycle motorcycle, ICartRepository cartRepository)
        {
            InitializeComponent();

            var viewModel = new MotorcycleDetailsViewModel(motorcycle, cartRepository);
            DataContext = viewModel;
        }

        private void GoBackCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}