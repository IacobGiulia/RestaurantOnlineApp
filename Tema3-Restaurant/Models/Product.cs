using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3_Restaurant.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PortionQuantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalQuantity { get; set; }

        [Required]
        public int CategoryID { get; set; }

        public bool Available { get; set; } = true;

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }

        public virtual ICollection<ProductImage> Images { get; set;}
        public virtual ICollection<ProductAllergen> ProductAllergens { get; set; }
        public virtual ICollection<MenuProduct> MenuProducts { get; set; }

        public string FirstImagePath => Images?.FirstOrDefault()?.Path;

    }
}
