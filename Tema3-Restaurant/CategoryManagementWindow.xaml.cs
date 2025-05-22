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
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using Tema3_Restaurant.Data;
using Tema3_Restaurant.Models;

namespace Tema3_Restaurant
{
    /// <summary>
    /// Interaction logic for CategoryManagementWindow.xaml
    /// </summary>
    public partial class CategoryManagementWindow : Window
    {
        private readonly RestaurantContext _context;
        private ObservableCollection<Category> _categories;
        private CollectionViewSource _categoriesViewSource;
        public CategoryManagementWindow()
        {
            InitializeComponent();
            var options = new DbContextOptionsBuilder<RestaurantContext>()
    .UseSqlServer(@"Server=localhost;Database=RestaurantDB;Trusted_Connection=True;TrustServerCertificate=True;")
    .Options;
            _context = new RestaurantContext(options);
            _categories = new ObservableCollection<Category>();
            _categoriesViewSource = new CollectionViewSource { Source = _categories };

            LvCategories.ItemsSource = _categoriesViewSource.View;
            LoadCategories();
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

        private void BtnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtCategoryName.Text))
            {
                MessageBox.Show("Please enter a category name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_context.Categories.Any(c => c.Name == TxtCategoryName.Text))
            {
                MessageBox.Show("A category with this name already exists.", "Duplicate Entry", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newCategory = new Category { Name = TxtCategoryName.Text };
            _context.Categories.Add(newCategory);
            _context.SaveChanges();

            LoadCategories();
            TxtCategoryName.Clear();
        }

        private void BtnUpdateCategory_Click(object sender, RoutedEventArgs e)
        {
            var selectedCategory = LvCategories.SelectedItem as Category;

            if (selectedCategory == null)
            {
                MessageBox.Show("Please select a category to update.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtCategoryName.Text))
            {
                MessageBox.Show("Please enter a category name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_context.Categories.Any(c => c.Name == TxtCategoryName.Text && c.ID != selectedCategory.ID))
            {
                MessageBox.Show("A category with this name already exists.", "Duplicate Entry", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            selectedCategory.Name = TxtCategoryName.Text;
            _context.Entry(selectedCategory).State = EntityState.Modified;
            _context.SaveChanges();

            LoadCategories();
            TxtCategoryName.Clear();
            LvCategories.SelectedItem = null;




        }

        private void BtnDeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            var selectedCategory = LvCategories.SelectedItem as Category;
            if(selectedCategory == null)
            {
                MessageBox.Show("Please select a category to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            bool hasProducts = _context.Products.Any(p => p.CategoryID == selectedCategory.ID);
            bool hasMenus = _context.Menus.Any(m => m.CategoryID == selectedCategory.ID);
            if (hasProducts || hasMenus)
            {
                MessageBox.Show("This category cannot be deleted because it is being used by products or menus.",
                    "Deletion Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete the category '{selectedCategory.Name}'?",
                "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if(result == MessageBoxResult.Yes)
            {
                _context.Categories.Remove(selectedCategory);
                _context.SaveChanges();

                LoadCategories();
                TxtCategoryName.Clear();
            }

        }

        private void LvCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCategory = LvCategories.SelectedItem as Category;

            if (selectedCategory != null)
            {
                TxtCategoryName.Text = selectedCategory.Name;
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
