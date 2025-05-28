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

        private string _searchKeyword;
        private bool _searchContains = true;
        private bool _searchDoesNotContain;
        private bool _searchInName = true;
        private bool _searchInAllergens;
        private bool _isSearchActive;
        private ObservableCollection<IGrouping<Category, object>> _groupedSearchResults;

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

                _isSearchActive = false;
            }
        }

        public string SearchKeyword
        {
            get { return _searchKeyword; }
            set
            {
                _searchKeyword = value;
                OnPropertyChanged();
            }
        }

        public bool SearchContains
        {
            get { return _searchContains; }
            set
            {
                _searchContains = value;
                _searchDoesNotContain = !value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SearchDoesNotContain));
            }
        }

        public bool SearchDoesNotContain
        {
            get { return _searchDoesNotContain; }
            set
            {
                _searchDoesNotContain = value;
                _searchContains = !value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SearchContains));
            }
        }

        public bool SearchInName
        {
            get { return _searchInName; }
            set
            {
                _searchInName = value;
                _searchInAllergens = !value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SearchInAllergens));
            }
        }

        public bool SearchInAllergens
        {
            get { return _searchInAllergens; }
            set
            {
                _searchInAllergens = value;
                _searchInName = !value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SearchInName));
            }
        }

        public bool IsSearchActive
        {
            get { return _isSearchActive; }
            set
            {
                _isSearchActive = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IGrouping<Category, object>> GroupedSearchResults
        {
            get { return _groupedSearchResults; }
            set
            {
                _groupedSearchResults = value;
                OnPropertyChanged();
            }
        }

        public MenuViewModel()
        {
            LoadCategories();
            LoadAllProductsAndMenus();

            GroupedSearchResults = new ObservableCollection<IGrouping<Category, object>>();
        }

        private void LoadCategories()
        {
            using (var context = new RestaurantContext())
            {
                Categories = new ObservableCollection<Category>(context.Categories.ToList());
            }
        }

        private void LoadAllProductsAndMenus()
        {
            using (var context = new RestaurantContext())
            {
     
                var products = context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .Include(p => p.ProductAllergens)
                        .ThenInclude(pa => pa.Allergen)
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
                    .ToList(); 

             
                Menus = new ObservableCollection<Menu>(menus);
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
                    .Where(p => p.CategoryID == SelectedCategory.ID) 
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
                    .Where(m => m.CategoryID == SelectedCategory.ID) 
                    .ToList();

                Menus = new ObservableCollection<Menu>(menus);
            }
        }

        private ICommand _clearFilterCommand;

        public ICommand ClearFilterCommand
        {
            get
            {
                if (_clearFilterCommand == null)
                {
                    _clearFilterCommand = new RelayCommand((param) =>
                    {
                        SelectedCategory = null;
                        SearchKeyword = string.Empty;
                        IsSearchActive = false;
                    });
                }
                return _clearFilterCommand;
            }
        }

        private ICommand _searchCommand;

        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                {
                    _searchCommand = new RelayCommand((param) =>
                    {
                        PerformSearch();
                    });
                }
                return _searchCommand;
            }
        }

        private void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchKeyword))
            {
                
                IsSearchActive = false;
                return;
            }

            List<Product> productsToSearch;
            List<Menu> menusToSearch;

            if (SelectedCategory != null)
            {
                productsToSearch = Products.ToList();
                menusToSearch = Menus.ToList();
            }
            else
            {
                
                using (var context = new RestaurantContext())
                {
                    productsToSearch = context.Products
                        .Include(p => p.Category)
                        .Include(p => p.Images)
                        .Include(p => p.ProductAllergens)
                            .ThenInclude(pa => pa.Allergen)
                        .ToList(); 

                    menusToSearch = context.Menus
                        .Include(m => m.Category)
                        .Include(m => m.MenuProducts)
                            .ThenInclude(mp => mp.Product)
                                .ThenInclude(p => p.ProductAllergens)
                                    .ThenInclude(pa => pa.Allergen)
                        .Include(m => m.MenuProducts)
                            .ThenInclude(mp => mp.Product)
                                .ThenInclude(p => p.Images)
                        .ToList(); 
                }
            }

            var filteredItems = new List<object>();
            string keyword = SearchKeyword.ToLower().Trim();

            foreach (var product in productsToSearch)
            {
                bool matchesSearch = false;

                if (SearchInName)
                {
                    
                    bool containsKeyword = product.Name.ToLower().Contains(keyword);
                    matchesSearch = SearchContains ? containsKeyword : !containsKeyword;
                }
                else if (SearchInAllergens)
                {
                    
                    bool containsAllergen = product.ProductAllergens != null &&
                                         product.ProductAllergens.Any(pa =>
                                             pa.Allergen.Name.ToLower().Contains(keyword));
                    matchesSearch = SearchContains ? containsAllergen : !containsAllergen;
                }

                if (matchesSearch)
                {
                    filteredItems.Add(product);
                }
            }

            
            foreach (var menu in menusToSearch)
            {
                bool matchesSearch = false;

                if (SearchInName)
                {
                    
                    bool containsKeyword = menu.Name.ToLower().Contains(keyword);
                    matchesSearch = SearchContains ? containsKeyword : !containsKeyword;
                }
                else if (SearchInAllergens)
                {
                    
                    bool containsAllergen = false;

                    if (menu.MenuProducts != null)
                    {
                        foreach (var menuProduct in menu.MenuProducts)
                        {
                            if (menuProduct.Product.ProductAllergens != null &&
                                menuProduct.Product.ProductAllergens.Any(pa =>
                                    pa.Allergen.Name.ToLower().Contains(keyword)))
                            {
                                containsAllergen = true;
                                break;
                            }
                        }
                    }

                    matchesSearch = SearchContains ? containsAllergen : !containsAllergen;
                }

                if (matchesSearch)
                {
                    filteredItems.Add(menu);
                }
            }

            var groupedItems = filteredItems
                .GroupBy(item =>
                {
                    if (item is Product product)
                        return product.Category;
                    else if (item is Menu menu)
                        return menu.Category;
                    return null;
                })
                .Where(g => g.Key != null)
                .OrderBy(g => g.Key.Name);

            GroupedSearchResults = new ObservableCollection<IGrouping<Category, object>>(groupedItems);
            IsSearchActive = true;
        }
    }
}