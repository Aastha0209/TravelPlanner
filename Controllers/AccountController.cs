using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Data;
using TravelPlanner.Models;
using System.Linq;
using BCrypt.Net;

namespace TravelPlanner.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ---------- REGISTER ----------
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    ViewBag.Error = "Email already registered!";
                    return View();
                }

                // ✅ Hash the password using BCrypt
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

                _context.Users.Add(user);
                _context.SaveChanges();

                ViewBag.Message = "Registration successful! Please login.";
                return RedirectToAction("Login");
            }
            return View();
        }

        // ---------- LOGIN ----------
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            // ✅ Verify BCrypt hash
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                ViewBag.Error = "Invalid email or password!";
                return View();
            }

            // ✅ Store user info in session
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserName", user.Name);

            return RedirectToAction("Dashboard", "Home");
        }

        // ---------- LOGOUT ----------
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
