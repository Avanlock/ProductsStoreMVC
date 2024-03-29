﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductsStore.Enums;
using ProductsStore.Models;

namespace ProductsStore.Controllers
{
    
    public class ProductsController : Controller
    {
        private readonly ProductContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ProductContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index(SortState sortState  = SortState.NameAsc)
        {
            IQueryable<Product> products = _db.Products.Include(c => c.Category).Include(b => b.Brand);
            ViewBag.NameSort = sortState == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewBag.BrandSort = sortState == SortState.BrandAsc ? SortState.BrandDesc : SortState.BrandAsc;
            ViewBag.DateSort = sortState == SortState.DateAsc ? SortState.DateDesc : SortState.DateAsc;
            ViewBag.CategorySort = sortState == SortState.CategoryAsc ? SortState.CategoryDesc : SortState.CategoryAsc;
            ViewBag.PriceSort = sortState == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;
            switch (sortState)
            {
                case SortState.NameDesc:
                    products = products.OrderByDescending(s => s.Name);
                    break;
                case SortState.BrandAsc:
                    products = products.OrderBy(s => s.Brand.Name);
                    break;
                case SortState.BrandDesc:
                    products = products.OrderByDescending(s => s.Brand.Name);
                    break;
                case SortState.DateAsc:
                    products = products.OrderBy(s => s.CreateDate);
                    break;
                case SortState.DateDesc:
                    products = products.OrderByDescending(s => s.CreateDate);
                    break;
                case SortState.CategoryAsc:
                    products = products.OrderBy(s => s.Category.Name);
                    break;
                case SortState.CategoryDesc:
                    products = products.OrderByDescending(s => s.Category.Name);
                    break;
                case SortState.PriceAsc:
                    products = products.OrderBy(s => s.Price);
                    break;
                case SortState.PriceDesc:
                    products = products.OrderByDescending(s => s.Price);
                    break;
                default:
                    products = products.OrderBy(s => s.Name);
                    break;
            }
            return View(products.AsNoTracking().ToList());
        }
        
        [HttpGet]
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        public IActionResult Add(Product product)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count != 0)
                {
                    string webRootPath = _webHostEnvironment.WebRootPath;
                    string upload = webRootPath + WebConsts.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    product.ImgUrl = fileName + extension;
                }
                else
                {
                    product.ImgUrl = "default-image.png";
                }
                product.CreateDate = DateTime.Now.ToString("dd/MM/yyyy/ HH:mm");
                product.UpdateDate = "Товар не редактировался";
                _db.Products.Add(product);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }
        
        [HttpGet]
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count != 0)
                {
                    var oldFile = Path.Combine(_webHostEnvironment.WebRootPath + WebConsts.ImagePath, product.ImgUrl);
                    if (System.IO.File.Exists(oldFile))
                        System.IO.File.Delete(oldFile);
                    string webRootPath = _webHostEnvironment.WebRootPath;
                    string upload = webRootPath + WebConsts.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    product.ImgUrl = fileName + extension;
                }
                product.UpdateDate = DateTime.Now.ToString("dd/MM/yyyy/ HH:mm");
                _db.Products.Update(product);
                _db.SaveChanges();
                return Redirect($"~/Products/About/?productId={product.Id}");
            }
            return View();
        }
        
        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int productId)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == productId);
            if (product is null)
                return Content("Такого телефона нет.");
            var imgPath = Path.Combine(_webHostEnvironment.WebRootPath + WebConsts.ImagePath, product.ImgUrl);
            if (System.IO.File.Exists(imgPath))
                System.IO.File.Delete(imgPath);
            _db.Products.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        [Authorize(Roles = "user, admin")]
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