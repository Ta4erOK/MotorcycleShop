using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;

namespace MotorcycleShop.UI.ViewModels
{
    public class CartViewModel : BaseViewModel
    {
        private readonly ICartRepository _cartRepository;

        private decimal _totalAmount;
        private string _statusMessage = "Готово";

        public ObservableCollection<CartItem> CartItems { get; }
        
        public decimal TotalAmount
        {
            get => _totalAmount;
            set => SetField(ref _totalAmount, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetField(ref _statusMessage, value);
        }

        public ICommand CheckoutCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand ContinueShoppingCommand { get; }

        public CartViewModel(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;

            CartItems = new ObservableCollection<CartItem>();

            CheckoutCommand = new RelayCommand(OpenCheckoutWindow);
            RemoveItemCommand = new RelayCommand<CartItem>(RemoveItem);
            ContinueShoppingCommand = new RelayCommand(ContinueShopping);

            LoadCartItemsAsync();
        }

        private async void LoadCartItemsAsync()
        {
            try
            {
                StatusMessage = "Загрузка корзины...";
                
                var items = await _cartRepository.GetAllItemsAsync();
                
                CartItems.Clear();
                foreach (var item in items)
                {
                    CartItems.Add(item);
                }

                TotalAmount = await _cartRepository.GetTotalAmountAsync();
                StatusMessage = $"Корзина: {CartItems.Count} товаров на сумму {TotalAmount:C}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка загрузки корзины: {ex.Message}";
            }
        }

        private void OpenCheckoutWindow()
        {
            // Открытие окна оформления заказа
            var orderWindow = new Views.OrderWindow(_cartRepository);
            var result = orderWindow.ShowDialog();
            
            // После оформления заказа обновляем корзину
            if (result == true)
            {
                LoadCartItemsAsync();
            }
        }

        private async void RemoveItem(CartItem? item)
        {
            if (item != null)
            {
                var result = await _cartRepository.RemoveItemAsync(item.Id);
                if (result)
                {
                    CartItems.Remove(item);
                    TotalAmount = await _cartRepository.GetTotalAmountAsync();
                    StatusMessage = $"Удалено. Общая сумма: {TotalAmount:C}";
                }
            }
        }

        private void ContinueShopping()
        {
            // Закрыть окно корзины - это будет обработано в code-behind
        }
    }
}