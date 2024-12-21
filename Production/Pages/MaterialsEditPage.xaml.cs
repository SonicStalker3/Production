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
    /// Логика взаимодействия для MaterialsEditPage.xaml
    /// </summary>
    public partial class MaterialsEditPage : Page
    {
        ProductionEntities _context = DBContext.GetContext();
        public ObservableCollection<Material> Materials { get; set; } = new ObservableCollection<Material>();

        public MaterialsEditPage()
        {
            this.DataContext = this;
            Task task = LoadMaterialsAsync();

            InitializeComponent();
        }

        private async Task LoadMaterialsAsync()
        {
            try
            {
                int batchSize = 10;
                int skip = 0;
                bool moreMaterials = true;

                while (moreMaterials)
                {
                    var loadedMaterials = await _context.Materials
                        .Include(m => m.MaterialUnitType)
                        .Include(m => m.MaterialType)
                        .OrderBy(m => m.MaterialID)
                        .Skip(skip)
                        .Take(batchSize)
                        .ToListAsync();

                    if (loadedMaterials.Count > 0)
                    {
                        foreach (var material in loadedMaterials)
                        {
                            Materials.Add(material);
                        }

                        skip += batchSize;
                    }
                    else
                    {
                        moreMaterials = false;
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
            if(sender is System.Windows.Controls.Image image)
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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MaterialEditPage(null));
        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            /*            var selectedHotel = (sender as Button).DataContext as Отель;
                        NavigationService.Navigate(new AddEditHotelsPage(selectedHotel));*/
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Material_Selected(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedItem is Material selectedMaterial)
            {
                NavigationService.Navigate(new MaterialEditPage(selectedMaterial));
            }
        }

        private void Materials_Selected(object sender, SelectionChangedEventArgs e)
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
        private void SortComboBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void FiltrationComboBox_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void FiltrationComboBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
