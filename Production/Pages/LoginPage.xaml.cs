using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace Production.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public HotelEntities _context = HotelEntities.GetContext();
        public LoginPage()
        {
            InitializeComponent();
        }
        public void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            string username = UserName.Text; // Предполагается, что у вас есть TextBox для имени пользователя
            string password = UserPassword.Password; // Предполагается, что у вас есть PasswordBox для пароля

            Пользователи user = AuthenticateUser(username, password);
            if (user != null)
            {
                // Выдача привилегий
                //AssignPrivileges(user);

                // Получаем ссылку на главное окно
                /*                MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                                if (mainWindow != null)
                                {
                                    mainWindow.SetUser(user); // Передаем пользователя в главное окно
                                }*/

                if (user.RoleID != 0)
                {
                    NavigationService.Navigate(new EditViewChoicePage(user));
                }
                else
                {
                    NavigationService.Navigate(new MaterialViewPage());
                }

                //NavigationService.Navigate(new HotelPage());
            }
            else
            {
                MessageBox.Show("Неверное имя пользователя или пароль.");
            }
        }

        private Пользователи AuthenticateUser(string username, string password)
        {

            return _context.Пользователи.Include(u => u.Роли).FirstOrDefault(u => u.ИмяПользователя == username && u.Пароль == password); //.Include(u => u.RoleID)
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
