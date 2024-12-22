using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Data.Entity;
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

namespace Production.Pages
{
    /// <summary>
    /// Логика взаимодействия для SuppliersViewPage.xaml
    /// </summary>
    public partial class SuppliersViewPage : Page
    {
        ProductionEntities _context = DBContext.GetContext();
        private ObservableCollection<Supplier> _allSuppliers { get; set; } = new ObservableCollection<Supplier>();
        public ObservableCollection<Supplier> Suppliers { get; set; } = new ObservableCollection<Supplier>();
        public ObservableCollection<BusinessType> BusinessTypes { get; set; } = new ObservableCollection<BusinessType>();
        public SuppliersViewPage()
        {
            this.DataContext = this;
            var supplierTypes = DBContext.GetContext().BusinessTypes.ToList();
            var allSupplierTypes = new BusinessType
            {
                Title = "Все",
                BusinessTypeID = -1
            };
            supplierTypes.Insert(0, allSupplierTypes);

            BusinessTypes = new ObservableCollection<BusinessType>(supplierTypes);

            var suppliers = DBContext.GetContext().Suppliers.Include(m => m.BusinessType).OrderBy(x => x.Name).ToList();
            _allSuppliers = new ObservableCollection<Supplier>(suppliers);

            LoadSuppliersAsync();
            InitializeComponent();
        }
        private async Task LoadSuppliersAsync()
        {
            try
            {
                int batchSize = 10;
                int skip = 0;
                bool moreSuppliers = true;

                while (moreSuppliers)
                {
                    var loadedSuppliers = await _context.Suppliers
                        .Include(m => m.Materials)
                        .OrderBy(m => m.SupplierID)
                        .Skip(skip)
                        .Take(batchSize)
                        .ToListAsync();

                    if (loadedSuppliers.Count > 0)
                    {
                        foreach (var supplier in loadedSuppliers)
                        {
                            Suppliers.Add(supplier);
                        }

                        skip += batchSize;
                    }
                    else
                    {
                        moreSuppliers = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Логирование или обработка исключения
                Debug.WriteLine($"Ошибка при загрузке материалов: {ex.Message}");
            }
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Image image &&
                (image.Source == null ||
                string.IsNullOrWhiteSpace((image.Source as BitmapImage).UriSource?.ToString()
                )))
            {
                BitmapImage errorImage = BitmapToBitmapImage(Properties.Resources.ErrorImage);
                image.Source = errorImage;
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
        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchPlaceholderText.Visibility = Visibility.Collapsed;
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchPlaceholderText.Visibility = Visibility.Visible;
            }
        }
        private void SortComboBox_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void FiltrationComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SortPlaceholderText.Visibility = Visibility.Collapsed;
        }

        private void FiltrationComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SortPlaceholderText.Visibility = Visibility.Visible;
            }

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ApplyFilters()
        {
            var searchText = SearchTextBox.Text.ToLower();
            var filteredSuppliers = _allSuppliers.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filteredSuppliers = filteredSuppliers.Where(t =>
                    t.INN.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    t.Name.Replace('c', 'с').Replace('C', 'С').IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (FiltrationComboBox.SelectedItem is BusinessType selectedType)
            {
                if (selectedType.BusinessTypeID != -1)
                {
                    filteredSuppliers = filteredSuppliers.Where(t => t.BusinessType == selectedType);
                }
            }

            Suppliers.Clear();
            foreach (var tour in filteredSuppliers.OrderBy(t => t.Name))
            {
                Suppliers.Add(tour);
            }
        }
        private void FiltrationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }
    }
}
