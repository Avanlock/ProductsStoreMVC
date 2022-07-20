using System.Linq;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            var orders = _db.Orders.Include(p => p.Product).ToList();
            return View(orders);
        }
        
        [HttpGet]
        [Authorize(Roles = "user")]
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
        [Authorize(Roles = "user")]
        public IActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                order.UserName = User.Identity.Name;
                _db.Orders.Add(order);
                _db.SaveChanges();
                return Redirect($"~/Products/About/?productId={order.ProductId}");
            }

            return View();
        }
    }
}