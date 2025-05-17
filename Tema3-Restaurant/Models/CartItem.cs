using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Tema3_Restaurant.Models
{
    public class CartItem : INotifyPropertyChanged
    {
        private int _quantity;
        private object _item;

        public object Item
        {
            get => _item;
            set
            {
                _item = value;
                OnPropertyChanged();
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }

        public string ItemType
        {
            get
            {
                if (Item is Product)
                    return "Product";
                else if (Item is Menu)
                    return "Menu";
                else return "Unknown";
            }

        }

        public int ItemID
        {
            get
            {
                if (Item is Product product)
                    return product.ID;
                else if (Item is Menu menu)
                    return menu.ID;
                else return -1;
            }
        }

        public decimal UnitPrice
        {
            get
            {
                if (Item is Product product)
                    return product.Price;
                else if (Item is Menu menu)
                    return menu.TotalPrice;
                else return 0;
            }

        }

        public decimal TotalPrice => Quantity * UnitPrice;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
