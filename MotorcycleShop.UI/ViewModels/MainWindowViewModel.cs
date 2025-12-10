using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;

namespace MotorcycleShop.UI.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly ICartRepository _cartRepository;

        private string _searchTerm = string.Empty;
        private string _selectedBrand = string.Empty;
        private int? _selectedYear;
        private decimal? _minPrice;
        private decimal? _maxPrice;
        private int _cartItemCount;
        private string _statusMessage = "Готово";

        public ObservableCollection<Motorcycle> Motorcycles { get; }
        public ObservableCollection<string> Brands { get; }
        public ObservableCollection<int> Years { get; }

        public string SearchTerm
        {
            get => _searchTerm;
            set => SetField(ref _searchTerm, value);
        }

        public string SelectedBrand
        {
            get => _selectedBrand;
            set => SetField(ref _selectedBrand, value);
        }

        public int? SelectedYear
        {
            get => _selectedYear;
            set => SetField(ref _selectedYear, value);
        }

        public decimal? MinPrice
        {
            get => _minPrice;
            set => SetField(ref _minPrice, value);
        }

        public decimal? MaxPrice
        {
            get => _maxPrice;
            set => SetField(ref _maxPrice, value);
        }

        public int CartItemCount
        {
            get => _cartItemCount;
            set => SetField(ref _cartItemCount, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetField(ref _statusMessage, value);
        }

        public ICommand SearchCommand { get; }
        public ICommand ClearFiltersCommand { get; }
        public ICommand OpenCartCommand { get; }
        public ICommand OpenMotorcycleDetailsCommand { get; }

        public MainWindowViewModel(IMotorcycleRepository motorcycleRepository, ICartRepository cartRepository)
        {
            _motorcycleRepository = motorcycleRepository;
            _cartRepository = cartRepository;

            Motorcycles = new ObservableCollection<Motorcycle>();
            Brands = new ObservableCollection<string>();
            Years = new ObservableCollection<int>();

            SearchCommand = new RelayCommand(async () => await SearchAsync());
            ClearFiltersCommand = new RelayCommand(async () => await ClearFiltersAsync());
            OpenCartCommand = new RelayCommand(OpenCart);
            OpenMotorcycleDetailsCommand = new RelayCommand<Motorcycle>(OpenMotorcycleDetails);

            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            try
            {
                StatusMessage = "Загрузка данных...";

                var motorcycles = await _motorcycleRepository.GetAllAsync();
                var allBrands = new ObservableCollection<string>(motorcycles.Select(m => m.Brand).Distinct().OrderBy(b => b));
                var allYears = new ObservableCollection<int>(motorcycles.Select(m => m.Year).Distinct().OrderBy(y => y));

                Motorcycles.Clear();
                foreach (var motorcycle in motorcycles)
                {
                    Motorcycles.Add(motorcycle);
                }

                Brands.Clear();
                foreach (var brand in allBrands)
                {
                    Brands.Add(brand);
                }

                Years.Clear();
                foreach (var year in allYears)
                {
                    Years.Add(year);
                }

                CartItemCount = await _cartRepository.GetItemCountAsync();
                StatusMessage = $"Загружено {Motorcycles.Count} мотоциклов";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка загрузки: {ex.Message}";
            }
        }

        private async Task SearchAsync()
        {
            try
            {
                StatusMessage = "Поиск...";

                var results = await _motorcycleRepository.SearchAsync(
                    SelectedBrand,
                    SelectedYear,
                    MinPrice,
                    MaxPrice,
                    SearchTerm);

                Motorcycles.Clear();
                foreach (var motorcycle in results)
                {
                    Motorcycles.Add(motorcycle);
                }

                StatusMessage = $"Найдено {Motorcycles.Count} мотоциклов";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка поиска: {ex.Message}";
            }
        }

        private async Task ClearFiltersAsync()
        {
            SearchTerm = string.Empty;
            SelectedBrand = string.Empty;  // или можно использовать null, если тип будет изменен на string?
            SelectedYear = null;
            MinPrice = null;
            MaxPrice = null;

            // Загружаем все мотоциклы без фильтрации
            try
            {
                StatusMessage = "Загрузка каталога...";

                var motorcycles = await _motorcycleRepository.GetAllAsync();

                Motorcycles.Clear();
                foreach (var motorcycle in motorcycles)
                {
                    Motorcycles.Add(motorcycle);
                }

                StatusMessage = $"Показано {Motorcycles.Count} мотоциклов";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка загрузки каталога: {ex.Message}";
            }
        }

        private void OpenCart()
        {
            // Открытие окна корзины
            var cartWindow = new Views.CartWindow(_cartRepository);
            cartWindow.ShowDialog();

            // После закрытия окна корзины обновляем количество элементов в корзине
            _ = UpdateCartItemCountAsync();
        }

        private async Task UpdateCartItemCountAsync()
        {
            CartItemCount = await _cartRepository.GetItemCountAsync();
        }

        private void OpenMotorcycleDetails(Motorcycle? motorcycle)
        {
            if (motorcycle != null)
            {
                // Открытие окна с деталями мотоцикла с передачей репозитория корзины
                var detailsWindow = new Views.MotorcycleDetailsWindow(motorcycle, _cartRepository);
                detailsWindow.ShowDialog();

                // Обновляем количество элементов в корзине после закрытия окна
                _ = UpdateCartItemCountAsync();
            }
        }
    }
}