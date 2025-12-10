using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace MotorcycleShop.UI.Controls
{
    public static class PlaceholderHelper
    {
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.RegisterAttached(
                "Placeholder",
                typeof(string),
                typeof(PlaceholderHelper),
                new PropertyMetadata("", OnPlaceholderChanged));

        public static string GetPlaceholder(DependencyObject obj)
        {
            return (string)obj.GetValue(PlaceholderProperty);
        }

        public static void SetPlaceholder(DependencyObject obj, string value)
        {
            obj.SetValue(PlaceholderProperty, value);
        }

        private static void OnPlaceholderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var placeholder = (string)e.NewValue;
            if (d is TextBox textBox)
            {
                // Обработка TextBox
                textBox.Loaded += (sender, args) =>
                {
                    if (string.IsNullOrEmpty(textBox.Text))
                    {
                        textBox.Text = placeholder;
                        textBox.FontStyle = FontStyles.Italic;
                        textBox.Foreground = new SolidColorBrush(Colors.Gray);
                    }
                };

                textBox.GotFocus += (sender, args) =>
                {
                    if (textBox.Text == placeholder)
                    {
                        textBox.Text = "";
                        textBox.FontStyle = FontStyles.Normal;
                        textBox.Foreground = new SolidColorBrush(Colors.Black);
                    }
                };

                textBox.LostFocus += (sender, args) =>
                {
                    if (string.IsNullOrEmpty(textBox.Text))
                    {
                        textBox.Text = placeholder;
                        textBox.FontStyle = FontStyles.Italic;
                        textBox.Foreground = new SolidColorBrush(Colors.Gray);
                    }
                };
            }
            else if (d is ComboBox comboBox)
            {
                // Обработка ComboBox
                comboBox.Loaded += (sender, args) =>
                {
                    if (comboBox.SelectedItem == null && string.IsNullOrEmpty(comboBox.Text))
                    {
                        comboBox.Text = placeholder;
                        comboBox.FontStyle = FontStyles.Italic;
                        comboBox.Foreground = new SolidColorBrush(Colors.Gray);
                    }
                };

                comboBox.GotFocus += (sender, args) =>
                {
                    if (comboBox.Text == placeholder)
                    {
                        comboBox.Text = "";
                        comboBox.FontStyle = FontStyles.Normal;
                        comboBox.Foreground = new SolidColorBrush(Colors.Black);
                    }
                };

                comboBox.LostFocus += (sender, args) =>
                {
                    if (string.IsNullOrEmpty(comboBox.Text) && comboBox.SelectedItem == null)
                    {
                        comboBox.Text = placeholder;
                        comboBox.FontStyle = FontStyles.Italic;
                        comboBox.Foreground = new SolidColorBrush(Colors.Gray);
                    }
                };
            }
        }
    }
}