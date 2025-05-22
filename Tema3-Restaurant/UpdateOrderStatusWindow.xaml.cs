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
using System.Text.RegularExpressions;
using Tema3_Restaurant.Models;
using Tema3_Restaurant.Data;
using Tema3_Restaurant.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Tema3_Restaurant
{
    /// <summary>
    /// Interaction logic for UpdateOrderStatusWindow.xaml
    /// </summary>
    public partial class UpdateOrderStatusWindow : Window
    {
        private readonly RestaurantContext _context;
        private readonly ActiveOrderViewModel _order;
        private DateTime? _selectedDateTime = null;
        public UpdateOrderStatusWindow(ActiveOrderViewModel order)
        {
            InitializeComponent();
            _context = new RestaurantContext();
            _order = order;

            TxtOrderCode.Text = _order.UniqueCode;
            TxtCurrentStatus.Text = _order.Status;

            foreach (ComboBoxItem item in CmbNewStatus.Items)
            {
                if (item.Content.ToString() == _order.Status)
                {
                    CmbNewStatus.SelectedItem = item;
                    break;
                }
            }

            if (_order.EstimatedDeliveryTime.HasValue)
            {
                DpEstimatedDeliveryDate.SelectedDate = _order.EstimatedDeliveryTime.Value.Date;
                TxtEstimatedDeliveryHour.Text = _order.EstimatedDeliveryTime.Value.Hour.ToString("D2");
                TxtEstimatedDeliveryMinute.Text = _order.EstimatedDeliveryTime.Value.Minute.ToString("D2");
                _selectedDateTime = _order.EstimatedDeliveryTime.Value;
            }
            else
            {

                var defaultTime = DateTime.Now.AddMinutes(30);
                DpEstimatedDeliveryDate.SelectedDate = defaultTime.Date;
                TxtEstimatedDeliveryHour.Text = defaultTime.Hour.ToString("D2");
                TxtEstimatedDeliveryMinute.Text = defaultTime.Minute.ToString("D2");
                _selectedDateTime = defaultTime;
            }
        }
            private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
            {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            }

        private void DpEstimatedDeliveryDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSelectedDateTime();
        }

        private void CmbNewStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If status is "In Delivery" and no delivery time is set, set default time to now + 30 min
            if (CmbNewStatus.SelectedItem != null &&
                ((ComboBoxItem)CmbNewStatus.SelectedItem).Content.ToString() == "In Delivery" &&
                !_selectedDateTime.HasValue)
            {
                var defaultTime = DateTime.Now.AddMinutes(30);
                DpEstimatedDeliveryDate.SelectedDate = defaultTime.Date;
                TxtEstimatedDeliveryHour.Text = defaultTime.Hour.ToString("D2");
                TxtEstimatedDeliveryMinute.Text = defaultTime.Minute.ToString("D2");
                _selectedDateTime = defaultTime;
            }
        }

        private void UpdateSelectedDateTime()
        {
            if (DpEstimatedDeliveryDate.SelectedDate.HasValue)
            {
                if (int.TryParse(TxtEstimatedDeliveryHour.Text, out int hour) &&
                    int.TryParse(TxtEstimatedDeliveryMinute.Text, out int minute))
                {
                    // Validate hour and minute values
                    hour = Math.Min(Math.Max(hour, 0), 23);
                    minute = Math.Min(Math.Max(minute, 0), 59);

                    TxtEstimatedDeliveryHour.Text = hour.ToString("D2");
                    TxtEstimatedDeliveryMinute.Text = minute.ToString("D2");

                    try
                    {
                        _selectedDateTime = new DateTime(
                            DpEstimatedDeliveryDate.SelectedDate.Value.Year,
                            DpEstimatedDeliveryDate.SelectedDate.Value.Month,
                            DpEstimatedDeliveryDate.SelectedDate.Value.Day,
                            hour, minute, 0);
                    }
                    catch (Exception)
                    {
                        _selectedDateTime = null;
                    }
                }
            }
            else
            {
                _selectedDateTime = null;
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (CmbNewStatus.SelectedItem == null)
            {
                MessageBox.Show("Please select a new status.", "Selection Required",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string newStatus = ((ComboBoxItem)CmbNewStatus.SelectedItem).Content.ToString();

            // Confirm when canceling order
            if (newStatus == "Canceled" &&
                MessageBox.Show("Are you sure you want to cancel this order?", "Confirm Cancellation",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            // Update time value before submission
            UpdateSelectedDateTime();

            try
            {
                using (var conn = _context.Database.GetDbConnection())
                {
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = "UpdateOrderStatus";
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        var orderIdParam = command.CreateParameter();
                        orderIdParam.ParameterName = "@OrderID";
                        orderIdParam.Value = _order.ID;
                        command.Parameters.Add(orderIdParam);

                        var statusParam = command.CreateParameter();
                        statusParam.ParameterName = "@NewStatus";
                        statusParam.Value = newStatus;
                        command.Parameters.Add(statusParam);

                        var deliveryTimeParam = command.CreateParameter();
                        deliveryTimeParam.ParameterName = "@EstimatedDeliveryTime";

                        if (_selectedDateTime.HasValue)
                        {
                            deliveryTimeParam.Value = _selectedDateTime.Value;
                        }
                        else
                        {
                            deliveryTimeParam.Value = DBNull.Value;
                        }
                        command.Parameters.Add(deliveryTimeParam);

                        // Execute procedure
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show($"Order status updated to '{newStatus}'.", "Update Successful",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating order status: {ex.Message}", "Update Failed",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
    }

