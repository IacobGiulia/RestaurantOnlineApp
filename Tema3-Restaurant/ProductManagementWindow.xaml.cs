using System;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Tema3_Restaurant.Data;
using Tema3_Restaurant.Models;
using Tema3_Restaurant.ViewModels;

namespace Tema3_Restaurant
{
    /// <summary>
    /// Interaction logic for ProductManagementWindow.xaml
    /// </summary>
    public partial class ProductManagementWindow : Window
    {
        private readonly RestaurantContext _context;
        private ObservableCollection<ProductViewModel> _products;
        private CollectionViewSource _productsViewSource;
        private ObservableCollection<Category> _categories;
        private ObservableCollection<Allergen> _allergens;
        private ObservableCollection<Allergen> _selectedAllergens;
        private string _imagePath;
        public ProductManagementWindow()
        {
            InitializeComponent();
            _context = new RestaurantContext();
            _products = new ObservableCollection<ProductViewModel>();
            _productsViewSource = new CollectionViewSource { Source = _products };
            _categories = new ObservableCollection<Category>();
            _allergens = new ObservableCollection<Allergen>();
            _selectedAllergens = new ObservableCollection<Allergen>();

            LvProducts.ItemsSource = _productsViewSource.View;
            CmbCategory.ItemsSource = _categories;
            LvAllAllergens.ItemsSource = _allergens;
            LvSelectedAllergens.ItemsSource = _selectedAllergens;

            LoadProducts();
            LoadCategories();
            LoadAllergens();
        }

        private void LoadProducts()
        {
            _products.Clear();
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.ProductAllergens)
                .ThenInclude(pa => pa.Allergen)
                .ToList();

            foreach(var product in products)
            {
                var productVM = new ProductViewModel
                {
                    ID = product.ID,
                    Name = product.Name,
                    Price = product.Price,
                    PortionQuantity = product.PortionQuantity,
                    TotalQuantity = product.TotalQuantity,
                    CategoryID = product.CategoryID,
                    CategoryName = product.Category?.Name,
                    Available = product.Available,
                    ImagePath = product.FirstImagePath
                };

                foreach (var pa in product.ProductAllergens)
                {
                    productVM.AllergenNames += pa.Allergen.Name + ", ";
                }
                if (productVM.AllergenNames != null && productVM.AllergenNames.EndsWith(", "))
                {
                    productVM.AllergenNames = productVM.AllergenNames.Substring(0, productVM.AllergenNames.Length - 2);
                }

                _products.Add(productVM);
            }



        }

        private void LoadCategories()
        {
            _categories.Clear();
            var categories = _context.Categories.ToList();
            foreach (var category in categories)
            {
                _categories.Add(category);
            }
        }

        private void LoadAllergens()
        {
            _allergens.Clear();
            var allergens = _context.Allergens.ToList();
            foreach (var allergen in allergens)
            {
                _allergens.Add(allergen);
            }
        }

        private void ClearForm()
        {
            TxtProductName.Clear();
            TxtPrice.Clear();
            TxtPortionQuantity.Clear();
            TxtTotalQuantity.Clear();
            CmbCategory.SelectedItem = null;
            ChkAvailable.IsChecked = true;
            ImgProduct.Source = null;
            _imagePath = null;
            _selectedAllergens.Clear();
            LvProducts.SelectedItem = null;
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtProductName.Text))
            {
                MessageBox.Show("Please enter a product name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(TxtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(TxtPortionQuantity.Text, out decimal portionQuantity) || portionQuantity <= 0)
            {
                MessageBox.Show("Please enter a valid portion quantity.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(TxtTotalQuantity.Text, out decimal totalQuantity) || totalQuantity < 0)
            {
                MessageBox.Show("Please enter a valid total quantity.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CmbCategory.SelectedItem == null)
            {
                MessageBox.Show("Please select a category.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check if product already exists
            if (_context.Products.Any(p => p.Name == TxtProductName.Text))
            {
                MessageBox.Show("A product with this name already exists.", "Duplicate Entry", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Create new product
            var newProduct = new Product
            {
                Name = TxtProductName.Text,
                Price = price,
                PortionQuantity = portionQuantity,
                TotalQuantity = totalQuantity,
                CategoryID = ((Category)CmbCategory.SelectedItem).ID,
                Available = ChkAvailable.IsChecked ?? true
            };

            _context.Products.Add(newProduct);
            _context.SaveChanges();

            if (!string.IsNullOrEmpty(_imagePath))
            {


                var productImage = new ProductImage
                {
                    ProductID = newProduct.ID,
                    Path = _imagePath
                };

                _context.ProductImages.Add(productImage);
                _context.SaveChanges();
            }

            foreach (var allergen in _selectedAllergens)
            {
                var productAllergen = new ProductAllergen
                {
                    ProductID = newProduct.ID,
                    AllergenID = allergen.ID
                };

                _context.ProductAllergens.Add(productAllergen);
            }

            _context.SaveChanges();

            // Refresh list and clear form
            LoadProducts();
            ClearForm();

        }

        private void BtnUpdateProduct_Click(object sender, RoutedEventArgs e)
        {
            var selectedProductVM = LvProducts.SelectedItem as ProductViewModel;
            if (selectedProductVM == null)
            {
                MessageBox.Show("Please select a product to update.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Validate input
            if (string.IsNullOrWhiteSpace(TxtProductName.Text))
            {
                MessageBox.Show("Please enter a product name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(TxtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(TxtPortionQuantity.Text, out decimal portionQuantity) || portionQuantity <= 0)
            {
                MessageBox.Show("Please enter a valid portion quantity.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(TxtTotalQuantity.Text, out decimal totalQuantity) || totalQuantity < 0)
            {
                MessageBox.Show("Please enter a valid total quantity.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CmbCategory.SelectedItem == null)
            {
                MessageBox.Show("Please select a category.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check if new name would create a duplicate (except for the current product)
            if (_context.Products.Any(p => p.Name == TxtProductName.Text && p.ID != selectedProductVM.ID))
            {
                MessageBox.Show("A product with this name already exists.", "Duplicate Entry", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Get existing product from database
            var product = _context.Products.Find(selectedProductVM.ID);
            if (product == null)
            {
                MessageBox.Show("Product not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Update product
            product.Name = TxtProductName.Text;
            product.Price = price;
            product.PortionQuantity = portionQuantity;
            product.TotalQuantity = totalQuantity;
            product.CategoryID = ((Category)CmbCategory.SelectedItem).ID;
            product.Available = ChkAvailable.IsChecked ?? true;

            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();

            // Update product image if a new one was selected
            if (!string.IsNullOrEmpty(_imagePath))
            {
                string fileName = Path.GetFileName(_imagePath);
                string targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Products", fileName);

                // Ensure directory exists
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Products"));

                // Copy file to target directory
                File.Copy(_imagePath, targetPath, true);

                // Remove existing images
                var existingImages = _context.ProductImages.Where(pi => pi.ProductID == product.ID).ToList();
                foreach (var img in existingImages)
                {
                    _context.ProductImages.Remove(img);
                }

                // Add new image
                var productImage = new ProductImage
                {
                    ProductID = product.ID,
                    Path = $"/Pics/{fileName}"
                };

                _context.ProductImages.Add(productImage);
                _context.SaveChanges();
            }

            // Update allergens
            // First, remove all existing allergens
            var existingAllergens = _context.ProductAllergens.Where(pa => pa.ProductID == product.ID).ToList();
            foreach (var pa in existingAllergens)
            {
                _context.ProductAllergens.Remove(pa);
            }

            // Then add selected allergens
            foreach (var allergen in _selectedAllergens)
            {
                var productAllergen = new ProductAllergen
                {
                    ProductID = product.ID,
                    AllergenID = allergen.ID
                };

                _context.ProductAllergens.Add(productAllergen);
            }

            _context.SaveChanges();

            // Refresh list and clear form
            LoadProducts();
            ClearForm();
        }

        private void BtnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var selectedProductVM = LvProducts.SelectedItem as ProductViewModel;
            if (selectedProductVM == null)
            {
                MessageBox.Show("Please select a product to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Check if product is used in any menus
            bool isUsedInMenu = _context.MenuProducts.Any(mp => mp.ProductID == selectedProductVM.ID);
            if (isUsedInMenu)
            {
                MessageBox.Show("This product cannot be deleted because it is used in one or more menus.",
                    "Deletion Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Confirm deletion
            var result = MessageBox.Show($"Are you sure you want to delete the product '{selectedProductVM.Name}'?",
                "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Get the product and related entities
                var product = _context.Products
                    .Include(p => p.Images)
                    .Include(p => p.ProductAllergens)
                    .FirstOrDefault(p => p.ID == selectedProductVM.ID);

                if (product != null)
                {
                    // Remove product allergens
                    foreach (var pa in product.ProductAllergens.ToList())
                    {
                        _context.ProductAllergens.Remove(pa);
                    }

                    // Remove product images
                    foreach (var img in product.Images.ToList())
                    {
                        _context.ProductImages.Remove(img);
                    }

                    // Remove product
                    _context.Products.Remove(product);
                    _context.SaveChanges();

                    // Refresh list and clear form
                    LoadProducts();
                    ClearForm();
                }
            }
        }

        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png",
                Title = "Select Product Image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _imagePath = openFileDialog.FileName;
                ImgProduct.Source = new BitmapImage(new Uri(_imagePath));
            }
        }

        private void BtnAddAllergen_Click(object sender, RoutedEventArgs e)
        {
            var selectedAllergen = LvAllAllergens.SelectedItem as Allergen;
            if (selectedAllergen == null)
            {
                MessageBox.Show("Please select an allergen to add.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Check if allergen is already selected
            if (_selectedAllergens.Any(a => a.ID == selectedAllergen.ID))
            {
                MessageBox.Show("This allergen is already selected.", "Duplicate Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            _selectedAllergens.Add(selectedAllergen);
        }

        private void BtnRemoveAllergen_Click(object sender, RoutedEventArgs e)
        {
            var selectedAllergen = LvSelectedAllergens.SelectedItem as Allergen;
            if (selectedAllergen == null)
            {
                MessageBox.Show("Please select an allergen to remove.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            _selectedAllergens.Remove(selectedAllergen);
        }

        private void LvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedProductVM = LvProducts.SelectedItem as ProductViewModel;
            if (selectedProductVM == null)
            {
                ClearForm();
                return;
            }

            // Load product details
            TxtProductName.Text = selectedProductVM.Name;
            TxtPrice.Text = selectedProductVM.Price.ToString();
            TxtPortionQuantity.Text = selectedProductVM.PortionQuantity.ToString();
            TxtTotalQuantity.Text = selectedProductVM.TotalQuantity.ToString();
            ChkAvailable.IsChecked = selectedProductVM.Available;

            // Set category
            foreach (var category in _categories)
            {
                if (category.ID == selectedProductVM.CategoryID)
                {
                    CmbCategory.SelectedItem = category;
                    break;
                }
            }

            // Load image
            if (!string.IsNullOrEmpty(selectedProductVM.ImagePath))
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, selectedProductVM.ImagePath.TrimStart('/'));
                if (File.Exists(imagePath))
                {
                    ImgProduct.Source = new BitmapImage(new Uri(imagePath));
                }
            }
            else
            {
                ImgProduct.Source = null;
            }

            // Load allergens
            _selectedAllergens.Clear();
            var productAllergens = _context.ProductAllergens
                .Include(pa => pa.Allergen)
                .Where(pa => pa.ProductID == selectedProductVM.ID)
                .ToList();

            foreach (var pa in productAllergens)
            {
                _selectedAllergens.Add(pa.Allergen);
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

