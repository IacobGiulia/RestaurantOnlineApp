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
using Tema3_Restaurant.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Tema3_Restaurant
{
    /// <summary>
    /// Interaction logic for AllergenManagementWindow.xaml
    /// </summary>
    public partial class AllergenManagementWindow : Window
    {
        private readonly RestaurantContext _context;

        private ObservableCollection<Allergen> _allergens;
        private CollectionViewSource _allergensViewSource;
        public AllergenManagementWindow()
        {
            InitializeComponent();
            var options = new DbContextOptionsBuilder<RestaurantContext>()
    .UseSqlServer(@"Server=localhost;Database=RestaurantDB;Trusted_Connection=True;TrustServerCertificate=True;")
    .Options;
            _context = new RestaurantContext(options);
            _allergens = new ObservableCollection<Allergen>();
            _allergensViewSource = new CollectionViewSource { Source = _allergens };

            LvAllergens.ItemsSource = _allergensViewSource.View;
            LoadAllergens();
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

        private void BtnAddAllergen_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtAllergenName.Text))
            {
                MessageBox.Show("Please enter an allergen name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_context.Allergens.Any(a => a.Name == TxtAllergenName.Text))
            {
                MessageBox.Show("An allergen with this name already exists.", "Duplicate Entry", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newAllergen = new Allergen { Name = TxtAllergenName.Text };
            _context.Allergens.Add(newAllergen);
            _context.SaveChanges();

            // Refresh list
            LoadAllergens();
            TxtAllergenName.Clear();
        }

        private void BtnUpdateAllergen_Click(object sender, RoutedEventArgs e)
        {
            var selectedAllergen = LvAllergens.SelectedItem as Allergen;
            if (selectedAllergen == null)
            {
                MessageBox.Show("Please select an allergen to update.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtAllergenName.Text))
            {
                MessageBox.Show("Please enter an allergen name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check if new name would create a duplicate (except for the current allergen)
            if (_context.Allergens.Any(a => a.Name == TxtAllergenName.Text && a.ID != selectedAllergen.ID))
            {
                MessageBox.Show("An allergen with this name already exists.", "Duplicate Entry", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Update allergen
            selectedAllergen.Name = TxtAllergenName.Text;
            _context.Entry(selectedAllergen).State = EntityState.Modified;
            _context.SaveChanges();

            // Refresh list
            LoadAllergens();
            TxtAllergenName.Clear();
            LvAllergens.SelectedItem = null;
        }

        private void BtnDeleteAllergen_Click(object sender, RoutedEventArgs e)
        {
            var selectedAllergen = LvAllergens.SelectedItem as Allergen;
            if (selectedAllergen == null)
            {
                MessageBox.Show("Please select an allergen to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Check if allergen is used by any products
            bool isUsed = _context.ProductAllergens.Any(pa => pa.AllergenID == selectedAllergen.ID);

            if (isUsed)
            {
                MessageBox.Show("This allergen cannot be deleted because it is associated with one or more products.",
                    "Deletion Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Confirm deletion
            var result = MessageBox.Show($"Are you sure you want to delete the allergen '{selectedAllergen.Name}'?",
                "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _context.Allergens.Remove(selectedAllergen);
                _context.SaveChanges();

                // Refresh list
                LoadAllergens();
                TxtAllergenName.Clear();
            }
        }

        private void LvAllergens_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedAllergen = LvAllergens.SelectedItem as Allergen;
            if (selectedAllergen != null)
            {
                TxtAllergenName.Text = selectedAllergen.Name;
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
    }
