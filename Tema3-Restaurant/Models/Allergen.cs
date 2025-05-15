using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3_Restaurant.Models
{
    [Table("Allergens")]
    public class Allergen
    {
        [Key]
        public int ID { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public virtual ICollection<ProductAllergen> ProductAllergens { get; set; }


    }
}
