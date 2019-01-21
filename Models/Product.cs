using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ProductsCategories.Models
{
    public class Product
    {
        [Key]
        public int ProductID {get;set;}
        [Required(ErrorMessage="Construct more pylons!")]
        [MinLength(3, ErrorMessage="Construct more than 2 Pylons, Peon!")]
        [Display(Name="Product Name: ")]
        public string Name{get;set;}
        [Required(ErrorMessage="You must describe more pylons!")]
        [MinLength(8, ErrorMessage="You can do better than that,summoner")]
        [Display(Name="Product Description: ")]
        public string Description{get;set;}
        [Required(ErrorMessage="All things have their price...")]
        [MinValue(0.02, ErrorMessage="Let's just say, you don't buy that... with money")]
        [Display(Name="Product Price: ")]
        public decimal Price {get;set;}
        public DateTime CreatedAt{get;set;}
        public DateTime UpdatedAt{get;set;}
        public List<Association> Associations {get;set;}

        public class MinValueAttribute : ValidationAttribute
        {
            private decimal _minvalue;
            public MinValueAttribute(double minValue)
            {
                _minvalue = (decimal)minValue;
            }
            public override bool IsValid(object value)
            {
                return _minvalue < (decimal)value;
            }
        }
    }
}