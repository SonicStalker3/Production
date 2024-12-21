using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using Microsoft.Win32;
using System.Data.Entity.Migrations;

namespace Production.Pages.EditingPages
{
    /// <summary>
    /// Логика взаимодействия для MaterialEditPage.xaml
    /// </summary>
    public partial class MaterialEditPage : Page
    {
        private Material _currentMaterial;
        ProductionEntities _context = DBContext.GetContext();
        private ObservableCollection<MaterialType> _materialTypes;
        private ObservableCollection<MaterialUnitType> _materialUnitTypes;
        private ObservableCollection<Supplier> _suppliers;
        public MaterialEditPage(Material current_material = null)
        {
            if (current_material != null)
            {
                _currentMaterial = current_material;
            }
            DataContext = _currentMaterial;
            _materialTypes = new ObservableCollection<MaterialType>(DBContext.GetContext().MaterialTypes.ToList());
            _suppliers = new ObservableCollection<Supplier>(DBContext.GetContext().Suppliers.ToList());
            _materialUnitTypes = new ObservableCollection<MaterialUnitType>(DBContext.GetContext().MaterialUnitTypes.ToList());
            InitializeComponent();
            MaterialTypeField.ItemsSource = _materialTypes;
            MaterialUnitTypeField.ItemsSource = _materialUnitTypes;
            //SuppliersField.ItemsSource = _suppliers;
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на валидность данных
            if (string.IsNullOrWhiteSpace(NameField.Text))
            {
                MessageBox.Show("Наименование материала не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(PackageQuantityField.Text, out int packageQuantity) || packageQuantity < 0)
            {
                MessageBox.Show("Количество в упаковке должно быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(StockQuantityField.Text, out int stockQuantity) || stockQuantity < 0)
            {
                MessageBox.Show("Количество на складе должно быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(MinQuantityField.Text, out int minQuantity) || minQuantity < 0)
            {
                MessageBox.Show("Минимальное допустимое количество должно быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _currentMaterial.Name = NameField.Text;
            _currentMaterial.MaterialType = MaterialUnitTypeField.SelectedItem as MaterialType;
            _currentMaterial.PackageQuantity = packageQuantity;
            _currentMaterial.StockQuantity = stockQuantity;
            _currentMaterial.MinQuantity = minQuantity;
            _currentMaterial.Description = DescriptionField.Text;

            //MessageBox.Show(_currentMaterial.MaterialID.ToString());
            //_currentMaterial.Suppliers.Add(_context.Suppliers.FirstOrDefault());
            if (_currentMaterial.MaterialID == 0)
            {
                _context.Materials.Add(_currentMaterial);
                MessageBox.Show("Материал успешно добавлен!");
            }
            else
            {
                _context.Materials.AddOrUpdate(_currentMaterial);
                MessageBox.Show("Материал успешно отредактирован!");
            }

            try
            {
                _context.SaveChanges();
                MessageBox.Show("Изменения сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                //NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении материала: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Image image)
            {
                if (image.DataContext is Material material)
                {
                    if (material.Image == null || material.Image.Length == 0)
                    {
                        BitmapImage errorImage = BitmapToBitmapImage(Properties.Resources.ErrorImage);
                        image.Source = errorImage;
                    }
                }
            }
        }

        private BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // Замораживаем объект для использования в других потоках
                return bitmapImage;
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();

        }
        private void SuppliersButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MaterialSuppliersEditPage((sender as Button).DataContext as Material));
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Изменение картинки");
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif", // Фильтр для изображений
                Title = "Выберите изображение"
            };

            // Открываем диалог и проверяем, выбрал ли пользователь файл
            if (openFileDialog.ShowDialog() == true)
            {
                // Получаем путь к выбранному изображению
                string filePath = openFileDialog.FileName;

                // Загружаем изображение и обновляем источник
                BitmapImage bitmapImage = new BitmapImage(new Uri(filePath));
                // Предполагается, что у вас есть элемент Image с именем, например, "MaterialImage"
                _currentMaterial.Image = BitmapImageToByteArray(bitmapImage); // Обновите имя элемента Image на ваше
            }
        }
        private byte[] BitmapImageToByteArray(BitmapImage bitmapImage)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder(); // Вы можете использовать другой кодировщик, если нужно
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(memoryStream);
                return memoryStream.ToArray();
            }
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
                    _context.Materials.Remove(_currentMaterial);
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
