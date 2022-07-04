using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductsStore.Models;

namespace ProductsStore.Controllers
{
    public class UserFeedbacksController : Controller
    {
        private readonly ProductContext _db;

        public UserFeedbacksController(ProductContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Feedback(int productId)
        {
            IEnumerable<int> productEvaluation = new List<int>(){1, 2, 3, 4, 5 };
            ViewBag.ProductEvaluation = new SelectList(productEvaluation);
            ViewBag.ProductId = productId;
            return View();
        }
        
        
        [HttpPost]
        public IActionResult Feedback(UserFeedback userFeedback)
        {
            if (ModelState.IsValid)
            {
                _db.UserFeedbacks.Add(userFeedback);
                _db.SaveChanges();
                return Redirect($"~/Products/About/?productId={userFeedback.ProductId}");
            }
            return View();
        }
    }
}