using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsCategories.Models
{
    public class Category
    {
        [Key]
        public int CategoryID{get;set;}
        [Required(ErrorMessage="PAPERS, PLEASE")]
        [MinLength(3, ErrorMessage="THIIS IS CLEARLY NOT ENOUGH PAPERS")]
        [Display(Name="Category Name: ")]
        public string Name{get;set;}
        
        public DateTime CreatedAt{get;set;}
        public DateTime UpdatedAt{get;set;}
        public List<Association> Associations{get;set;}
    }
}