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

namespace Production.Pages
{
    /// <summary>
    /// Логика взаимодействия для MaterialViewPage.xaml
    /// </summary>
    public partial class MaterialViewPage : Page
    {
        public ObservableCollection<Material> Materials { get; set; }
        public MaterialViewPage()
        {
            InitializeComponent();
            DataContext = this;
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

    }
}
