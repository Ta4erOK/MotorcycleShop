using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;

namespace MotorcycleShop.UI.ViewModels
{
    public class OrderStatusViewModel : BaseViewModel
    {
        private readonly IOrderRepository _orderRepository;

        private string _orderNumber = string.Empty;
        private Order? _currentOrder;
        private string _statusMessage = "Введите номер заказа для поиска";

        public string OrderNumber
        {
            get => _orderNumber;
            set => SetField(ref _orderNumber, value);
        }

        public Order? CurrentOrder
        {
            get => _currentOrder;
            set => SetField(ref _currentOrder, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetField(ref _statusMessage, value);
        }

        public ICommand SearchCommand { get; }
        public ICommand NewOrderCommand { get; }

        public OrderStatusViewModel(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;

            SearchCommand = new RelayCommand(async () => await SearchOrderAsync());
            NewOrderCommand = new RelayCommand(NewOrder);

            // Попробуем получить все заказы для отображения (для демонстрации)
            LoadAllOrdersAsync();
        }

        private async void LoadAllOrdersAsync()
        {
            try
            {
                var orders = await _orderRepository.GetAllAsync();
                // В реальном приложении здесь можно загружать все заказы, 
                // но в демонстрационных целях мы будем искать по номеру
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка загрузки заказов: {ex.Message}";
            }
        }

        private async Task SearchOrderAsync()
        {
            if (int.TryParse(OrderNumber, out int orderId))
            {
                try
                {
                    var order = await _orderRepository.GetByIdAsync(orderId);
                    if (order != null)
                    {
                        CurrentOrder = order;
                        StatusMessage = $"Заказ #{order.Id} найден";
                    }
                    else
                    {
                        CurrentOrder = null;
                        StatusMessage = "Заказ не найден";
                    }
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Ошибка поиска заказа: {ex.Message}";
                }
            }
            else
            {
                StatusMessage = "Введите корректный номер заказа";
            }
        }

        private void NewOrder()
        {
            // Закрыть окно статуса заказа для возврата к новому заказу
            // Это будет обработано в code-behind
        }
    }
}