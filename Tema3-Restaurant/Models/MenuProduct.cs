using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3_Restaurant.Models
{
    [Table("MenuProduct")]
    public class MenuProduct
    {
        public int MenuID { get; set; }
        public int ProductID { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Quantity { get; set; }

        [ForeignKey("MenuID")]
        public virtual Menu Menu { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }


    }
}
