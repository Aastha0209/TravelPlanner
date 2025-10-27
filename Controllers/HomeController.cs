using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TravelPlanner.Data;
using TravelPlanner.Models;

namespace TravelPlanner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public HomeController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // ---------- INDEX ----------
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
                return RedirectToAction("Dashboard");

            return RedirectToAction("Login", "Account");
        }

        // ---------- DASHBOARD ----------
        public IActionResult Dashboard()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            var trips = _context.Trips
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.StartDate)
                .ToList();

            foreach (var trip in trips)
            {
                trip.Itineraries = _context.Itineraries.Where(i => i.TripId == trip.TripId).ToList();
                trip.Images = _context.TripImages.Where(img => img.TripId == trip.TripId).ToList();
            }

            return View(trips);
        }

        // ---------- CREATE TRIP ----------
        [HttpGet]
        public IActionResult CreateTrip()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var trip = new Trip { StartDate = DateTime.Today, EndDate = DateTime.Today };
            return View(trip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTrip(Trip trip)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(trip);

            trip.UserId = userId.Value;
            _context.Trips.Add(trip);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Trip created successfully!";
            return RedirectToAction("Dashboard");
        }

        // ---------- EDIT TRIP ----------
        [HttpGet]
        public IActionResult EditTrip(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var trip = _context.Trips.FirstOrDefault(t => t.TripId == id && t.UserId == userId);
            if (trip == null)
                return NotFound();

            return View(trip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTrip(Trip trip)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var existingTrip = _context.Trips.FirstOrDefault(t => t.TripId == trip.TripId && t.UserId == userId);
            if (existingTrip == null)
                return NotFound();

            existingTrip.Name = trip.Name;
            existingTrip.Destination = trip.Destination;
            existingTrip.StartDate = trip.StartDate;
            existingTrip.EndDate = trip.EndDate;

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Trip details updated!";
            return RedirectToAction("Dashboard");
        }

        // ---------- DELETE TRIP ----------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTrip(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var trip = _context.Trips.FirstOrDefault(t => t.TripId == id && t.UserId == userId);
            if (trip != null)
            {
                var items = _context.Itineraries.Where(i => i.TripId == trip.TripId).ToList();
                _context.Itineraries.RemoveRange(items);

                var images = _context.TripImages.Where(img => img.TripId == trip.TripId).ToList();
                foreach (var img in images)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", img.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }

                _context.TripImages.RemoveRange(images);
                _context.Trips.Remove(trip);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Trip deleted successfully!";
            }

            return RedirectToAction("Dashboard");
        }

        // ---------- ITINERARY ----------
        [HttpGet]
        public IActionResult Itinerary(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var trip = _context.Trips.FirstOrDefault(t => t.TripId == id && t.UserId == userId);
            if (trip == null)
                return NotFound();

            trip.Itineraries = _context.Itineraries.Where(i => i.TripId == id).OrderBy(i => i.Date).ToList();
            trip.Images = _context.TripImages.Where(img => img.TripId == id).ToList();

            return View(trip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddItineraryItem(Itinerary item)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var trip = _context.Trips.FirstOrDefault(t => t.TripId == item.TripId && t.UserId == userId);
            if (trip == null)
            {
                TempData["ErrorMessage"] = "Trip not found or unauthorized access.";
                return RedirectToAction("Dashboard");
            }

            if (string.IsNullOrWhiteSpace(item.Name) || string.IsNullOrWhiteSpace(item.Type) || item.Date == null)
            {
                TempData["ErrorMessage"] = "Please fill all required fields before saving.";
                return RedirectToAction("Itinerary", new { id = item.TripId });
            }

            _context.Itineraries.Add(item);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Itinerary item added successfully!";
            return RedirectToAction("Itinerary", new { id = item.TripId });
        }

        // ---------- IMAGE UPLOAD ----------
        [HttpPost]
        public IActionResult UploadTripImage(IFormFile file, int tripId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var uploadsPath = Path.Combine(_environment.WebRootPath, "images");
                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                var filePath = Path.Combine(uploadsPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    file.CopyTo(stream);

                _context.TripImages.Add(new TripImage
                {
                    TripId = tripId,
                    ImageUrl = "/images/" + fileName
                });

                _context.SaveChanges();
                TempData["SuccessMessage"] = "Image uploaded successfully!";
            }

            return RedirectToAction("Itinerary", new { id = tripId });
        }

        // ---------- DELETE IMAGE ----------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTripImage(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var image = _context.TripImages.FirstOrDefault(img => img.TripImageId == id);
            if (image != null)
            {
                var trip = _context.Trips.FirstOrDefault(t => t.TripId == image.TripId && t.UserId == userId);
                if (trip != null)
                {
                    var filePath = Path.Combine(_environment.WebRootPath, image.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);

                    _context.TripImages.Remove(image);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Image deleted.";
                }
            }

            return RedirectToAction("Itinerary", new { id = image?.TripId });
        }

        // ---------- PROFILE ----------
        [HttpGet]
        public IActionResult Profile()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(string Name, string Email, string Password, IFormFile ProfilePicture)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                return NotFound();

            try
            {
                user.Name = Name;
                user.Email = Email;

                if (!string.IsNullOrWhiteSpace(Password))
                {
                    using var sha256 = SHA256.Create();
                    var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(Password));
                    user.PasswordHash = BitConverter.ToString(bytes).Replace("-", "").ToLower();
                }

                if (ProfilePicture != null && ProfilePicture.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "profile");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(ProfilePicture.FileName)}";
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await ProfilePicture.CopyToAsync(stream);

                    user.ProfilePicture = "/images/profile/" + fileName;
                }

                _context.SaveChanges();

                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetString("UserEmail", user.Email);

                ViewBag.Message = "Profile updated successfully!";
                return View("Profile", user);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error updating profile: " + ex.Message;
                return View("Profile", user);
            }
        }

        // ---------- LOGOUT ----------
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
