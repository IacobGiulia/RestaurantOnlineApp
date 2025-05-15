using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3_Restaurant.Models
{
    [Table("ProductAllergen")]
    public class ProductAllergen
    {
        public int ProductID { get; set; }
        public int AllergenID { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }

        [ForeignKey("AllergenID")]
        public virtual Allergen Allergen { get; set; }
    }
}
