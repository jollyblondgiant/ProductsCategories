using Microsoft.EntityFrameworkCore;
using ProductsCategories.Models;
namespace ProductsCategories.Models{
    public class ProdCatContext : DbContext
    {
        public ProdCatContext(DbContextOptions<ProdCatContext> options) :base(options){}
        
        public DbSet<Association> Associations{get;set; }
        public DbSet<Product> Products {get;set;}
        public DbSet<Category> Categorys {get;set;}
    }
}