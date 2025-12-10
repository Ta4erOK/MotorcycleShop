using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using MotorcycleShop.Domain;

namespace MotorcycleShop.UI.ViewModels
{
    public class PaymentViewModel : BaseViewModel
    {
        private Order _order;
        private string _cardNumber = string.Empty;
        private string _expiryDate = string.Empty;
        private string _cvv = string.Empty;
        private string _cardHolderName = string.Empty;

        public Order Order { get; }

        public string CardNumber
        {
            get => _cardNumber;
            set => SetField(ref _cardNumber, value);
        }

        public string ExpiryDate
        {
            get => _expiryDate;
            set => SetField(ref _expiryDate, value);
        }

        public string Cvv
        {
            get => _cvv;
            set => SetField(ref _cvv, value);
        }

        public string CardHolderName
        {
            get => _cardHolderName;
            set => SetField(ref _cardHolderName, value);
        }

        public decimal OrderTotal => Order?.TotalAmount ?? 0;

        public ICommand PayCommand { get; }
        public ICommand CancelCommand { get; }

        public PaymentViewModel(Order order)
        {
            _order = order ?? throw new ArgumentNullException(nameof(order));
            Order = _order;

            PayCommand = new RelayCommand(ValidateAndProcessPayment);
            CancelCommand = new RelayCommand(CancelPayment);
        }

        private void ValidateAndProcessPayment()
        {
            if (!ValidatePaymentData())
            {
                MessageBox.Show("Пожалуйста, проверьте правильность введенных данных карты.", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // В реальном приложении здесь будет вызов платежного провайдера
            // Для демонстрации просто покажем сообщение об успешной оплате
            MessageBox.Show($"Платеж на сумму {OrderTotal:C} успешно обработан.", "Оплата прошла успешно", 
                MessageBoxButton.OK, MessageBoxImage.Information);

            // Установка результата для окна
            // Это будет обработано в code-behind
        }

        private bool ValidatePaymentData()
        {
            // Проверка номера карты (16 цифр, с пробелами или без)
            var cardNumberDigits = Regex.Replace(CardNumber, @"[^\d]", "");
            if (cardNumberDigits.Length != 16 || !long.TryParse(cardNumberDigits, out _))
            {
                return false;
            }

            // Проверка даты (MM/YY или MM/YY)
            var dateParts = ExpiryDate.Split('/');
            if (dateParts.Length != 2 || 
                !int.TryParse(dateParts[0], out int month) || 
                !int.TryParse(dateParts[1], out int year))
            {
                return false;
            }

            if (month < 1 || month > 12 || year < 0 || year > 99)
            {
                return false;
            }

            // Проверка CVV (3 цифры)
            if (!int.TryParse(Cvv, out int cvvValue) || cvvValue < 0 || cvvValue > 999 || Cvv.Length != 3)
            {
                return false;
            }

            // Проверка имени владельца карты (не должно быть пустым)
            if (string.IsNullOrWhiteSpace(CardHolderName))
            {
                return false;
            }

            return true;
        }

        private void CancelPayment()
        {
            // Закрытие окна - будет обработано в code-behind
        }
    }
}