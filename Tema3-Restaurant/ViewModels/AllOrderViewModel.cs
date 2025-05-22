using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tema3_Restaurant.Models;

namespace Tema3_Restaurant.ViewModels
{
    public class AllOrderViewModel : BaseViewModel
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

        public string CustomerFullName => $"{FirstName} {LastName}";
        public string CustomerPhone => Phone;
        public string FormattedDate => DateAndTime.ToString("yyyy-MM-dd HH:mm");
        public string FormattedEstimatedDelivery => EstimatedDeliveryTime.HasValue ?
            EstimatedDeliveryTime.Value.ToString("yyyy-MM-dd HH:mm") : "Not set";

        public string StatusWithColor
        {
            get
            {
                switch (Status.ToLower())
                {
                    case "inregistrata":
                        return "🔵 inregistrata";
                    case "se pregateste":
                        return "🟠 se pregateste";
                    case "a plecat la client":
                        return "🟡 a plecat la client";
                    case "livrata":
                        return "🟢 livrata";
                    case "anulata":
                        return "🔴 anulata";
                    default:
                        return Status;
                }
            }
        }

    }
}
