using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3_Restaurant.Models
{
    [Table("ConfigurationApp")]
    public class ConfigurationApp
    {
        [Key]
        public int ID { get; set; }

        [Required, MaxLength(100)]
        public string Key { get; set; }

        [Required, MaxLength(200)]

        public string Value { get; set; }


    }
}
