using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Data;
using TravelPlanner.Models;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

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

                // Hash the password before saving
                user.PasswordHash = HashPassword(user.PasswordHash);

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

            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                ViewBag.Error = "Invalid email or password!";
                return View();
            }

            // Store user info in session
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

        // ---------- Password Hashing ----------
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            string enteredHash = HashPassword(enteredPassword);
            return enteredHash == storedHash;
        }
    }
}
