using System;
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
using Production.Utils;

namespace Production
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string PageTitle { get; set; }
        public MainWindow()
        {
            //ImportImages.Start();
            InitializeComponent();
        }

        /*private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            var frame = sender as Frame;
            if (frame != null && frame.Content is Page pageWithTitle)
            {
                //MessageBox.Show();
                PageTitle = pageWithTitle.Title;
            }
        }*/
    }
}
