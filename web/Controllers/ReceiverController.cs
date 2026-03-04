using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

namespace web.Controllers
{
    public class ReceiverController : Controller
    {
        private readonly WelfareDb _context;

        public ReceiverController(WelfareDb context)
        {
            _context = context;
        }

        // Main Dashboard - This is the one being called
        public async Task<IActionResult> ReceiverDashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "UserLogin");
            }

            var user = await _context.UserLoginConfidentials.FindAsync(userId.Value);
            if (user == null || user.UserType != "Receiver")
            {
                return RedirectToAction("Index", "UserLogin");
            }

            // Get user info
            ViewBag.UserId = user.UserId;
            ViewBag.FullName = user.FullName;
            ViewBag.Email = user.Email;
            ViewBag.Phone = user.Phone;
            ViewBag.CNIC = user.CNIC;
            ViewBag.DateOfBirth = user.DateOfBirth.ToString("MMM dd, yyyy");
            ViewBag.Gender = user.Gender;
            ViewBag.City = user.City;
            ViewBag.Address = user.Address;

            // Get all requests for this receiver
            var requests = await _context.ReceiverRequests
                .Where(r => r.ReceiverId == userId.Value)
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();

            ViewBag.TotalRequests = requests.Count;
            ViewBag.PendingRequests = requests.Count(r => r.Status == "Pending");
            ViewBag.ApprovedRequests = requests.Count(r => r.Status == "Approved");
            ViewBag.FulfilledRequests = requests.Count(r => r.Status == "Fulfilled");
            ViewBag.RejectedRequests = requests.Count(r => r.Status == "Rejected");

            // Convert to dynamic list for view
            ViewBag.RequestHistory = requests.Select(r => new
            {
                RequestId = r.RequestId,
                Type = r.RequestType,
                Description = r.Description,
                Amount = r.RequestedAmount,
                Status = r.Status,
                Date = r.RequestDate.ToString("MMM dd, yyyy"),
                Details = GetRequestDetails(r)
            }).ToList<object>();

            // Get welfare fund info for display
            var fund = await _context.WelfareFunds.FirstOrDefaultAsync();
            ViewBag.FoodAvailable = fund?.FoodInventoryUnits ?? 0;
            ViewBag.MaleClothes = fund?.MaleClothesInventory ?? 0;
            ViewBag.FemaleClothes = fund?.FemaleClothesInventory ?? 0;
            ViewBag.KidsClothes = fund?.KidsClothesInventory ?? 0;
            ViewBag.ShelterAvailable = fund?.ShelterAvailable ?? 0;

            return View();
        }

        // Alternative Index action that redirects to ReceiverDashboard
        public IActionResult Index()
        {
            return RedirectToAction("ReceiverDashboard");
        }

        // Request Food
        [HttpPost]
        public async Task<IActionResult> RequestFood(int FamilyMembers, int FoodQuantity, string Description)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "UserLogin");
            }

            var request = new ReceiverRequest
            {
                ReceiverId = userId.Value,
                RequestType = "Food",
                Description = Description ?? "Food assistance needed",
                RequestedAmount = 0,
                FamilyMembers = FamilyMembers,
                FoodQuantity = FoodQuantity,
                Status = "Pending",
                RequestDate = DateTime.Now
            };

            _context.ReceiverRequests.Add(request);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Food request submitted successfully!";
            TempData["JustRequested"] = "food";
            return RedirectToAction("ReceiverDashboard");
        }

        // Request Clothes
        [HttpPost]
        public async Task<IActionResult> RequestClothes(
            int? MaleQuantity,
            int? FemaleQuantity,
            int? KidsQuantity,
            string ClothesType,
            string Description)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "UserLogin");
            }

            var totalQuantity = (MaleQuantity ?? 0) + (FemaleQuantity ?? 0) + (KidsQuantity ?? 0);

            if (totalQuantity == 0)
            {
                TempData["Error"] = "Please specify at least one clothing item.";
                return RedirectToAction("ReceiverDashboard");
            }

            var request = new ReceiverRequest
            {
                ReceiverId = userId.Value,
                RequestType = "Clothes",
                Description = Description ?? "Clothes assistance needed",
                RequestedAmount = 0,
                MaleClothesQuantity = MaleQuantity ?? 0,
                FemaleClothesQuantity = FemaleQuantity ?? 0,
                KidsClothesQuantity = KidsQuantity ?? 0,
                ClothesType = ClothesType,
                Status = "Pending",
                RequestDate = DateTime.Now
            };

            _context.ReceiverRequests.Add(request);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Clothes request submitted successfully!";
            TempData["JustRequested"] = "clothes";
            return RedirectToAction("ReceiverDashboard");
        }

        // Request Loan
        [HttpPost]
        public async Task<IActionResult> RequestLoan(
            decimal Amount,
            string LoanPurpose,
            int RepaymentMonths,
            string Description)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "UserLogin");
            }

            if (Amount < 100)
            {
                TempData["Error"] = "Minimum loan amount is PKR 100.";
                return RedirectToAction("ReceiverDashboard");
            }

            var request = new ReceiverRequest
            {
                ReceiverId = userId.Value,
                RequestType = "Loan",
                Description = Description ?? "Financial assistance needed",
                RequestedAmount = Amount,
                LoanPurpose = LoanPurpose,
                RepaymentMonths = RepaymentMonths,
                Status = "Pending",
                RequestDate = DateTime.Now
            };

            _context.ReceiverRequests.Add(request);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Loan request submitted successfully!";
            TempData["JustRequested"] = "loan";
            return RedirectToAction("ReceiverDashboard");
        }

        // Request Shelter
        [HttpPost]
        public async Task<IActionResult> RequestShelter(
            int DurationDays,
            int RequiredRooms,
            string Description)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "UserLogin");
            }

            var request = new ReceiverRequest
            {
                ReceiverId = userId.Value,
                RequestType = "Shelter",
                Description = Description ?? "Shelter assistance needed",
                RequestedAmount = 0,
                ShelterDurationDays = DurationDays,
                RequiredRooms = RequiredRooms,
                Status = "Pending",
                RequestDate = DateTime.Now
            };

            _context.ReceiverRequests.Add(request);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Shelter request submitted successfully!";
            TempData["JustRequested"] = "shelter";
            return RedirectToAction("ReceiverDashboard");
        }

        // Update Profile
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(
            string FullName,
            string Email,
            string Phone,
            DateTime DateOfBirth,
            string Gender,
            string City,
            string Address,
            string Password,
            string ConfirmPassword)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "UserLogin");
            }

            var user = await _context.UserLoginConfidentials.FindAsync(userId.Value);
            if (user == null)
            {
                return RedirectToAction("Index", "UserLogin");
            }

            // Update basic info
            user.FullName = FullName;
            user.Email = Email;
            user.Phone = Phone;
            user.DateOfBirth = DateOfBirth;
            user.Gender = Gender;
            user.City = City;
            user.Address = Address;

            // Update password if provided
            if (!string.IsNullOrWhiteSpace(Password))
            {
                if (Password != ConfirmPassword)
                {
                    TempData["Error"] = "Passwords do not match!";
                    return RedirectToAction("ReceiverDashboard");
                }

                if (Password.Length < 6)
                {
                    TempData["Error"] = "Password must be at least 6 characters long!";
                    return RedirectToAction("ReceiverDashboard");
                }

                user.PasswordHash = Password; // In production, you should hash this!
            }

            await _context.SaveChangesAsync();

            HttpContext.Session.SetString("UserName", FullName);
            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("ReceiverDashboard");
        }

        // Helper method to format request details
        private string GetRequestDetails(ReceiverRequest request)
        {
            switch (request.RequestType)
            {
                case "Food":
                    return $"{request.FamilyMembers} family members, {request.FoodQuantity} units";
                case "Clothes":
                    var clothesDetails = new List<string>();
                    if (request.MaleClothesQuantity > 0)
                        clothesDetails.Add($"Male: {request.MaleClothesQuantity}");
                    if (request.FemaleClothesQuantity > 0)
                        clothesDetails.Add($"Female: {request.FemaleClothesQuantity}");
                    if (request.KidsClothesQuantity > 0)
                        clothesDetails.Add($"Kids: {request.KidsClothesQuantity}");
                    return string.Join(", ", clothesDetails) + (request.ClothesType != null ? $" ({request.ClothesType})" : "");
                case "Loan":
                    return $"PKR {request.RequestedAmount:N0} for {request.LoanPurpose}";
                case "Shelter":
                    return $"{request.ShelterDurationDays} days, {request.RequiredRooms} room(s)";
                default:
                    return request.Description;
            }
        }
    }
}