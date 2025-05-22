using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Tema3_Restaurant.ViewModels;
using Microsoft.EntityFrameworkCore;
using Tema3_Restaurant.Models;
using System.Data;

namespace Tema3_Restaurant
{
    /// <summary>
    /// Interaction logic for AllOrdersWindow.xaml
    /// </summary>
    public partial class AllOrdersWindow : Window
    {
        private readonly RestaurantContext _context;
        private ObservableCollection<AllOrderViewModel> _allOrders;


        public AllOrdersWindow()
        {
            InitializeComponent();
            var options = new DbContextOptionsBuilder<RestaurantContext>()
    .UseSqlServer(@"Server=localhost;Database=RestaurantDB;Trusted_Connection=True;TrustServerCertificate=True;")
    .Options;
            _context = new RestaurantContext(options);
            _allOrders = new ObservableCollection<AllOrderViewModel>();

            LoadAllOrders();
            LvAllOrders.ItemsSource = _allOrders;
        }

        private void LoadAllOrders()
        {
            _allOrders.Clear();
            using (var conn = _context.Database.GetDbConnection())
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "GetAllOrdersSorted";
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var order = new AllOrderViewModel
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

                            _allOrders.Add(order);
                        }
                    }
                }
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadAllOrders();
        }

        private void BtnViewDetails_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = LvAllOrders.SelectedItem as AllOrderViewModel;
            if (selectedOrder == null)
            {
                MessageBox.Show("Please select an order to view details.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var orderDetailWindow = new OrderDetailWindow(selectedOrder.ID);
            orderDetailWindow.ShowDialog();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
