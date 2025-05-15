using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Tema3_Restaurant.Models;
using Tema3_Restaurant.Data;


namespace Tema3_Restaurant.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private ObservableCollection<Category> _categories;
        private ObservableCollection<Product> _products;
        private ObservableCollection<Menu> _menus;
        private Category _selectedCategory;

        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Menu> Menus
        {
            get { return _menus; }
            set
            {
                _menus = value;
                OnPropertyChanged();
            }
        }

        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
                LoadProductsAndMenusByCategory();
            }
        }

        public MenuViewModel()
        {
            LoadCategories();
            LoadAllProductsAndMenus();
        }

        private void LoadCategories()
        {
            using ( var context = new RestaurantContext())
            {
                Categories = new ObservableCollection<Category>(context.Categories.ToList());
            }
        }

        private void LoadAllProductsAndMenus()
        {
            using ( var context = new RestaurantContext())
            {
                var products = context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .Include(p => p.ProductAllergens)
                        .ThenInclude(pa => pa.Allergen)
                    .Where(p => p.Available)
                    .ToList();

                Products = new ObservableCollection<Product>(products);

                var menus = context.Menus
                    .Include(m => m.Category)
                    .Include(m => m.MenuProducts)
                        .ThenInclude(mp => mp.Product)
                            .ThenInclude(mp => mp.ProductAllergens)
                                .ThenInclude(pa => pa.Allergen)
                    .Include(m => m.MenuProducts)
                        .ThenInclude(mp => mp.Product)
                            .ThenInclude(p => p.Images)
                    .Where(m => m.Available)
                    .ToList();

                Menus = new ObservableCollection<Menu>((IEnumerable<Menu>)menus);

            }
        }

        private void LoadProductsAndMenusByCategory()
        {
            if (SelectedCategory == null)
            {
                LoadAllProductsAndMenus();
                return;
            }

            using (var context = new RestaurantContext())
            {

                var products = context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .Include(p => p.ProductAllergens)
                        .ThenInclude(pa => pa.Allergen)
                    .Where(p => p.CategoryID == SelectedCategory.ID && p.Available)
                    .ToList();

                Products = new ObservableCollection<Product>(products);


                var menus = context.Menus
                    .Include(m => m.Category)
                    .Include(m => m.MenuProducts)
                        .ThenInclude(mp => mp.Product)
                            .ThenInclude(p => p.ProductAllergens)
                                .ThenInclude(pa => pa.Allergen)
                    .Include(m => m.MenuProducts)
                        .ThenInclude(mp => mp.Product)
                            .ThenInclude(p => p.Images)
                    .Where(m => m.CategoryID == SelectedCategory.ID && m.Available)
                    .ToList();

                Menus = new ObservableCollection<Menu>((IEnumerable<Menu>)menus);
            }
        }

        private ICommand _clearFilterCommand;

        public ICommand ClearFilterCommand
        {
            get
            {
                if (_clearFilterCommand == null)
                {
                    _clearFilterCommand = new RelayCommand(() =>
                    {
                        SelectedCategory = null;
                    });
                }
                return _clearFilterCommand;
            }
        }

    }
}
