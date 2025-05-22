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
using System.Collections.ObjectModel;
using Tema3_Restaurant.Data;
using Tema3_Restaurant.Models;
using Tema3_Restaurant.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Tema3_Restaurant
{
    /// <summary>
    /// Interaction logic for OrderDetailWindow.xaml
    /// </summary>
    public partial class OrderDetailWindow : Window
    {
        private readonly RestaurantContext _context;
        private readonly int _orderId;
        private Order _order;
        private ObservableCollection<OrderItemViewModel> _orderItems;

        public OrderDetailWindow(int orderID)
        {
            InitializeComponent();

            _context = new RestaurantContext();
            _orderId = orderID;
            _orderItems = new ObservableCollection<OrderItemViewModel>();

            LoadOrderDetails();
            LoadOrderItems();

            LvOrderItems.ItemsSource = _orderItems;
        }

        private void LoadOrderDetails()
        {
            try
            {
                // Get order with user information
                _order = _context.Orders
                    .Where(o => o.ID == _orderId)
                    .Select(o => new Order
                    {
                        ID = o.ID,
                        UserID = o.UserID,
                        DateAndTime = o.DateAndTime,
                        State = o.State,
                        ProductsPrice = o.ProductsPrice,
                        DeliveryPrice = o.DeliveryPrice,
                        Discount = o.Discount,
                        TotalPrice = o.TotalPrice,
                        UniqueCode = o.UniqueCode,
                        EstimatedDeliveryTime = o.EstimatedDeliveryTime,
                        User = new User
                        {
                            ID = o.User.ID,
                            FirstName = o.User.FirstName,
                            LastName = o.User.LastName,
                            Email = o.User.Email,
                            Phone = o.User.Phone,
                            Address = o.User.Address
                        }
                    })
                    .FirstOrDefault();

                if (_order == null)
                {
                    MessageBox.Show("Order not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                    return;
                }

                // Populate the UI with order details
                TxtOrderDate.Text = _order.DateAndTime.ToString("dd/MM/yyyy HH:mm");
                TxtOrderCode.Text = _order.UniqueCode;
                TxtOrderStatus.Text = _order.State;
                TxtEstimatedDelivery.Text = _order.EstimatedDeliveryTime.HasValue
                    ? _order.EstimatedDeliveryTime.Value.ToString("dd/MM/yyyy HH:mm")
                    : "Not set";

                TxtProductsPrice.Text = _order.ProductsPrice.ToString("C");
                TxtDeliveryPrice.Text = _order.DeliveryPrice.ToString("C");
                TxtDiscount.Text = _order.Discount.ToString("C");
                TxtTotalPrice.Text = _order.TotalPrice.ToString("C");

                TxtCustomerName.Text = $"{_order.User.FirstName} {_order.User.LastName}";
                TxtCustomerPhone.Text = _order.User.Phone;
                TxtCustomerAddress.Text = _order.User.Address;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading order details: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void LoadOrderItems()
        {
            _orderItems.Clear();

            try
            {
                using (var conn = _context.Database.GetDbConnection())
                {
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = "GetOrderDetails";
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameter
                        var param = command.CreateParameter();
                        param.ParameterName = "@OrderID";
                        param.Value = _orderId;
                        command.Parameters.Add(param);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var orderItem = new OrderItemViewModel
                                {
                                    ItemType = reader.GetString(reader.GetOrdinal("ItemType")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice"))
                                };

                                _orderItems.Add(orderItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading order items: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            // Skip if order is completed or canceled
            if (_order.State == "Delivered" || _order.State == "Canceled")
            {
                MessageBox.Show("Cannot update status for delivered or canceled orders.",
                    "Status Update", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Create view model for the order (used by update status window)
            var orderVM = new ActiveOrderViewModel
            {
                ID = _order.ID,
                DateAndTime = _order.DateAndTime,
                Status = _order.State,
                UniqueCode = _order.UniqueCode,
                ProductsPrice = _order.ProductsPrice,
                DeliveryPrice = _order.DeliveryPrice,
                TotalPrice = _order.TotalPrice,
                EstimatedDeliveryTime = _order.EstimatedDeliveryTime,
                FirstName = _order.User.FirstName,
                LastName = _order.User.LastName,
                Phone = _order.User.Phone,
                Address = _order.User.Address
            };

            var updateStatusWindow = new UpdateOrderStatusWindow(orderVM);
            if (updateStatusWindow.ShowDialog() == true)
            {
                // Refresh order details after status update
                LoadOrderDetails();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }


}

