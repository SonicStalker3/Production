using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Data.Entity;
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
using static System.Net.Mime.MediaTypeNames;

namespace Production.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        ProductionEntities _context = DBContext.GetContext();
        public LoginPage()
        {
            InitializeComponent();
        }
        public void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            string username = UserName.Text;
            string password = UserPassword.Password;

            User user = AuthenticateUser(username, password);
            if (user != null)
            {
                // Выдача привилегий
                //AssignPrivileges(user);

                if (user.RoleID != 2)
                {
                    NavigationService.Navigate(new EditViewChoicePage(user));
                }
                else
                {
                    NavigationService.Navigate(new MaterialsViewPage());
                }
            }
            else
            {
                MessageBox.Show("Неверное имя пользователя или пароль.");
            }
        }

        private User AuthenticateUser(string username, string password)
        {
            var user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.UserName == username);
            if (user != null && user.PasswordHash == HashPassword(password))
            {
                return user;
            }
            //Clipboard.SetText(HashPassword(password));

            return null;
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
