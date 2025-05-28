using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Tema3_Restaurant.Data;
using Tema3_Restaurant.Models;

namespace Tema3_Restaurant.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        /// <summary>
        
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

        /// <summary>
        /// 
        /// </summary>

        private readonly RestaurantContext _context;
        private readonly User _currentUser;
        private ObservableCollection<CartItem> _cartItems;
        private ObservableCollection<Order> _orders;
        private bool _showAllOrders = true;
        private decimal _deliveryFee;
        private decimal _minOrderValueForFreeDelivery;
        private decimal _discountPercentage;
        private decimal _minOrderValueForDiscount;
        private int _orderCountForDiscount;
        private int _timeWindowForDiscountInDays;

        public OrderViewModel(User currentUser)
        {
            _context = new RestaurantContext();
            _currentUser = currentUser;
            _cartItems = new ObservableCollection<CartItem>();
            _orders = new ObservableCollection<Order>();

            LoadConfigurationSettings();
            LoadUserOrders();
            LoadCategories();
            LoadAllProductsAndMenus();

            GroupedSearchResults = new ObservableCollection<IGrouping<Category, object>>();

            AddToCartCommand = new RelayCommand2(AddToCart);
            DecreaseQuantityCommand = new RelayCommand2(DecreaseQuantity);
            IncreaseQuantityCommand = new RelayCommand2(IncreaseQuantity);
            RemoveFromCartCommand = new RelayCommand2(RemoveFromCart);
            DecreaseCartItemCommand = new RelayCommand2(DecreaseCartItem);
            IncreaseCartItemCommand = new RelayCommand2(IncreaseCartItem);
            PlaceOrderCommand = new RelayCommand2(PlaceOrder);
            CancelOrderCommand = new RelayCommand2(CancelOrder);
            RefreshOrdersCommand = new RelayCommand2(param => LoadUserOrders());
            ContinueShoppingCommand = new RelayCommand2(param => SwitchToMenuTab?.Invoke());
            ViewCartCommand = new RelayCommand2(param => SwitchToCartTab?.Invoke());
            CheckoutCommand = new RelayCommand2(param => SwitchToCartTab?.Invoke());
        }

        public ObservableCollection<CartItem> CartItems
        {
            get => _cartItems;
            set
            {
                _cartItems = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CartItemCount));
                OnPropertyChanged(nameof(CartTotal));
                OnPropertyChanged(nameof(HasItemsInCart));
                OnPropertyChanged(nameof(SubTotal));
                OnPropertyChanged(nameof(DeliveryFee));
                OnPropertyChanged(nameof(Discount));
                OnPropertyChanged(nameof(OrderTotal));
            }
        }

        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasOrders));
            }
        }

        public bool ShowAllOrders
        {
            get => _showAllOrders;
            set
            {
                _showAllOrders = value;
                OnPropertyChanged();
                LoadUserOrders();
            }
        }

        public bool ShowActiveOrders
        {
            get => !_showAllOrders;
            set
            {
                _showAllOrders = !value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShowAllOrders));
                LoadUserOrders();
            }
        }

        public int CartItemCount => CartItems.Sum(item => item.Quantity);
        public decimal CartTotal => CartItems.Sum(item => item.TotalPrice);
        public bool HasItemsInCart => CartItems.Count > 0;
        public bool HasOrders => Orders.Count > 0;

        public decimal SubTotal => CartTotal;

        public decimal DeliveryFee
        {
            get
            {
                if (SubTotal >= _minOrderValueForFreeDelivery)
                    return 0;
                return _deliveryFee;
            }
        }

        public decimal Discount
        {
            get
            {
                decimal discount = 0;

                if (SubTotal >= _minOrderValueForDiscount)
                {
                    discount = SubTotal * (_discountPercentage / 100);
                }

                DateTime timeWindow = DateTime.Now.AddDays(-_timeWindowForDiscountInDays);
                int orderCount = _context.Orders
                    .Count(o => o.UserID == _currentUser.ID && o.DateAndTime >= timeWindow);

                if (orderCount >= _orderCountForDiscount)
                {
                    discount = Math.Max(discount, SubTotal * (_discountPercentage / 100));
                }

                return Math.Round(discount, 2);
            }
        }

        public decimal OrderTotal => SubTotal + DeliveryFee - Discount;

        public ICommand AddToCartCommand { get; private set; }
        public ICommand DecreaseQuantityCommand { get; private set; }
        public ICommand IncreaseQuantityCommand { get; private set; }
        public ICommand RemoveFromCartCommand { get; private set; }
        public ICommand DecreaseCartItemCommand { get; private set; }
        public ICommand IncreaseCartItemCommand { get; private set; }
        public ICommand PlaceOrderCommand { get; private set; }
        public ICommand CancelOrderCommand { get; private set; }
        public ICommand RefreshOrdersCommand { get; private set; }
        public ICommand ContinueShoppingCommand { get; private set; }
        public ICommand ViewCartCommand { get; private set; }
        public ICommand CheckoutCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action SwitchToMenuTab;
        public event Action SwitchToCartTab;

        private void LoadConfigurationSettings()
        {
            try
            {
                var configurationSettings = _context.ConfigurationApp.ToList();

                _deliveryFee = GetDecimalConfigValue(configurationSettings, "CostTransport", 15);
                _minOrderValueForFreeDelivery = GetDecimalConfigValue(configurationSettings, "ValoareMinimaTransportGratuit", 100);
                _discountPercentage = GetDecimalConfigValue(configurationSettings, "ProcentReducereComanda", 10);
                _minOrderValueForDiscount = GetDecimalConfigValue(configurationSettings, "ValoareMinimaPentruReducere", 150);
                _orderCountForDiscount = GetIntConfigValue(configurationSettings, "NumarComenziPentruDiscount", 5);
                _timeWindowForDiscountInDays = GetIntConfigValue(configurationSettings, "IntervalZilePentruDiscount", 30);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading configuration settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                _deliveryFee = 10;
                _minOrderValueForFreeDelivery = 100;
                _discountPercentage = 10;
                _minOrderValueForDiscount = 200;
                _orderCountForDiscount = 3;
                _timeWindowForDiscountInDays = 30;
            }
        }

        private decimal GetDecimalConfigValue(List<ConfigurationApp> configurations, string key, decimal defaultValue)
        {
            var config = configurations.FirstOrDefault(c => c.Key == key);
            if (config != null && decimal.TryParse(config.Value, out decimal value))
            {
                return value;
            }
            return defaultValue;
        }

        private int GetIntConfigValue(List<ConfigurationApp> configurations, string key, int defaultValue)
        {
            var config = configurations.FirstOrDefault(c => c.Key == key);
            if (config != null && int.TryParse(config.Value, out int value))
            {
                return value;
            }
            return defaultValue;
        }

        private void LoadUserOrders()
        {
            try
            {
                using (var context = new RestaurantContext())
                {
                    var query = context.Orders
                        .Include(o => o.Items)
                        .Where(o => o.UserID == _currentUser.ID);

                    if (!ShowAllOrders)
                    {
                        query = query.Where(o => o.State != "Livrata" && o.State != "Anulata");
                    }

                    var orders = query.OrderByDescending(o => o.DateAndTime).ToList();

                    foreach (var order in orders)
                    {
                        foreach (var item in order.Items)
                        {
  
                            if (item.ItemType == "Product")
                            {
                                var product = context.Products.Find(item.ItemID);
                                if (product != null)
                                {
                                    item.ProductName = product.Name;
                                }
                            }
                            else if (item.ItemType == "Menu")
                            {
                                var menu = context.Menus.Find(item.ItemID);
                                if (menu != null)
                                {
                                    item.ProductName = menu.Name;
                                }
                            }
                        }

                        order.GetType().GetProperty("CanBeCancelled").SetValue(order,
                            order.State == "Inregistrata" || order.State == "Se pregateste");

                        order.GetType().GetProperty("HasEstimatedDeliveryTime").SetValue(order,
                            order.EstimatedDeliveryTime.HasValue);
                    }

                    Orders = new ObservableCollection<Order>(orders);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddToCart(object parameter)
        {
            if (parameter is Product product)
            {
                var existingItem = CartItems.FirstOrDefault(ci =>
                    ci.ItemType == "Product" && ci.ItemID == product.ID);

                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    CartItems.Add(new CartItem
                    {
                        Item = product,
                        Quantity = 1
                    });
                }
            }
            else if (parameter is Menu menu)
            {
                var existingItem = CartItems.FirstOrDefault(ci =>
                    ci.ItemType == "Menu" && ci.ItemID == menu.ID);

                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    CartItems.Add(new CartItem
                    {
                        Item = menu,
                        Quantity = 1
                    });
                }
            }

            UpdateCartDependencies();
        }

        private void DecreaseQuantity(object parameter)
        {

            if (parameter is Product product)
            {
                int currentQuantity = GetQuantity(product);
                if (currentQuantity > 0)
                {
                    SetQuantity(product, currentQuantity - 1);
                }
            }
            else if (parameter is Menu menu)
            {
                int currentQuantity = GetQuantity(menu);
                if (currentQuantity > 0)
                {
                    SetQuantity(menu, currentQuantity - 1);
                }
            }
        }

        private void IncreaseQuantity(object parameter)
        {

            if (parameter is Product product)
            {
                int currentQuantity = GetQuantity(product);
                SetQuantity(product, currentQuantity + 1);
            }
            else if (parameter is Menu menu)
            {
                int currentQuantity = GetQuantity(menu);
                SetQuantity(menu, currentQuantity + 1);
            }
        }

        private int GetQuantity(object item)
        {
            return 0;
        }

        private void SetQuantity(object item, int quantity)
        {
            if (quantity > 0)
            {
                AddToCart(item);
            }
        }

        private void RemoveFromCart(object parameter)
        {
            if (parameter is CartItem cartItem)
            {
                CartItems.Remove(cartItem);
                UpdateCartDependencies();
            }
        }

        private void DecreaseCartItem(object parameter)
        {
            if (parameter is CartItem cartItem)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                }
                else
                {
                    CartItems.Remove(cartItem);
                }
                UpdateCartDependencies();
            }
        }

        private void IncreaseCartItem(object parameter)
        {
            if (parameter is CartItem cartItem)
            {
                cartItem.Quantity++;
                UpdateCartDependencies();
            }
        }

        private void UpdateCartDependencies()
        {
            OnPropertyChanged(nameof(CartItemCount));
            OnPropertyChanged(nameof(CartTotal));
            OnPropertyChanged(nameof(HasItemsInCart));
            OnPropertyChanged(nameof(SubTotal));
            OnPropertyChanged(nameof(DeliveryFee));
            OnPropertyChanged(nameof(Discount));
            OnPropertyChanged(nameof(OrderTotal));
        }

        private void PlaceOrder(object parameter)
        {
            try
            {
                if (!HasItemsInCart)
                {
                    MessageBox.Show("Your cart is empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var context = new RestaurantContext())
                {

                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {

                            var order = new Order
                            {
                                UserID = _currentUser.ID,
                                DateAndTime = DateTime.Now,
                                State = "Inregistrata",
                                ProductsPrice = SubTotal,
                                DeliveryPrice = DeliveryFee,
                                Discount = Discount,
                                TotalPrice = OrderTotal,
                                UniqueCode = GenerateUniqueCode(),
                                Items = new List<OrderItem>()
                            };

                            foreach (var cartItem in CartItems)
                            {
                                var orderItem = new OrderItem
                                {
                                    ItemType = cartItem.ItemType,
                                    ItemID = cartItem.ItemID,
                                    Quantity = cartItem.Quantity,
                                    UnitPrice = cartItem.UnitPrice
                                };

                                order.Items.Add(orderItem);

                                UpdateProductQuantities(context, cartItem);
                            }

                            order.EstimatedDeliveryTime = DateTime.Now.AddHours(1);

                            context.Orders.Add(order);
                            context.SaveChanges();

                            transaction.Commit();

                            CartItems.Clear();
                            UpdateCartDependencies();

                            LoadUserOrders();

                            MessageBox.Show($"Order placed successfully! Your order code is: {order.UniqueCode}",
                                "Order Placed", MessageBoxButton.OK, MessageBoxImage.Information);

                            SwitchToMenuTab?.Invoke();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception($"Error placing order: {ex.Message}", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to place order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GenerateUniqueCode()
        {
            string dateString = DateTime.Now.ToString("yyyyMMddHHmmss");
            string randomString = new Random().Next(1000, 9999).ToString();
            return $"ORD-{dateString}-{randomString}";
        }

        private void UpdateProductQuantities(RestaurantContext context, CartItem cartItem)
        {
            if (cartItem.ItemType == "Product")
            {
                var product = context.Products.Find(cartItem.ItemID);
                if (product != null)
                {
                    decimal quantityToDeduct = product.PortionQuantity * cartItem.Quantity;
                    product.TotalQuantity -= quantityToDeduct;

                    if (product.TotalQuantity < 0)
                        product.TotalQuantity = 0;

                    product.Available = product.TotalQuantity >= product.PortionQuantity;

                    context.Entry(product).State = EntityState.Modified;
                }
            }
            else if (cartItem.ItemType == "Menu")
            {
                var menu = context.Menus
                    .Include(m => m.MenuProducts)
                    .ThenInclude(mp => mp.Product)
                    .FirstOrDefault(m => m.ID == cartItem.ItemID);

                if (menu != null)
                {
                    foreach (var menuProduct in menu.MenuProducts)
                    {

                        decimal quantityToDeduct = menuProduct.Quantity * cartItem.Quantity;
                        menuProduct.Product.TotalQuantity -= quantityToDeduct;

                        if (menuProduct.Product.TotalQuantity < 0)
                            menuProduct.Product.TotalQuantity = 0;

                        menuProduct.Product.Available = menuProduct.Product.TotalQuantity >= menuProduct.Product.PortionQuantity;

                        context.Entry(menuProduct.Product).State = EntityState.Modified;
                    }
                }
            }
   
        }

        private void CancelOrder(object parameter)
        {
            if (parameter is Order order)
            {
                if (order.State != "Inregistrata" && order.State != "Se pregateste")
                {
                    MessageBox.Show("This order cannot be cancelled.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var result = MessageBox.Show($"Are you sure you want to cancel order {order.UniqueCode}?",
                    "Confirm Cancellation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (var context = new RestaurantContext())
                        {
                            using (var transaction = context.Database.BeginTransaction())
                            {
                                try
                                {

                                    var dbOrder = context.Orders
                                        .Include(o => o.Items)
                                        .FirstOrDefault(o => o.ID == order.ID);

                                    if (dbOrder != null)
                                    {

                                        RestoreProductQuantities(context, dbOrder);

                                        dbOrder.State = "Anulata";

                                        context.SaveChanges();

                                        transaction.Commit();

                                        LoadUserOrders();

                                        LoadProductsAndMenusByCategory();

                                        MessageBox.Show("Order cancelled successfully and stock has been restored.",
                                            "Order Cancelled", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    throw new Exception($"Error cancelling order: {ex.Message}", ex);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to cancel order: {ex.Message}", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void RestoreProductQuantities(RestaurantContext context, Order order)
        {
            foreach (var orderItem in order.Items)
            {
                if (orderItem.ItemType == "Product")
                {
                    var product = context.Products.Find(orderItem.ItemID);
                    if (product != null)
                    {
                        
                        decimal quantityToRestore = product.PortionQuantity * orderItem.Quantity;
                        product.TotalQuantity += quantityToRestore;

                        
                        product.Available = product.TotalQuantity >= product.PortionQuantity;

                        
                        context.Entry(product).State = EntityState.Modified;
                    }
                }
                else if (orderItem.ItemType == "Menu")
                {
                    var menu = context.Menus
                        .Include(m => m.MenuProducts)
                        .ThenInclude(mp => mp.Product)
                        .FirstOrDefault(m => m.ID == orderItem.ItemID);

                    if (menu != null)
                    {
                        foreach (var menuProduct in menu.MenuProducts)
                        {
                            decimal quantityToRestore = menuProduct.Quantity * orderItem.Quantity;
                            menuProduct.Product.TotalQuantity += quantityToRestore;

                            menuProduct.Product.Available = menuProduct.Product.TotalQuantity >= menuProduct.Product.PortionQuantity;

                            context.Entry(menuProduct.Product).State = EntityState.Modified;
                        }
                    }
                }
            }
        }

    }

    
}