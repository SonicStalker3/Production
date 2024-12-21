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
using Production.DB;

namespace Production.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditViewChoicePage.xaml
    /// </summary>
    public partial class EditViewChoicePage : Page
    {
        public User user;

        public EditViewChoicePage(User user)
        {
            InitializeComponent();
            switch (user.RoleID)
            {
                case 0:
                    EditHotelBtn.IsEnabled = true;
                    EditTourBtn.IsEnabled = true;
                    ViewTourBtn.IsEnabled = true;
                    break;
                case 1:
                    EditHotelBtn.IsEnabled = false;
                    EditTourBtn.IsEnabled = false;
                    ViewTourBtn.IsEnabled = true;
                    break;
                case 2:
                    EditHotelBtn.IsEnabled = true;
                    EditTourBtn.IsEnabled = false;
                    ViewTourBtn.IsEnabled = true;
                    break;
                default:
                    EditHotelBtn.IsEnabled = true;
                    EditTourBtn.IsEnabled = false;
                    ViewTourBtn.IsEnabled = true;
                    break;
            }
        }

        private void EditHotelBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MaterialsEditPage());
        }

        private void EditTourBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SuppliersEditPage());
        }

        private void ViewTourBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MaterialViewPage());
        }
        private void EditUsersBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
