using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using MotorcycleShop.Domain;
using MotorcycleShop.UI.ViewModels;

namespace MotorcycleShop.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для PaymentWindow.xaml
    /// </summary>
    public partial class PaymentWindow : Window
    {
        public PaymentWindow(Order order)
        {
            InitializeComponent();

            var viewModel = new PaymentViewModel(order);
            DataContext = viewModel;

            // Добавляем обработчики для форматирования ввода
            CardNumberTextBox.TextChanged += CardNumberTextBox_TextChanged;
            ExpiryDateTextBox.TextChanged += ExpiryDateTextBox_TextChanged;
        }

        private void CardNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                // Удаляем все, кроме цифр
                var text = Regex.Replace(textBox.Text, @"[^\d]", "");

                // Ограничиваем длину до 16 символов
                if (text.Length > 16)
                {
                    text = text.Substring(0, 16);
                }

                // Форматируем в XXXX XXXX XXXX XXXX
                var formattedText = "";
                for (int i = 0; i < text.Length; i++)
                {
                    if (i > 0 && i % 4 == 0)
                    {
                        formattedText += " ";
                    }
                    formattedText += text[i];
                }

                // Сохраняем позицию курсора
                var caretIndex = textBox.CaretIndex;
                textBox.Text = formattedText;
                textBox.CaretIndex = caretIndex;
            }
        }

        private void ExpiryDateTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                // Удаляем все, кроме цифр
                var text = Regex.Replace(textBox.Text, @"[^\d]", "");

                // Ограничиваем длину до 4 символов
                if (text.Length > 4)
                {
                    text = text.Substring(0, 4);
                }

                // Форматируем в MM/YY
                if (text.Length > 2)
                {
                    text = text.Substring(0, 2) + "/" + text.Substring(2);
                }

                // Сохраняем позицию курсора
                var caretIndex = textBox.CaretIndex;
                textBox.Text = text;
                textBox.CaretIndex = caretIndex;
            }
        }

        private void CancelCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}