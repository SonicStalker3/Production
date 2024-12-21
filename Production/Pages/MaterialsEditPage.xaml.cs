﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
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

namespace Production.Pages
{
    /// <summary>
    /// Логика взаимодействия для MaterialsEditPage.xaml
    /// </summary>
    public partial class MaterialsEditPage : Page
    {
        ProductionEntities _context = DBContext.GetContext();
        public ObservableCollection<Material> Materials { get; set; }

        public MaterialsEditPage()
        {
            this.DataContext = this;

            Materials = new ObservableCollection<Material>();
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
            if (sender is System.Windows.Controls.Image image &&
                (image.Source == null ||
                string.IsNullOrWhiteSpace((image.Source as BitmapImage).UriSource?.ToString()
                )))
            {
                BitmapImage errorImage = BitmapToBitmapImage(Properties.Resources.ErrorImage);
                image.Source = errorImage;
            }
        }

        public BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
        {
            using (var stream = new MemoryStream(byteArray))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad; // Загружает изображение в память
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // Замораживает объект для использования в других потоках
                return bitmapImage;
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
    }
}
