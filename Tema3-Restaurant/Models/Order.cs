using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3_Restaurant.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public DateTime DateAndTime { get; set; } = DateTime.Now;

        [Required, MaxLength(50)]
        public string State { get; set; } = "Registered";

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal ProductsPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal DeliveryPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Discount { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalPrice { get; set; }

        [Required, MaxLength(50)]
        public string UniqueCode { get; set; }

        public DateTime? EstimatedDeliveryTime { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public virtual ICollection<OrderItem> Items { get; set; }

    }
}
