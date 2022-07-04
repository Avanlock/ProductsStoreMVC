using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductsStore.Models;

namespace ProductsStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductContext _db;

        public ProductsController(ProductContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var phones = _db.Products.ToList();
            return View(phones);
        }
        
        [HttpGet]
        public IActionResult Add()
        {
            IEnumerable<SelectListItem> categoryItems = _db.Categories.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            
            IEnumerable<SelectListItem> brandItems = _db.Brands.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            
            ViewBag.brandItems = brandItems;
            ViewBag.categoryItems = categoryItems;
            return View();
        }
        
        [HttpPost]
        public IActionResult Add(Product product)
        {
            if (ModelState.IsValid)
            {
                product.UpdateDate = "Товар не редактировался";
                _db.Products.Add(product);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }
        
        [HttpGet]
        public IActionResult Edit(int productId)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<SelectListItem> categoryItems = _db.Categories.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            
                IEnumerable<SelectListItem> brandItems = _db.Brands.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                ViewBag.brandItems = brandItems;
                ViewBag.categoryItems = categoryItems;
                var product = _db.Products.FirstOrDefault(p => p.Id == productId);
                if (product is null)
                    return BadRequest();
                return View(product);
            }
            return View();
        }
        

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                product.UpdateDate = DateTime.Now.ToString("dd/MM/yyyy/ HH:mm");
                _db.Products.Update(product);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        
        [HttpGet]
        public IActionResult Delete(int productId)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == productId);
            if (product is null)
                return Content("Такого телефона нет.");
            _db.Products.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public IActionResult About(int productId)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == productId);
            if (product is null)
                return Content("Такого телефона нет.");
            product.Category = _db.Categories.Find(product.CategoryId);
            product.Brand = _db.Brands.Find(product.BrandId);
            return View(product);
        }
    }
}