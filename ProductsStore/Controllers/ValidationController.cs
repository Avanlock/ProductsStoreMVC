using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductsStore.Models;

namespace ProductsStore.Controllers
{
    public class ValidationController : Controller
    {
        private readonly ProductContext _context;

        public ValidationController(ProductContext context)
        {
            _context = context;
        }

        [AcceptVerbs("GET", "POST")]
        public bool CheckDate(DateTime foundationDate) => foundationDate.Year > 100 && foundationDate < DateTime.Now;

        [AcceptVerbs("GET", "POST")]
        public bool CheckBrand(string name) => !_context.Brands.ToList().Any(b => b.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        
        [AcceptVerbs("GET", "POST")]
        public bool CheckCategory(string name) => !_context.Categories.ToList().Any(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
    }
}