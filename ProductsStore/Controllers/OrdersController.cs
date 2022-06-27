using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProductsStore.Models;

namespace ProductsStore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ProductContext _db;

        public OrdersController(ProductContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var orders = _db.Orders.Include(p => p.Product).ToList();
            return View(orders);
        }
        
        [HttpGet]
        public IActionResult Create(int productId)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == productId);
            if (product is null)
                return Content("Такого телефона нет.");
            Order order = new Order
            {
                Product = product
            };
            
            return View(order);
        }
        
        [HttpPost]
        public IActionResult Create(Order order)
        {
            _db.Orders.Add(order);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}