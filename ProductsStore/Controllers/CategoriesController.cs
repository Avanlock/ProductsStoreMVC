using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductsStore.Models;

namespace ProductsStore.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ProductContext _db;

        public CategoriesController(ProductContext db)
        {
            _db = db;
        }
        
        public IActionResult Index()
        {
            var categories = _db.Categories.ToList();
            return View(categories);
        }
        
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        
        
        [HttpPost]
        public IActionResult Add(Category category)
        {
            if (category != null)
            {
                if (!_db.Categories.Any(c => c.Name.Equals(category.Name)))
                {
                    _db.Categories.Add(category);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return Content("Такая категория уже есть!");
        }
        
        
        [HttpGet]
        public IActionResult Delete(int categoryId)
        {
            var category = _db.Categories.FirstOrDefault(c => c.Id == categoryId);
            if (category is null)
                return Content("Такого телефона нет.");
            _db.Categories.Remove(category);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}