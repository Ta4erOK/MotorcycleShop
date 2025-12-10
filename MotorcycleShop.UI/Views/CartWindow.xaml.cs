using System.Windows;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.UI.ViewModels;

namespace MotorcycleShop.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для CartWindow.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        private CartViewModel _viewModel;

        // Конструктор для передачи репозитория извне
        public CartWindow(ICartRepository cartRepository)
        {
            InitializeComponent();

            _viewModel = new CartViewModel(cartRepository, this);
            DataContext = _viewModel;
        }

        private void RemoveItemCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            // Обработка удаления элемента будет в ViewModel
        }
    }
}