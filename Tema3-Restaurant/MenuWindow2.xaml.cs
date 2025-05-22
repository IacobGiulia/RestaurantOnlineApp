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
using Tema3_Restaurant.ViewModels;
using Tema3_Restaurant.Models;

namespace Tema3_Restaurant
{
    /// <summary>
    /// Interaction logic for MenuWindow2.xaml
    /// </summary>
    public partial class MenuWindow2 : Window
    {
        private readonly OrderViewModel _orderViewModel;
        private readonly User _currentUser;
        public MenuWindow2(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            _orderViewModel = new OrderViewModel(_currentUser);

            DataContext = _orderViewModel;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
