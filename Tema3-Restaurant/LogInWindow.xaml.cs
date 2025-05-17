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
using System.Windows.Shapes;
using Tema3_Restaurant.Data;
using Tema3_Restaurant.Models;


namespace Tema3_Restaurant
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        public LogInWindow()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.", "Missing Data", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            User userValid = CheckUserInDatabase(email, password);

            if (userValid != null)
            {
                if(userValid.Role == "Client")
                {
                    MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    MenuWindow2 window2 = new MenuWindow2(userValid);
                    window2.Show();
                    this.Close();
                }
                else if(userValid.Role == "Angajat")
                {
                    MessageBox.Show("Te ai conectat ca angajat!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                
            }
            else
            {
                MessageBox.Show("Invalid email or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private User CheckUserInDatabase(string email, string password)
        {
            using (var context = new RestaurantContext())
            {
                return context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            }
        }
    }
}
