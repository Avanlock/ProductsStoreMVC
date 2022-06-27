using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductsStore.Models;

namespace ProductsStore.Controllers
{
    public class BrandsController : Controller
    {
        private readonly ProductContext _db;

        public BrandsController(ProductContext db)
        {
            _db = db;
        }
        
        public IActionResult Index()
        {
            var brands = _db.Brands.ToList();
            return View(brands);
        }
        
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Add(Brand brand)
        {
            if (!_db.Brands.Any(b => b.Name.Equals(brand.Name)))
            {
                if (brand != null)
                {
                    _db.Brands.Add(brand);
                    _db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return Content("Такой бренд уже есть!");
        }
        
        [HttpGet]
        public IActionResult Delete(int brandId)
        {
            var brand = _db.Brands.FirstOrDefault(b => b.Id == brandId);
            if (brand is null)
                return Content("Такого телефона нет.");
            _db.Brands.Remove(brand);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}