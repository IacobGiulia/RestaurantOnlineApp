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

namespace Tema3_Restaurant
{
    /// <summary>
    /// Interaction logic for MenuWindow1.xaml
    /// </summary>
    public partial class MenuWindow1 : Window
    {
        public MenuWindow1()
        {
            InitializeComponent();
            DataContext = new MenuViewModel();
        }
    }
}
