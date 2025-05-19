using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3_Restaurant.ViewModels
{
    class ActiveOrderViewModel : BaseViewModel
    {
        public int ID { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Status { get; set; }
        public string UniqueCode { get; set; }
        public decimal ProductsPrice { get; set; }
        public decimal DeliveryPrice { get; set; }

        public decimal TotalPrice { get; set; }
        public DateTime? EstimatedDeliveryTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public string FormattedDate => DateAndTime.ToString("yyyy-MM-dd HH:mm");
        public string FullName => $"{FirstName}{LastName}";

        public string FormattedEstimatedDelivery => EstimatedDeliveryTime.HasValue? EstimatedDeliveryTime.Value.ToString("yyyy-MM-dd HH:mm") : "Not Set";

        public string StatusWithColor
        {
            get
            {
                return Status;
            }
        }
    }
}
