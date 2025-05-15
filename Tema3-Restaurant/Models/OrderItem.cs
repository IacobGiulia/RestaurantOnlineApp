using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Tema3_Restaurant.Models
{
    [Table("OrderItem")]
    public class OrderItem
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required, MaxLength(20)]
        public string ItemType { get; set; }

        [Required]
        public int ItemID { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal UnitPrice { get; set; }

        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        [NotMapped]
        public string ProductName { get; set; }

        [NotMapped]
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
