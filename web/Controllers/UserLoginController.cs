using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

namespace web.Controllers
{
    public class UserLoginController : Controller
    {
        private readonly WelfareDb _context;

        public UserLoginController(WelfareDb context)
        {
            _context = context;
        }

        // GET: show login page
        public IActionResult Index()
        {
            return View();
        }

        // POST: handle login for user / admin / ngo based on hidden LoginType
        [HttpPost]
        public async Task<IActionResult> Index(string LoginType, string UserID, string Password)
        {
            // Preserve the login type for the view in case of error
            ViewBag.LoginType = LoginType ?? "user";

            if (string.IsNullOrWhiteSpace(UserID) || string.IsNullOrWhiteSpace(Password))
            {
                ViewBag.Error = "Please provide both ID and password.";
                return View();
            }

            if (!int.TryParse(UserID, out var id))
            {
                ViewBag.Error = "ID must be a numeric value.";
                return View();
            }

            LoginType = (LoginType ?? "user").ToLowerInvariant();

            switch (LoginType)
            {
                case "ngo":
                    var ngo = await _context.NGOsLogins.FindAsync(id);
                    if (ngo != null && ngo.PasswordHash == Password)
                    {
                        // Check if NGO is active
                        if (!ngo.IsActive)
                        {
                            ViewBag.Error = "Your NGO account has been deactivated. Please contact support.";
                            return View();
                        }

                        // FIXED: Changed from "NGOId" to "NgoId" to match NGOController
                        HttpContext.Session.SetInt32("NgoId", id);
                        HttpContext.Session.SetString("UserType", "NGO");
                        TempData["Success"] = "NGO login successful.";
                        return RedirectToAction("Index", "NGO");
                    }
                    break;

                case "admin":
                    var admin = await _context.AdminLogins.FindAsync(id);
                    if (admin != null && admin.passwordHash == Password)
                    {
                        HttpContext.Session.SetInt32("AdminId", id);
                        HttpContext.Session.SetString("UserType", "Admin");
                        TempData["Success"] = $"Welcome back, {admin.FullName}!";
                        // FIXED: Changed from "Dashboard" to "Index"
                        return RedirectToAction("Index", "Admin");
                    }
                    break;

                default: // user (Donor or Receiver)
                    var user = await _context.UserLoginConfidentials.FindAsync(id);

                    // DEBUG: Log what we found
                    Console.WriteLine($"DEBUG: User found: {user != null}");
                    if (user != null)
                    {
                        Console.WriteLine($"DEBUG: User ID: {user.UserId}");
                        Console.WriteLine($"DEBUG: User Type: {user.UserType}");
                        Console.WriteLine($"DEBUG: Password Match: {user.PasswordHash == Password}");
                        Console.WriteLine($"DEBUG: IsActive: {user.IsActive}");
                    }

                    if (user != null && user.PasswordHash == Password)
                    {
                        // Check if user is active
                        if (!user.IsActive)
                        {
                            ViewBag.Error = "Your account has been deactivated. Please contact support.";
                            return View();
                        }

                        // Store user info in session
                        HttpContext.Session.SetInt32("UserId", user.UserId);
                        HttpContext.Session.SetString("UserName", user.FullName);
                        HttpContext.Session.SetString("UserType", user.UserType);

                        // DEBUG: Verify session was set
                        Console.WriteLine($"DEBUG: Session UserId set: {HttpContext.Session.GetInt32("UserId")}");
                        Console.WriteLine($"DEBUG: Session UserType set: {HttpContext.Session.GetString("UserType")}");

                        // Set success message
                        TempData["Success"] = $"Welcome back, {user.FullName}!";

                        // Redirect based on UserType column
                        if (user.UserType.Equals("Donor", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("DEBUG: Redirecting to Donor Dashboard");
                            return RedirectToAction("Index", "DonorDashboard");
                        }
                        else if (user.UserType.Equals("Receiver", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("DEBUG: Redirecting to Receiver Dashboard");
                            return RedirectToAction("ReceiverDashboard", "Receiver");
                        }
                        else
                        {
                            Console.WriteLine($"DEBUG: Invalid UserType: {user.UserType}");
                            ViewBag.Error = "Invalid user type. Please contact administrator.";
                            return View();
                        }
                    }

                    Console.WriteLine("DEBUG: Authentication failed - no match found");
                    break;
            }

            ViewBag.Error = "Invalid ID or password.";
            return View();
        }

        public IActionResult Register()
        {
            // Clear any existing ModelState to prevent validation errors on GET
            ModelState.Clear();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserLoginConfidentials loginConfidentials)
        {
            // Remove navigation property errors from ModelState
            ModelState.Remove("ReceiverRequests");
            ModelState.Remove("DonorTransactions");

            // Check for duplicates BEFORE ModelState validation
            if (await _context.UserLoginConfidentials.AnyAsync(u => u.UserId == loginConfidentials.UserId))
            {
                ModelState.AddModelError("UserId", "User ID already exists. Please choose a different one.");
                loginConfidentials.PasswordHash = ""; // Clear password
                return View(loginConfidentials);
            }

            if (await _context.UserLoginConfidentials.AnyAsync(u => u.CNIC == loginConfidentials.CNIC))
            {
                ModelState.AddModelError("CNIC", "CNIC already registered. Please use a different CNIC.");
                loginConfidentials.PasswordHash = ""; // Clear password
                return View(loginConfidentials);
            }

            if (await _context.UserLoginConfidentials.AnyAsync(u => u.Email == loginConfidentials.Email))
            {
                ModelState.AddModelError("Email", "Email already registered. Please use a different email.");
                loginConfidentials.PasswordHash = ""; // Clear password
                return View(loginConfidentials);
            }

            if (ModelState.IsValid)
            {
                // Set registration date and active status
                loginConfidentials.RegistrationDate = DateTime.Now;
                loginConfidentials.IsActive = true;

                await _context.UserLoginConfidentials.AddAsync(loginConfidentials);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Registration successful! Please login with your credentials.";
                return RedirectToAction("Index");
            }

            // Clear password on any validation error
            loginConfidentials.PasswordHash = "";
            return View(loginConfidentials);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Logged out successfully.";
            return RedirectToAction("Index");
        }
    }
}