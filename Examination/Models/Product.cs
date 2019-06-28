using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Examination.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0.0, 1_000_000.00)]
        public decimal Cost { get; set; }
        [Required]
        public int ManufacturerId { get; set; }
    }
}
