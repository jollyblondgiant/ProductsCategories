using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using ProductsCategories.Models;
using Microsoft.EntityFrameworkCore;
using ProductsCategories.Data;

namespace ProductsCategories.Controllers
{
    public class HomeController : Controller
    {
        private ProdCatContext dbContext;
        public HomeController(ProdCatContext context)
        {
            dbContext = context;
        }
        [HttpGet]   //INDEX ROUTE
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("products")]   //VIEW ALL PRODUCTS
        public IActionResult Products()
        {
            List<Product> Products = dbContext.Products.ToList();
            List<List<string>> ProdDict = new List<List<string>>();
            foreach(var product in Products)
            {
                string ID = product.ProductID.ToString();
                ProdDict.Add(new List<string>{ID, product.Name});
                
            }
            ViewBag.Products = ProdDict;    
            

            return View();
        }
        [HttpGet("categories")]     //VIEW ALL CATEGORIES
        public IActionResult Categories()
        {
            List<Category> Categories = dbContext.Categorys.ToList();
            List<List<string>> CatDict = new List<List<string>>();
            foreach(var category in Categories)
            {
                string ID = category.CategoryID.ToString();
                CatDict.Add(new List<string>{ID, category.Name});
            }
            ViewBag.Categories = CatDict;
            return View();
        }
        [HttpGet("products/{productID}")]      //VIEW PRODUCT BY ID
        public IActionResult ViewProduct(int productID)
        {
            Product thisProduct = dbContext.Products.FirstOrDefault(product=> product.ProductID == productID);

            HttpContext.Session.SetInt32("ID", thisProduct.ProductID);
            HttpContext.Session.SetString("ProductName", thisProduct.Name);
            
            var ProdcutWithCategories = dbContext.Products
                .Include(p=>p.Associations)
                    .ThenInclude(ass=>ass.Category)
                .FirstOrDefault(p=>p.ProductID == productID);

            var notCats = dbContext.Categorys
                .Include(c=>c.Associations)
                .Where(c=>c.Associations.All(a=>a.ProductID != productID));

            ViewBag.Categories = notCats;

            return View(ProdcutWithCategories);
        }
        [HttpGet("categories/{categoryID}")]    //VIEW CATEGORY BY ID 
        public IActionResult ViewCategory(int categoryID)
        {
            Category thisCategory = dbContext.Categorys.FirstOrDefault(category=>category.CategoryID == categoryID);

            var CategoryWithProducts = dbContext.Categorys
                .Include(c=>c.Associations)
                    .ThenInclude(ass=>ass.Product)
                .FirstOrDefault(c=>c.CategoryID == categoryID);

            var notProds = dbContext.Products
                .Include(p=>p.Associations)
                .Where(p=>p.Associations.All(a=>a.CategoryID != categoryID));

            ViewBag.Products = notProds;
            
            return View(CategoryWithProducts);
        }
        [HttpPost("products")]      //ADD NEW PRODUCT
        public IActionResult AddProd(Product newProduct)
        {
            if(ModelState.IsValid)
            {
                
                string formatNewPrice = newProduct.Price.ToString("0.00"); 
                newProduct.Price = decimal.Parse(formatNewPrice);
                
                dbContext.Add(newProduct);
                dbContext.SaveChanges();
                return RedirectToAction("Products");
            }
            return View("Products");
        }
        [HttpPost("categories")]        //ADD NEW CATEGORY
        public IActionResult AddCat(Category newCat)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(newCat);
                dbContext.SaveChanges();
                return RedirectToAction("Categories");
            }
            return View("Categories");
        }
        [HttpPost("AddCategory/{productID}")]       //ADD CATEGORY TO PRODUCT
        public IActionResult AddCatToProd(int productID, string category)
        {   
        
            Product DbProduct = dbContext.Products      //GET A PRODUCT
                .Where(p=>p.ProductID == productID)
                    .FirstOrDefault();
            
            Category DbCategory = dbContext.Categorys
                .Where(c=>c.Name == category)
                    .FirstOrDefault();

            Association newAss = new Association();
            newAss.ProductID = DbProduct.ProductID;
            newAss.CategoryID = DbCategory.CategoryID;

            dbContext.Associations.Add(newAss);
            dbContext.SaveChanges();

            
            
            return Redirect($"/products/{productID}");
        }

        [HttpPost("AddProduct/{categoryID}")]        //ADD PRODUCT TO CATEGORY
        public IActionResult AddProdToCat(int categoryID, string product)
        {
            System.Console.WriteLine("################################");
            System.Console.WriteLine(categoryID);
            System.Console.WriteLine(product);
            System.Console.WriteLine("################################");

            Category DbCategory = dbContext.Categorys
                .Where(c=>c.CategoryID == categoryID)
                    .FirstOrDefault();

            Product DbProduct = dbContext.Products  
                .Where(p=>p.Name == product)
                    .FirstOrDefault();

            Association newAss = new Association();
            newAss.ProductID = DbProduct.ProductID;
            newAss.CategoryID = DbCategory.CategoryID;

            dbContext.Associations.Add(newAss);
            dbContext.SaveChanges(); 

            return Redirect($"/categories/{categoryID}");
        }
    }
}
