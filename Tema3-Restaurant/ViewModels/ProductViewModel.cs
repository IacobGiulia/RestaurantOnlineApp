using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3_Restaurant.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal PortionQuantity { get; set; }
        public decimal TotalQuantity { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public bool Available { get; set; }
        public string ImagePath { get; set; }
        public string AllergenNames { get; set; }


    }
}
