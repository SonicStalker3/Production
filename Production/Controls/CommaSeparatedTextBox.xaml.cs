using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Production.Controls
{
    /// <summary>
    /// Логика взаимодействия для CommaSeparatedTextBox.xaml
    /// </summary>
    public partial class CommaSeparatedTextBox : UserControl
    {
        public CommaSeparatedTextBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(CommaSeparatedTextBox), new PropertyMetadata(string.Empty));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty SuggestionsProperty =
            DependencyProperty.Register("Suggestions", typeof(IEnumerable<object>), typeof(CommaSeparatedTextBox), new PropertyMetadata(null));

        public IEnumerable<object> Suggestions
        {
            get { return (IEnumerable<object>)GetValue(SuggestionsProperty); }
            set { SetValue(SuggestionsProperty, value); }
        }

        public static readonly DependencyProperty DisplayableMemberPathProperty =
            DependencyProperty.Register("DisplayableMemberPath", typeof(string), typeof(CommaSeparatedTextBox), new PropertyMetadata(string.Empty));

        public string DisplayableMemberPath
        {
            get { return (string)GetValue(DisplayableMemberPathProperty); }
            set { SetValue(DisplayableMemberPathProperty, value); }
        }

        private void InputTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Получаем текст из TextBox
            string input = InputTextBox.Text;

            // Разделяем текст на слова и берем последнее слово
            var words = input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                             .Select(word => word.Trim())
                             .ToList();

            string currentWord = words.LastOrDefault();

            // Если текущее слово пустое, скрываем список подсказок
            if (string.IsNullOrWhiteSpace(currentWord))
            {
                SuggestionsListBox.Visibility = Visibility.Collapsed;
                return;
            }

            // Получаем список подсказок, которые начинаются с текущего слова
            var suggestions = Suggestions?
                .Where(entry => entry.GetType().GetProperty(DisplayableMemberPath)?.GetValue(entry, null)?.ToString().StartsWith(currentWord, StringComparison.OrdinalIgnoreCase) == true)
                .Select(entry => entry.GetType().GetProperty(DisplayableMemberPath)?.GetValue(entry, null)?.ToString())
                .ToList();

            // Обновляем список подсказок
            SuggestionsListBox.ItemsSource = suggestions;

            // Показываем или скрываем список подсказок
            SuggestionsListBox.Visibility = suggestions != null && suggestions.Any() ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SuggestionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SuggestionsListBox.SelectedItem is string selectedSuggestion)
            {
                // Получаем текущий текст
                var currentText = InputTextBox.Text;

                // Разделяем текст на элементы
                var items = currentText.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                       .Select(item => item.Trim())
                                       .ToList();

                // Определяем текущее слово, которое нужно заменить
                string lastInput = items.LastOrDefault();
                if (string.IsNullOrWhiteSpace(lastInput))
                {
                    // Если последнее слово пустое, ничего не делаем
                    return;
                }

                // Заменяем последнее слово на выбранную подсказку
                items[items.Count - 1] = selectedSuggestion.Trim();

                // Обновляем текст в InputTextBox
                InputTextBox.Text = string.Join(", ", items) + (items.Count > 0 ? ", " : string.Empty);
                InputTextBox.CaretIndex = InputTextBox.Text.Length; // Устанавливаем курсор в конец
                SuggestionsListBox.Visibility = Visibility.Collapsed; // Скрываем список подсказок
            }
        }
    }
}
