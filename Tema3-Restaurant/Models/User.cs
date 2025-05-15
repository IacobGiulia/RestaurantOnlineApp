using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3_Restaurant.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [Required, MaxLength(200)]
        public string Email { get; set; }

        [Required, MaxLength(20)]
        public string Phone { get; set; }

        [Required, MaxLength(200)]
        public string Address { get; set; }

        [Required, MaxLength(100)]
        public string Password { get; set; }

        [Required, MaxLength(20)]
        public string Role { get; set; }

        public virtual ICollection <Order> Orders { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName}{LastName}";

    }
}
