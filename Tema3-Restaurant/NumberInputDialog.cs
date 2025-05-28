using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows;

namespace Tema3_Restaurant
{
    public class NumberInputDialog : Window
    {
        private TextBox txtValue;
        public decimal ResultValue { get; private set; }

        public NumberInputDialog(string prompt, string title, decimal defaultValue = 1)
        {
            Title = title;
            Width = 300;
            Height = 150;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ResizeMode = ResizeMode.NoResize;

            Grid grid = new Grid();
            grid.Margin = new Thickness(10);
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            TextBlock lblPrompt = new TextBlock
            {
                Text = prompt,
                Margin = new Thickness(0, 0, 0, 5)
            };
            Grid.SetRow(lblPrompt, 0);
            grid.Children.Add(lblPrompt);

            txtValue = new TextBox
            {
                Text = defaultValue.ToString(),
                Margin = new Thickness(0, 0, 0, 15)
            };
            Grid.SetRow(txtValue, 1);
            grid.Children.Add(txtValue);

            StackPanel buttons = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            Button btnOk = new Button
            {
                Content = "OK",
                Width = 60,
                Height = 25,
                Margin = new Thickness(0, 0, 10, 0),
                IsDefault = true
            };
            btnOk.Click += BtnOk_Click;

            Button btnCancel = new Button
            {
                Content = "Cancel",
                Width = 60,
                Height = 25,
                IsCancel = true
            };

            buttons.Children.Add(btnOk);
            buttons.Children.Add(btnCancel);
            Grid.SetRow(buttons, 2);
            grid.Children.Add(buttons);

            Content = grid;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(txtValue.Text, out decimal result) && result > 0)
            {
                ResultValue = result;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid positive number.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
