using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Windows.Shapes;
using Tema3_Restaurant.Data;
using Tema3_Restaurant.Models;
using System.Data;
using Tema3_Restaurant.ViewModels;

namespace Tema3_Restaurant
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private readonly RestaurantContext _context;
        private readonly User _currentUser;
        private ObservableCollection<ActiveOrderViewModel> _activeOrders;
        private ObservableCollection<LowStockProductViewModel> _lowStockProducts;
        private decimal _lowStockThreshold;
        public AdminWindow(User user)
        {
            InitializeComponent();
            _context = new RestaurantContext();
            _currentUser = user;
            _activeOrders = new ObservableCollection<ActiveOrderViewModel>();
            _lowStockProducts = new ObservableCollection<LowStockProductViewModel>();

            TxtEmployeeName.Text = $"Welcome, {_currentUser.FirstName} {_currentUser.LastName}";

            LoadConfiguration();
            LoadActiveOrders();
            LoadLowStockProducts();

            LvActiveOrders.ItemsSource = _activeOrders;
            LvLowStockProducts.ItemsSource = _lowStockProducts;
        }

        private void LoadConfiguration()
        {
            var lowStockConfig = _context.ConfigurationApp.FirstOrDefault(c => c.Key == "PragMinimStoc");
            if (lowStockConfig != null && decimal.TryParse(lowStockConfig.Value, out decimal threshold))
            {
                _lowStockThreshold = threshold;
            }
            else
            {
                _lowStockThreshold = 1500;
            }
        }

        private void LoadActiveOrders()
        {
            _activeOrders.Clear();
            using (var conn = _context.Database.GetDbConnection())
            {
                conn.Open();
                using(var command = conn.CreateCommand())
                {
                    command.CommandText = "GetActiveOrdersSorted";
                    command.CommandType = CommandType.StoredProcedure;

                    using(var reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            var order = new ActiveOrderViewModel
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                DateAndTime = reader.GetDateTime(reader.GetOrdinal("DateAndTime")),
                                Status = reader.GetString(reader.GetOrdinal("State")),
                                UniqueCode = reader.GetString(reader.GetOrdinal("UniqueCode")),
                                ProductsPrice = reader.GetDecimal(reader.GetOrdinal("ProductsPrice")),
                                DeliveryPrice = reader.GetDecimal(reader.GetOrdinal("DeliveryPrice")),
                                TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                Address = reader.GetString(reader.GetOrdinal("Address"))

                            };
                            if (!reader.IsDBNull(reader.GetOrdinal("EstimatedDeliveryTime")))
                            {
                                order.EstimatedDeliveryTime = reader.GetDateTime(reader.GetOrdinal("EstimatedDeliveryTime"));
                            }
                            _activeOrders.Add(order);

                        }    
                    }
                }
            }
        }

        private void LoadLowStockProducts()
        {
            _lowStockProducts.Clear();
            using(var conn = _context.Database.GetDbConnection())
            {
                conn.Open();
                using(var command = conn.CreateCommand())
                {
                    command.CommandText = "GetLowStockProducts";
                    command.CommandType = CommandType.StoredProcedure;

                    var param = command.CreateParameter();
                    param.ParameterName = "@ThresholdQuantity";
                    param.Value = _lowStockThreshold;
                    command.Parameters.Add(param);

                    using(var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _lowStockProducts.Add(new LowStockProductViewModel{
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                TotalQuantity = reader.GetDecimal(reader.GetOrdinal("TotalQuantity"))
                            });
                        }
                    }
                }
            }
        }

        private void BtnManageCategories_Click(object sender, RoutedEventArgs e)
        {
            var categoryWindow = new CategoryManagementWindow();
            categoryWindow.ShowDialog();

            LoadActiveOrders();
            LoadLowStockProducts();
        }
        private void BtnManageProducts_Click(object sender, RoutedEventArgs e)
        {
            var productWindow = new ProductManagementWindow();
            productWindow.ShowDialog();

            LoadActiveOrders();
            LoadLowStockProducts();
        }


        private void BtnManageMenus_Click(object sender, RoutedEventArgs e)
        {
            var menuWindow = new MenuManagementWindow();
            menuWindow.ShowDialog();

            LoadActiveOrders();
            LoadLowStockProducts();
        }


        private void BtnManageAllergens_Click(object sender, RoutedEventArgs e)
        {
            var allergenWindow = new AllergenManagementWindow();
            allergenWindow.ShowDialog();

            LoadActiveOrders();
            LoadLowStockProducts();
        }


        private void BtnViewAllOrders_Click(object sender, RoutedEventArgs e)
        {
            var allOrdersWindow = new AllOrdersWindow();
            allOrdersWindow.ShowDialog();
        }

        private void BtnViewOrderDetails_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = LvActiveOrders.SelectedItem as ActiveOrderViewModel;
            if(selectedOrder == null)
            {
                MessageBox.Show("Please select an order to view details.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var orderDetailWindow = new OrderDetailWindow(selectedOrder.ID);
            orderDetailWindow.ShowDialog();
        }

        private void BtnUpdateOrderStatus_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = LvActiveOrders.SelectedItem as ActiveOrderViewModel;
            if (selectedOrder == null)
            {
                MessageBox.Show("Please select an order to update.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var updateStatusWindow = new UpdateOrderStatusWindow(selectedOrder);
            if (updateStatusWindow.ShowDialog() == true)
            {
                // Refresh after status update
                LoadActiveOrders();
            }
        }

        private void BtnRefresh_Click(object sedner, RoutedEventArgs e)
        {
            LoadActiveOrders();
            LoadLowStockProducts();
        }
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
