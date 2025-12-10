using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MotorcycleShop.Data.InMemory;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.UI.ViewModels;

namespace MotorcycleShop.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // В продвинутом приложении зависимости будут внедряться через DI контейнер
        var motorcycleRepository = new InMemoryMotorcycleRepository();
        var cartRepository = new InMemoryCartRepository();

        var viewModel = new MainWindowViewModel(motorcycleRepository, cartRepository);
        DataContext = viewModel;
    }

    private void MotorcycleDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            var selectedMotorcycle = (sender as DataGrid)?.SelectedItem as Domain.Motorcycle;
            if (selectedMotorcycle != null)
            {
                viewModel.OpenMotorcycleDetailsCommand.Execute(selectedMotorcycle);
            }
        }
    }
}