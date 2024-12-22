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
                    /*
                    EditHotelBtn.IsEnabled = true;
                    EditTourBtn.IsEnabled = true;
                    ViewTourBtn.IsEnabled = true;
                    */
                    EditMareialsBtn.Visibility = Visibility.Visible;
                    EditSuppliersBtn.Visibility = Visibility.Visible;
                    ViewMaterialsBtn.Visibility = Visibility.Visible;
                    ViewSuppliersBtn.Visibility = Visibility.Visible;
                    break;
                case 1:
                    EditMareialsBtn.Visibility = Visibility.Collapsed;
                    EditSuppliersBtn.Visibility = Visibility.Collapsed;
                    ViewMaterialsBtn.Visibility = Visibility.Visible;
                    ViewSuppliersBtn.Visibility = Visibility.Visible;
                    /*
                       EditHotelBtn.IsEnabled = false;
                       EditTourBtn.IsEnabled = false;
                       ViewTourBtn.IsEnabled = true;
                    */
                    break;
                case 2:
                    EditMareialsBtn.Visibility = Visibility.Visible;
                    EditSuppliersBtn.Visibility = Visibility.Collapsed;
                    ViewMaterialsBtn.Visibility = Visibility.Visible;
                    ViewSuppliersBtn.Visibility = Visibility.Visible;
                    /*                  
                    EditHotelBtn.IsEnabled = true;
                    EditTourBtn.IsEnabled = false;
                    ViewTourBtn.IsEnabled = true;
                    */
                    break;
                default:
                    EditMareialsBtn.Visibility = Visibility.Visible;
                    EditSuppliersBtn.Visibility = Visibility.Collapsed;
                    ViewMaterialsBtn.Visibility = Visibility.Visible;
                    ViewSuppliersBtn.Visibility = Visibility.Visible;
                    /*
                    EditHotelBtn.IsEnabled = true;
                    EditTourBtn.IsEnabled = false;
                    ViewTourBtn.IsEnabled = true;
                    */
                    break;
            }
        }

        private void EditMaterialsBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MaterialsEditPage());
        }

        private void EditSuppliersBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SuppliersEditPage());
        }

        private void ViewMaterialsBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MaterialsViewPage());
        }
        private void ViewSuppliersBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SuppliersViewPage());
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
