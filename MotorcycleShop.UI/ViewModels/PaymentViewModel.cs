using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using MotorcycleShop.Domain;
using MotorcycleShop.UI.Views;

namespace MotorcycleShop.UI.ViewModels
{
    public class PaymentViewModel : BaseViewModel
    {
        private Order _order;
        private PaymentWindow _window;
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
            _window = null;
            Order = _order;

            PayCommand = new RelayCommand(ValidateAndProcessPayment);
            CancelCommand = new RelayCommand(CancelPayment);
        }

        // Конструктор с передачей окна
        public PaymentViewModel(Order order, PaymentWindow window)
        {
            _order = order ?? throw new ArgumentNullException(nameof(order));
            _window = window;
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

            // Закрываем все окна, кроме главного
            CloseAllWindowsExceptMain();

            // Закрываем текущее окно
            _window?.Dispatcher.Invoke(() =>
            {
                _window.Close();
            });
        }

        private void CloseAllWindowsExceptMain()
        {
            // Получаем все окна приложения
            var applicationWindows = new System.Collections.Generic.List<Window>();

            foreach (Window window in Application.Current.Windows)
            {
                // Не закрываем главное окно
                if (window != Application.Current.MainWindow)
                {
                    applicationWindows.Add(window);
                }
            }

            // Закрываем все окна, кроме главного
            foreach (var window in applicationWindows)
            {
                window.Close();
            }
        }

        public void ProcessPayment()
        {
            ValidateAndProcessPayment();
        }

        private bool ValidatePaymentData()
        {
            // Проверка, что поля не пусты
            if (string.IsNullOrWhiteSpace(CardNumber))
            {
                System.Windows.MessageBox.Show("Пожалуйста, введите номер карты.", "Ошибка",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ExpiryDate))
            {
                System.Windows.MessageBox.Show("Пожалуйста, введите срок действия карты.", "Ошибка",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(Cvv))
            {
                System.Windows.MessageBox.Show("Пожалуйста, введите CVV код.", "Ошибка",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(CardHolderName))
            {
                System.Windows.MessageBox.Show("Пожалуйста, введите имя владельца карты.", "Ошибка",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return false;
            }

            // Проверка номера карты (16 цифр, с пробелами или без)
            var cardNumberDigits = Regex.Replace(CardNumber, @"[^\d]", "");
            if (cardNumberDigits.Length != 16 || !long.TryParse(cardNumberDigits, out _))
            {
                System.Windows.MessageBox.Show("Номер карты должен содержать 16 цифр.", "Ошибка",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return false;
            }

            // Проверка даты (MM/YY или MM/YY)
            var dateParts = ExpiryDate.Split('/');
            if (dateParts.Length != 2 ||
                !int.TryParse(dateParts[0], out int month) ||
                !int.TryParse(dateParts[1], out int year))
            {
                System.Windows.MessageBox.Show("Неверный формат срока действия. Используйте формат ММ/ГГ.", "Ошибка",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return false;
            }

            if (month < 1 || month > 12 || year < 0 || year > 99)
            {
                System.Windows.MessageBox.Show("Неверный срок действия карты.", "Ошибка",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return false;
            }

            // Проверка CVV (3 цифры)
            if (!int.TryParse(Cvv, out int cvvValue) || cvvValue < 0 || cvvValue > 999 || Cvv.Length != 3)
            {
                System.Windows.MessageBox.Show("CVV код должен содержать 3 цифры.", "Ошибка",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return false;
            }

            // Проверка имени владельца карты (не должно быть пустым)
            if (string.IsNullOrWhiteSpace(CardHolderName))
            {
                System.Windows.MessageBox.Show("Пожалуйста, введите имя владельца карты.", "Ошибка",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void CancelPayment()
        {
            // Закрытие окна
            _window?.Close();
        }
    }
}