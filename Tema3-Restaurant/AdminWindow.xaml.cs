using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Tema3_Restaurant.Data;
using Tema3_Restaurant.Models;
using Tema3_Restaurant.ViewModels;

namespace Tema3_Restaurant
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private RestaurantContext _context;
        private readonly User _currentUser;
        private ObservableCollection<ActiveOrderViewModel> _activeOrders;
        private ObservableCollection<LowStockProductViewModel> _lowStockProducts;
        private decimal _lowStockThreshold;

        public AdminWindow(User user)
        {
            InitializeComponent();

            _currentUser = user;
            _activeOrders = new ObservableCollection<ActiveOrderViewModel>();
            _lowStockProducts = new ObservableCollection<LowStockProductViewModel>();

            TxtEmployeeName.Text = $"Welcome, {_currentUser.FirstName} {_currentUser.LastName}";

            InitializeContext();

            try
            {
                LoadConfiguration();
                LoadActiveOrders();
                LoadLowStockProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            LvActiveOrders.ItemsSource = _activeOrders;
            LvLowStockProducts.ItemsSource = _lowStockProducts;
        }

        private void InitializeContext()
        {
            _context?.Dispose();

            _context = new RestaurantContext();
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

            try
            {
                using (var orderContext = new RestaurantContext())
                {
                    using (var conn = orderContext.Database.GetDbConnection())
                    {
                        conn.Open();
                        using (var command = conn.CreateCommand())
                        {
                            command.CommandText = "GetActiveOrdersSorted";
                            command.CommandType = CommandType.StoredProcedure;

                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading active orders: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadLowStockProducts()
        {
            _lowStockProducts.Clear();

            try
            {
                using (var stockContext = new RestaurantContext())
                {
                    if (stockContext.Database.CanConnect())
                    {
                        using (var conn = stockContext.Database.GetDbConnection())
                        {
                            conn.Open();
                            using (var command = conn.CreateCommand())
                            {
                                command.CommandText = "GetLowStockProducts";
                                command.CommandType = CommandType.StoredProcedure;

                                var param = command.CreateParameter();
                                param.ParameterName = "@ThresholdQuantity";
                                param.Value = _lowStockThreshold;
                                command.Parameters.Add(param);

                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        _lowStockProducts.Add(new LowStockProductViewModel
                                        {
                                            Name = reader.GetString(reader.GetOrdinal("Name")),
                                            TotalQuantity = reader.GetDecimal(reader.GetOrdinal("TotalQuantity"))
                                        });
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cannot connect to database. Please check your connection string.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading low stock products: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnManageCategories_Click(object sender, RoutedEventArgs e)
        {
            var categoryWindow = new CategoryManagementWindow();
            categoryWindow.ShowDialog();

            RefreshData();
        }

        private void BtnManageProducts_Click(object sender, RoutedEventArgs e)
        {
            var productWindow = new ProductManagementWindow();
            productWindow.ShowDialog();

            RefreshData();
        }

        private void BtnManageMenus_Click(object sender, RoutedEventArgs e)
        {
            var menuWindow = new MenuManagementWindow();
            menuWindow.ShowDialog();

            RefreshData();
        }

        private void BtnManageAllergens_Click(object sender, RoutedEventArgs e)
        {
            var allergenWindow = new AllergenManagementWindow();
            allergenWindow.ShowDialog();

            RefreshData();
        }

        private void BtnViewAllOrders_Click(object sender, RoutedEventArgs e)
        {
            var allOrdersWindow = new AllOrdersWindow();
            allOrdersWindow.ShowDialog();

            RefreshData();
        }

        private void BtnViewOrderDetails_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = LvActiveOrders.SelectedItem as ActiveOrderViewModel;
            if (selectedOrder == null)
            {
                MessageBox.Show("Please select an order to view details.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var orderDetailWindow = new OrderDetailWindow(selectedOrder.ID);
            orderDetailWindow.ShowDialog();

            RefreshData();
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
                RefreshData();
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            try
            {
                
                InitializeContext();

                LoadConfiguration();
                LoadActiveOrders();
                LoadLowStockProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing data: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _context?.Dispose();
        }
    }
}