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
            _context = new RestaurantContext();
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


    }
}
