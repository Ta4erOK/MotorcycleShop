using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.UI.ViewModels;

namespace MotorcycleShop.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(IMotorcycleRepository motorcycleRepository, IOrderRepository orderRepository, ICartRepository cartRepository)
    {
        InitializeComponent();

        var viewModel = new MainWindowViewModel(motorcycleRepository, cartRepository);
        DataContext = viewModel;
    }

    private void MotorcycleDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            // Находим DataGrid в предке
            var dataGrid = sender as DataGrid;
            if (dataGrid != null && dataGrid.SelectedItem is Domain.Motorcycle selectedMotorcycle)
            {
                viewModel.OpenMotorcycleDetailsCommand.Execute(selectedMotorcycle);
            }
            // Если клик был по кнопке, обработка будет в XAML
        }
    }

    private void DetailsButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            // Получаем мотоцикл из контекста данных кнопки
            if (sender is Button button && button.DataContext is Domain.Motorcycle motorcycle)
            {
                viewModel.OpenMotorcycleDetailsCommand.Execute(motorcycle);
            }
        }
    }
}