using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tema3_Restaurant.Data;

namespace Tema3_Restaurant.Models
{
    [Table("Menus")]
    public class Menu
    {
        [Key]
        public int ID { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set;}

        [Required]
        public int CategoryID { get; set; }

        public bool Available { get; set; } = true;

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }

        public virtual ICollection<MenuProduct> MenuProducts { get; set; }

        [NotMapped]
        public decimal TotalPrice
        {
            get
            {
                decimal price = 0;
                if(MenuProducts != null)
                {
                    foreach(var mp in MenuProducts)
                    {
                        price += mp.Product.Price * (mp.Quantity / mp.Product.PortionQuantity);
                    }
                    using (var context = new Data.RestaurantContext())
                    {
                        var configuration = context.ConfigurationApp
                            .FirstOrDefault(c => c.Key == "ProcentReducereMeniu");

                        if (configuration!= null && decimal.TryParse(configuration.Value, out decimal discountPercentage))
                        {
                            price = price * (1 - (discountPercentage / 100));
                        }
                    }
                }
                return Math.Round(price, 2);
            }
        }
    }
}
