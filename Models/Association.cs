using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ProductsCategories.Models
{
    public class Association
    {
        [Key]
        public int AssociationID{get;set;}

        public int ProductID {get;set;}

        public int CategoryID{get;set;}
        public Category Category {get;set;}
        public Product Product {get;set;}
    }
}