using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsStore.Models;
using ProductsStore.ViewModels;

namespace ProductsStore.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ProductContext _db;

        public AccountsController(ProductContext db)
        {
            _db = db;
        }
        
        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            ClaimsIdentity id = new ClaimsIdentity(
                claims,
                "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(id),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(1)
                }
            );
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _db.Users.Include(r=> r.Role).FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user is not null)
                {
                    if (user.Password.Equals(model.Password))
                    {
                        await Authenticate(user);
                        return RedirectToAction("Index", "Products");
                    }
                }
                else
                    ModelState.AddModelError("", "Некорректный логин или пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user is null)
                {
                    Role role = _db.Roles.FirstOrDefault(r => r.Name == "user");
                    user = new User {Email = model.Email, UserName = model.UserName, Password = model.Password, RoleId = 2, Role = role};
                    _db.Users.Add(user);
                    await _db.SaveChangesAsync();
                    await Authenticate(user);
                    return RedirectToAction("Index", "Products");
                }
                ModelState.AddModelError("", "Некорректные логин или пароль");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Products");
        }
    }
}