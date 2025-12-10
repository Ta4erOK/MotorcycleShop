using System.Windows;
using MotorcycleShop.Data.InMemory;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.UI.ViewModels;

namespace MotorcycleShop.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для CartWindow.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        public CartWindow()
        {
            InitializeComponent();

            // В продвинутом приложении зависимости будут внедряться через DI контейнер
            var cartRepository = new InMemoryCartRepository();
            
            var viewModel = new CartViewModel(cartRepository);
            DataContext = viewModel;
        }

        // Конструктор для передачи репозитория извне
        public CartWindow(ICartRepository cartRepository)
        {
            InitializeComponent();
            
            var viewModel = new CartViewModel(cartRepository);
            DataContext = viewModel;
        }

        private void ContinueShoppingCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void RemoveItemCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            // Обработка удаления элемента будет в ViewModel
        }
    }
}