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
            if (ModelState.IsValid)
            {
                _db.Brands.Add(brand);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
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
        
        [HttpGet]
        public IActionResult Edit(int brandId)
        {
            if (ModelState.IsValid)
            {
                var brand = _db.Brands.FirstOrDefault(b => b.Id == brandId);
                if (brand is null)
                {
                    return BadRequest();
                }
                return View(brand);
            }
            return View();
        }
        

        [HttpPost]
        public IActionResult Edit(Brand brand)
        {
            if (ModelState.IsValid)
            {
                _db.Brands.Update(brand);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}