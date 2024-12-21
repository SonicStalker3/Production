using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
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
using Production.DB;

namespace Production.Pages.EditingPages
{
    /// <summary>
    /// Логика взаимодействия для SupplierEditPage.xaml
    /// </summary>
    public partial class SupplierEditPage : Page
    {
        private Supplier _currentSupplier;
        ProductionEntities _context = DBContext.GetContext();
        private ObservableCollection<BusinessType> _materialSupplierTypes;
        private ObservableCollection<Supplier> _suppliers;
        public SupplierEditPage(Supplier supplier)
        {
            if (supplier != null)
            {
                _currentSupplier = supplier;
            }
            else
            {
                _currentSupplier = new Supplier();
            }
            InitializeComponent();
            DataContext = _currentSupplier;
            _materialSupplierTypes = new ObservableCollection<BusinessType>(_context.BusinessTypes.ToList());
            SupplierTypeField.ItemsSource = _materialSupplierTypes;
        }
        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            // Валидация наименования поставщика
            if (string.IsNullOrWhiteSpace(SupplierNameField.Text))
            {
                MessageBox.Show("Наименование поставщика не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Валидация ИНН
            if (string.IsNullOrWhiteSpace(INNField.Text))
            {
                MessageBox.Show("ИНН не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Валидация типа поставщика
            if (SupplierTypeField.SelectedItem == null)
            {
                MessageBox.Show("Тип поставщика должен быть выбран.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(RaitingField.Text, out int raiting) || raiting < 0 || raiting > 100)
            {
                MessageBox.Show("Допустимый рейтинг должнен быть положительным числом в диапазоне от 0 до 100.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Обновление или добавление материала
            if (_currentSupplier.SupplierID == 0)
            {
                _context.Suppliers.Add(_currentSupplier);
                MessageBox.Show("Материал успешно добавлен!");
            }
            else
            {
                _context.Suppliers.AddOrUpdate(_currentSupplier);
                MessageBox.Show("Материал успешно отредактирован!");
            }

            try
            {
                _context.SaveChanges();
                MessageBox.Show("Изменения сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении материала: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Отображаем диалоговое окно с подтверждением
            var continueBox = MessageBox.Show("Вы уверены, что хотите удалить этот материал?",
                                                "Подтверждение удаления",
                                                MessageBoxButton.OKCancel,
                                                MessageBoxImage.Warning);

            // Проверяем, нажал ли пользователь "OK"
            if (continueBox == MessageBoxResult.OK)
            {
                try
                {
                    // Удаляем материал из контекста
                    _context.Suppliers.Remove(_currentSupplier);
                    _context.SaveChanges();

                    MessageBox.Show("Материал успешно удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.GoBack(); // Возвращаемся на предыдущую страницу
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении материала: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
