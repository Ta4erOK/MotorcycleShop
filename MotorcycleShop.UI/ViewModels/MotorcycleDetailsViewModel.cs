using System;
using System.Windows.Input;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;
using MotorcycleShop.UI.Views;

namespace MotorcycleShop.UI.ViewModels
{
    public class MotorcycleDetailsViewModel : BaseViewModel
    {
        private readonly Motorcycle _motorcycle;
        private readonly ICartRepository _cartRepository;
        private readonly MotorcycleDetailsWindow _window;

        public Motorcycle Motorcycle { get; }

        public ICommand AddToCartCommand { get; }
        public ICommand GoBackCommand { get; }

        public MotorcycleDetailsViewModel(Motorcycle motorcycle, ICartRepository cartRepository)
        {
            _motorcycle = motorcycle ?? throw new ArgumentNullException(nameof(motorcycle));
            _cartRepository = cartRepository;
            _window = null; // будет null, если не передано окно
            Motorcycle = _motorcycle;

            AddToCartCommand = new RelayCommand(AddToCart);
            GoBackCommand = new RelayCommand(GoBack);
        }

        // Конструктор с передачей окна
        public MotorcycleDetailsViewModel(Motorcycle motorcycle, ICartRepository cartRepository, MotorcycleDetailsWindow window)
        {
            _motorcycle = motorcycle ?? throw new ArgumentNullException(nameof(motorcycle));
            _cartRepository = cartRepository;
            _window = window;
            Motorcycle = _motorcycle;

            AddToCartCommand = new RelayCommand(AddToCart);
            GoBackCommand = new RelayCommand(GoBack);
        }

        private async void AddToCart()
        {
            // Добавить мотоцикл в корзину
            await _cartRepository.AddItemAsync(_motorcycle, 1);
            System.Windows.MessageBox.Show("Мотоцикл добавлен в корзину!", "Успешно",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }

        private void GoBack()
        {
            // Закрыть окно
            _window?.Close();
        }
    }
}