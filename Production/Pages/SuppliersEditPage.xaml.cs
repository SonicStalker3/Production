using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Input;
using Production.DB;
using Production.Pages.EditingPages;

namespace Production.Pages
{
    /// <summary>
    /// Логика взаимодействия для SuppliersEditPage.xaml
    /// </summary>
    public partial class SuppliersEditPage : Page
    {

        ProductionEntities _context = DBContext.GetContext();
        public ObservableCollection<Supplier> Suppliers { get; set; } = new ObservableCollection<Supplier>();


        public SuppliersEditPage()
        {
            Task task = LoadSuppliersAsync();

            InitializeComponent();
            this.DataContext = this;
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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
/*            var selectedHotel = DGridHotel.SelectedItem as Отель;
            NavigationService.Navigate(new AddEditHotelsPage(null));*/
        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
/*            var selectedHotel = (sender as Button).DataContext as Отель;
            NavigationService.Navigate(new AddEditHotelsPage(selectedHotel));*/
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
/*            var selectedHotel = DGridHotel.SelectedItem as Отель;
            HotelEntities.GetContext().Отель.Remove(selectedHotel);
            HotelEntities.GetContext().SaveChanges();*/
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void Supplier_Selected(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedItem is Supplier selectedSupplier)
            {
                NavigationService.Navigate(new SupplierEditPage(selectedSupplier));
            }
        }

        private void Suppliers_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void SortComboBox_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void FiltrationComboBox_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
