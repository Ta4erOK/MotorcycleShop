using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;

namespace MotorcycleShop.UI.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;

        private string _customerName = string.Empty;
        private string _email = string.Empty;
        private string _phone = string.Empty;
        private string _address = string.Empty;
        private string _comment = string.Empty;
        private decimal _orderTotal;

        public string CustomerName
        {
            get => _customerName;
            set => SetField(ref _customerName, value);
        }

        public string Email
        {
            get => _email;
            set => SetField(ref _email, value);
        }

        public string Phone
        {
            get => _phone;
            set => SetField(ref _phone, value);
        }

        public string Address
        {
            get => _address;
            set => SetField(ref _address, value);
        }

        public string Comment
        {
            get => _comment;
            set => SetField(ref _comment, value);
        }

        public decimal OrderTotal
        {
            get => _orderTotal;
            set => SetField(ref _orderTotal, value);
        }

        public ICommand PayCommand { get; }
        public ICommand GoBackCommand { get; }

        public OrderViewModel(ICartRepository cartRepository, IOrderRepository orderRepository)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;

            PayCommand = new RelayCommand(async () => await ProcessOrderAsync());
            GoBackCommand = new RelayCommand(GoBack);

            CalculateOrderTotalAsync();
        }

        private async void CalculateOrderTotalAsync()
        {
            OrderTotal = await _cartRepository.GetTotalAmountAsync();
        }

        private async Task ProcessOrderAsync()
        {
            try
            {
                // Создание заказа
                var order = new Order
                {
                    CustomerName = CustomerName,
                    Email = Email,
                    Phone = Phone,
                    Address = Address,
                    Comment = Comment,
                    Items = new System.Collections.Generic.List<OrderItem>() // В реальной реализации нужно преобразовать CartItem в OrderItem
                };

                // Преобразование элементов корзины в элементы заказа
                var cartItems = await _cartRepository.GetAllItemsAsync();
                foreach (var cartItem in cartItems)
                {
                    order.Items.Add(new OrderItem
                    {
                        Motorcycle = cartItem.Motorcycle,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Motorcycle.Price
                    });
                }

                // Сохранение заказа
                var savedOrder = await _orderRepository.CreateAsync(order);

                // Очистка корзины после создания заказа
                await _cartRepository.ClearCartAsync();

                // Открытие окна оплаты
                var paymentWindow = new Views.PaymentWindow(savedOrder);
                var result = paymentWindow.ShowDialog();
                
                if (result == true)
                {
                    // Закрытие окна оформления заказа с результатом true (успешно)
                    // Это будет обработано в code-behind
                }
            }
            catch (Exception ex)
            {
                // В реальном приложении нужно показать сообщение об ошибке
                System.Windows.MessageBox.Show($"Ошибка при создании заказа: {ex.Message}", "Ошибка", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void GoBack()
        {
            // Закрытие окна - будет обработано в code-behind
        }
    }
}