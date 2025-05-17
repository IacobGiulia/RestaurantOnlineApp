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
    /// Interaction logic for CreateAccountWindow.xaml
    /// </summary>
    public partial class CreateAccountWindow : Window
    {
        public CreateAccountWindow()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
            string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
            string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
            string.IsNullOrWhiteSpace(PhoneTextBox.Text) ||
            string.IsNullOrWhiteSpace(AddressTextBox.Text) ||
            string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("You must fill all the spaces!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValidPhoneNumber(PhoneTextBox.Text))
            {
                MessageBox.Show("The phone number is invalid!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var context = new RestaurantContext())
            {
                if (context.Users.Any(u => u.Email == EmailTextBox.Text))
                {
                    MessageBox.Show("There is an other account registered with this email!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                User newUser = new User
                {
                    FirstName = FirstNameTextBox.Text,
                    LastName = LastNameTextBox.Text,
                    Email = EmailTextBox.Text,
                    Phone = PhoneTextBox.Text,
                    Address = AddressTextBox.Text,
                    Password = PasswordBox.Password,
                    Role = "Client"
                };

                context.Users.Add(newUser);
                context.SaveChanges();

                MessageBox.Show("Account successfully created!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);


            }
        }

            private bool IsValidPhoneNumber(string phoneNumber)
        {

            return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^[0-9]{11}$");
        }
    }
    
 }


