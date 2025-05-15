using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tema3_Restaurant;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void LogIn_Click(object sender, RoutedEventArgs e)
    {
        LogInWindow window = new LogInWindow();
        window.Show();
        this.Close();
    }

    private void CreateAccount_Click(object sender, RoutedEventArgs e)
    {
        CreateAccountWindow window = new CreateAccountWindow();
        window.Show();
        this.Close();
    }

    private void ContinueWithoutAccount_Click(object sender, RoutedEventArgs e)
    {
        MenuWindow1 window = new MenuWindow1();
        window.Show();
        this.Close();
    }
}