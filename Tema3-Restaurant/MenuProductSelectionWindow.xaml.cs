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
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using Tema3_Restaurant.Models;

namespace Tema3_Restaurant
{
    /// <summary>
    /// Interaction logic for MenuProductSelectionWindow.xaml
    /// </summary>
    public partial class MenuProductSelectionWindow : Window
    {
        private readonly RestaurantContext _context;
        private ObservableCollection<Product> _products;
        public ObservableCollection<MenuProduct> SelectedProducts { get; private set; }
        public MenuProductSelectionWindow(ObservableCollection<MenuProduct> existingProducts)
        {
            InitializeComponent();

            _context = new RestaurantContext();
            SelectedProducts = new ObservableCollection<MenuProduct>();
            _products = new ObservableCollection<Product>();

            // Copy existing products to our collection
            foreach (var mp in existingProducts)
            {
                SelectedProducts.Add(new MenuProduct
                {
                    ProductID = mp.ProductID,
                    Product = mp.Product,
                    Quantity = mp.Quantity
                });
            }

            LoadProducts();
            LvProducts.ItemsSource = _products;
            LvSelectedProducts.ItemsSource = SelectedProducts;
        }

        private void LoadProducts()
        {
            _products.Clear();
            var products = _context.Products
                .Include(p => p.Category)
                .Where(p => p.Available)
                .OrderBy(p => p.Category.Name)
                .ThenBy(p => p.Name)
                .ToList();

            foreach (var product in products)
            {
                _products.Add(product);
            }
        }

        private void BtnAddToMenu_Click(object sender, RoutedEventArgs e)
        {
            if (LvProducts.SelectedItem is Product selectedProduct)
            {
                // Check if product is already in the menu
                var existingProduct = SelectedProducts.FirstOrDefault(mp => mp.ProductID == selectedProduct.ID);
                if (existingProduct != null)
                {
                    MessageBox.Show("This product is already in the menu. You can adjust its quantity.",
                        "Product Already Added", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Show quantity dialog
                var quantityDialog = new NumberInputDialog("Enter quantity:", "Add Product to Menu", selectedProduct.PortionQuantity);
                if (quantityDialog.ShowDialog() == true)
                {
                    decimal quantity = quantityDialog.ResultValue;

                    // Add the product to selected products
                    SelectedProducts.Add(new MenuProduct
                    {
                        ProductID = selectedProduct.ID,
                        Product = selectedProduct,
                        Quantity = quantity
                    });
                }
            }
            else
            {
                MessageBox.Show("Please select a product to add.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnRemoveFromMenu_Click(object sender, RoutedEventArgs e)
        {
            if (LvSelectedProducts.SelectedItem is MenuProduct selectedMenuProduct)
            {
                SelectedProducts.Remove(selectedMenuProduct);
            }
            else
            {
                MessageBox.Show("Please select a product to remove.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnUpdateQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (LvSelectedProducts.SelectedItem is MenuProduct selectedMenuProduct)
            {
                var quantityDialog = new NumberInputDialog("Enter new quantity:", "Update Quantity", selectedMenuProduct.Quantity);
                if (quantityDialog.ShowDialog() == true)
                {
                    decimal newQuantity = quantityDialog.ResultValue;

                    // Update the quantity
                    int index = SelectedProducts.IndexOf(selectedMenuProduct);
                    SelectedProducts[index].Quantity = newQuantity;

                    // Refresh the list view
                    LvSelectedProducts.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Please select a product to update.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
