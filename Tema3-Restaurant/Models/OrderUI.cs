using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tema3_Restaurant.Models
{
    public partial class Order : INotifyPropertyChanged
    {
        private bool _canBeCancelled;
        private bool _hasEstimatedDeliveryTime;

        // UI-specific properties
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public bool CanBeCancelled
        {
            get => _canBeCancelled;
            set
            {
                if (_canBeCancelled != value)
                {
                    _canBeCancelled = value;
                    OnPropertyChanged();
                }
            }
        }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public bool HasEstimatedDeliveryTime
        {
            get => _hasEstimatedDeliveryTime;
            set
            {
                if (_hasEstimatedDeliveryTime != value)
                {
                    _hasEstimatedDeliveryTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
