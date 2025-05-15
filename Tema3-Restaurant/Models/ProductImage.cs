using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3_Restaurant.Models
{
    [Table("ProductImages")]
    public class ProductImage
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required, MaxLength(500)]
        public string Path { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
    }
}
