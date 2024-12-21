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
        private ObservableCollection<Material> _allMaterials = new ObservableCollection<Material>();
        public ObservableCollection<Material> Materials { get; set; } = new ObservableCollection<Material>();
        public ObservableCollection<MaterialType> MaterialTypes { get; set; } = new ObservableCollection<MaterialType>();
        public MaterialsEditPage()
        {
            DataContext = this;

            var materialTypes = DBContext.GetContext().MaterialTypes.ToList();

            // Создание нового типа "Все"
            var allMaterialTypes = new MaterialType
            {
                Tittle = "Все",
                MaterialTypeID = -1
            };

            materialTypes.Insert(0, allMaterialTypes);


            MaterialTypes = new ObservableCollection<MaterialType>(materialTypes);

            var materials = DBContext.GetContext().Materials.Include(m => m.MaterialType).OrderBy(x => x.Name).ToList();
            _allMaterials = new ObservableCollection<Material>(materials);

            LoadMaterialsAsync();

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

        private void ApplyFilters()
        {
            var searchText = SearchTextBox.Text.ToLower();
            var filteredMaterials = _allMaterials.AsEnumerable();

            filteredMaterials = filteredMaterials.Where(t => t.Name.ToLower().Contains(searchText));

            if (FiltrationComboBox.SelectedItem is MaterialType selectedType)
            {
                if (selectedType.MaterialTypeID != -1)
                {
                    filteredMaterials = filteredMaterials.Where(t => t.MaterialType == selectedType);
                }
            }

            Materials.Clear();
            foreach (var tour in filteredMaterials.OrderBy(t => t.Name))
            {
                Materials.Add(tour);
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
            SortPlaceholderText.Visibility = Visibility.Collapsed;
        }
        private void SortComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SortPlaceholderText.Visibility = Visibility.Visible;
            }
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
