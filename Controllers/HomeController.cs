using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdsCats.Models;

namespace ProdsCats.Controllers
{
    public class HomeController : Controller
    {
        private ProdsCatsContext dbContext;
        public HomeController(ProdsCatsContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            List<Product> AllProducts = dbContext.Products.ToList();
            ViewBag.AllProducts = AllProducts;
            return View("Index");
        }

        [HttpPost]
        [Route("newprod")]
        public IActionResult NewProd(Product newProduct)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(newProduct);
                dbContext.SaveChanges();
                return RedirectToAction("Index", newProduct);
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet]
        [Route("categories")]
        public IActionResult Categories()
        {
            List<Category> AllCategories = dbContext.Categories.ToList();
            ViewBag.AllCategories = AllCategories;
            return View("Categories");
        }

        [HttpPost]
        [Route("newcat")]
        public IActionResult NewCat(Category newCat)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(newCat);
                dbContext.SaveChanges();
                return RedirectToAction("Categories", newCat);
            }
            else
            {
                return View("Categories");
            }
        }

        [HttpGet]
        [Route("products/{prodId}")]
        public IActionResult Product(int prodId)
        {
            Product selectedProduct = dbContext.Products
            .Include(prod => prod.Categories)
            .ThenInclude(a => a.Category)
            .FirstOrDefault(prod => prod.ProductId == prodId);
            if (selectedProduct == null)
            {
                return RedirectToAction("Index");
            }

            List<Category> AllCategories = dbContext.Categories.ToList();
            ViewBag.AllCategories = AllCategories;

            return View("Product", selectedProduct);
        }

        [HttpGet]
        [Route("categories/{catId}")]
        public IActionResult Category(int catId)
        {
            Category selectedCat = dbContext.Categories
            .Include(prod => prod.Products)
            .ThenInclude(a => a.Product)
            .FirstOrDefault(prod => prod.CategoryId == catId);
            if (selectedCat == null)
            {
                return RedirectToAction("Index");
            }

            List<Product> AllProducts = dbContext.Products.ToList();
            ViewBag.AllProducts = AllProducts;

            return View("Category", selectedCat);
        }

        [HttpPost]
        [Route("product")]
        public IActionResult AddCatToProd(Association newAss)
        {
            Association existingAss = dbContext.Associations.FirstOrDefault(a => a.ProductId == newAss.ProductId && a.CategoryId == newAss.CategoryId);

            if (existingAss == null)
            {
                dbContext.Associations.Add(newAss);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Product", new { prodId = newAss.ProductId });
        }

        [HttpPost]
        [Route("category")]
        public IActionResult AddProdToCat(Association newAss)
        {
            Association existingAss = dbContext.Associations.FirstOrDefault(a => a.CategoryId == newAss.CategoryId && a.ProductId == newAss.ProductId);

            if (existingAss == null)
            {
                dbContext.Associations.Add(newAss);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Category", new { catId = newAss.CategoryId });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
