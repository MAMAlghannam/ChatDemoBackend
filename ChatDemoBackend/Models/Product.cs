using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatDemoBackend.Models
{
    public class Product
    {
        [Key]
        public int Product_Id { get; set; }

        [StringLength(255, MinimumLength = 3, ErrorMessage = "Product name is required and must at least three characters.")]
        [Display(Name = "Product name")]
        [Required]
        public string Product_Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Price is required and must be more than 0.")]
        [Display(Name = "Product price")]
        [Required]
        public double Product_Price { get; set; }

        public string Print()
        {
            return Product_Name + " " + Product_Price + " SAR";
        }
    }
}
