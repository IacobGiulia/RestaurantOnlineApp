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
using Microsoft.EntityFrameworkCore;
using Tema3_Restaurant.Data;
using Tema3_Restaurant.Models;

namespace Tema3_Restaurant
{
    /// <summary>
    /// Interaction logic for MenuManagementWindow.xaml
    /// </summary>
    public partial class MenuManagementWindow : Window
    {
        private readonly RestaurantContext _context;
        private Tema3_Restaurant.Models.Menu _currentMenu;
        private ObservableCollection<MenuProduct> _menuProducts;
        private bool _isNewMenu = false;
        public MenuManagementWindow()
        {
            InitializeComponent();

            InitializeComponent();
            _context = new RestaurantContext();
            _menuProducts = new ObservableCollection<MenuProduct>();

            LoadCategories();
            LoadMenus();
            ClearMenuDetails();

            LvMenuProducts.ItemsSource = _menuProducts;
        }

        private void LoadCategories()
        {
            var categories = _context.Categories.ToList();
            CmbCategory.ItemsSource = categories;
        }

        private void LoadMenus()
        {
            var menus = _context.Menus
                .Include(m => m.Category)
                .Include(m => m.MenuProducts)
                    .ThenInclude(mp => mp.Product)
                .ToList();

            LvMenus.ItemsSource = menus;
        }

        private void ClearMenuDetails()
        {
            _currentMenu = null;
            _isNewMenu = false;
            TxtMenuName.Text = string.Empty;
            CmbCategory.SelectedIndex = -1;
            ChkAvailable.IsChecked = true;
            _menuProducts.Clear();
            UpdateTotalPrice();
            BtnManageProducts.IsEnabled = false;
        }

        private void UpdateTotalPrice()
        {
            decimal totalPrice = 0;
            if (_menuProducts.Count > 0)
            {
                foreach (var mp in _menuProducts)
                {
                    totalPrice += mp.Product.Price * (mp.Quantity / mp.Product.PortionQuantity);
                }

                // Apply menu discount if configured
                var configuration = _context.ConfigurationApp
                    .FirstOrDefault(c => c.Key == "ProcentReducereMeniu");

                if (configuration != null && decimal.TryParse(configuration.Value, out decimal discountPercentage))
                {
                    totalPrice = totalPrice * (1 - (discountPercentage / 100));
                }
            }

            TxtTotalPrice.Text = $"{Math.Round(totalPrice, 2):C}";
        }

        private void LvMenus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LvMenus.SelectedItem is Tema3_Restaurant.Models.Menu selectedMenu)
            {
                _currentMenu = selectedMenu;
                _isNewMenu = false;

                TxtMenuName.Text = selectedMenu.Name;
                CmbCategory.SelectedItem = selectedMenu.Category;
                ChkAvailable.IsChecked = selectedMenu.Available;

                _menuProducts.Clear();
                foreach (var mp in selectedMenu.MenuProducts)
                {
                    _menuProducts.Add(mp);
                }

                UpdateTotalPrice();
                BtnManageProducts.IsEnabled = true;
            }
        }

        private void BtnAddMenu_Click(object sender, RoutedEventArgs e)
        {
            ClearMenuDetails();
            _isNewMenu = true;
            _currentMenu = new Tema3_Restaurant.Models.Menu();
            BtnManageProducts.IsEnabled = true;
        }

        private void BtnDeleteMenu_Click(object sender, RoutedEventArgs e)
        {
            if (_currentMenu == null || _isNewMenu)
            {
                MessageBox.Show("Please select a menu to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete the menu '{_currentMenu.Name}'?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Remove all menu products first
                    var menuProductsToRemove = _context.MenuProducts.Where(mp => mp.MenuID == _currentMenu.ID).ToList();
                    _context.MenuProducts.RemoveRange(menuProductsToRemove);

                    // Remove the menu
                    _context.Menus.Remove(_currentMenu);
                    _context.SaveChanges();

                    LoadMenus();
                    ClearMenuDetails();
                    MessageBox.Show("Menu deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting menu: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnManageProducts_Click(object sender, RoutedEventArgs e)
        {
            if (_currentMenu == null)
            {
                MessageBox.Show("Please create or select a menu first.", "No Menu Selected",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (_isNewMenu)
            {
                _currentMenu.Name = TxtMenuName.Text;
            }

            // Save the menu first if it's new
            if (_isNewMenu && string.IsNullOrWhiteSpace(_currentMenu.Name))
            {
                MessageBox.Show("Please enter a menu name before adding products.", "Missing Information",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var productSelectionWindow = new MenuProductSelectionWindow(_menuProducts);
            if (productSelectionWindow.ShowDialog() == true)
            {
                // Clear existing menu products
                _menuProducts.Clear();

                // Add all selected products
                foreach (var mp in productSelectionWindow.SelectedProducts)
                {
                    _menuProducts.Add(mp);
                }

                UpdateTotalPrice();
            }
        }

        private void BtnSaveMenu_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtMenuName.Text))
            {
                MessageBox.Show("Please enter a menu name.", "Missing Information",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (CmbCategory.SelectedItem == null)
            {
                MessageBox.Show("Please select a category.", "Missing Information",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (_menuProducts.Count == 0)
            {
                MessageBox.Show("Please add at least one product to the menu.", "Missing Information",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                if (_isNewMenu)
                {
                    // Create a new menu
                    _currentMenu = new Tema3_Restaurant.Models.Menu
                    {
                        Name = TxtMenuName.Text,
                        CategoryID = ((Category)CmbCategory.SelectedItem).ID,
                        Available = ChkAvailable.IsChecked ?? true
                    };

                    _context.Menus.Add(_currentMenu);
                    _context.SaveChanges(); // Salvăm mai întâi meniul pentru a obține ID-ul

                    // Add menu products
                    foreach (var mp in _menuProducts)
                    {
                        // Creăm instanțe noi de MenuProduct pentru a evita probleme de tracking
                        var newMenuProduct = new MenuProduct
                        {
                            MenuID = _currentMenu.ID,
                            ProductID = mp.ProductID,
                            Quantity = mp.Quantity
                        };
                        _context.MenuProducts.Add(newMenuProduct);
                    }

                    _context.SaveChanges();
                    MessageBox.Show("Menu created successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Update existing menu
                    _currentMenu.Name = TxtMenuName.Text;
                    _currentMenu.CategoryID = ((Category)CmbCategory.SelectedItem).ID;
                    _currentMenu.Available = ChkAvailable.IsChecked ?? true;
                    _context.SaveChanges(); // Salvăm modificările la meniu

                    // Remove all existing menu products
                    var existingProducts = _context.MenuProducts.Where(mp => mp.MenuID == _currentMenu.ID).ToList();
                    _context.MenuProducts.RemoveRange(existingProducts);
                    _context.SaveChanges(); // Salvăm ștergerea produselor vechi

                    // Add updated menu products
                    foreach (var mp in _menuProducts)
                    {
                        // Creăm instanțe noi de MenuProduct pentru a evita probleme de tracking
                        var newMenuProduct = new MenuProduct
                        {
                            MenuID = _currentMenu.ID,
                            ProductID = mp.ProductID,
                            Quantity = mp.Quantity
                        };
                        _context.MenuProducts.Add(newMenuProduct);
                    }

                    _context.SaveChanges();
                    MessageBox.Show("Menu updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                LoadMenus();
                ClearMenuDetails();
            }
            catch (Exception ex)
            {
                // Afișăm mesajul de eroare intern pentru diagnosticare
                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += $"\n\nInner Exception: {ex.InnerException.Message}";
                }

                MessageBox.Show($"Error saving menu: {errorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
