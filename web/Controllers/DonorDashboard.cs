using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

namespace web.Controllers
{
    public class DonorDashboardController : Controller
    {
        private readonly WelfareDb _context;

        public DonorDashboardController(WelfareDb context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue || userId == 0)
            {
                TempData["Error"] = "Please login first.";
                return RedirectToAction("Index", "UserLogin");
            }

            var user = _context.UserLoginConfidentials.FirstOrDefault(u => u.UserId == userId.Value);
            if (user == null || !user.UserType.Trim().Equals("Donor", StringComparison.OrdinalIgnoreCase))
            {
                HttpContext.Session.Clear();
                TempData["Error"] = "Access denied.";
                return RedirectToAction("Index", "UserLogin");
            }

            ViewBag.UserId = user.UserId;
            ViewBag.FullName = user.FullName;
            ViewBag.Email = user.Email;
            ViewBag.Phone = user.Phone;
            ViewBag.CNIC = user.CNIC;
            ViewBag.DateOfBirth = user.DateOfBirth.ToString("dd MMM yyyy");
            ViewBag.Gender = user.Gender;
            ViewBag.Address = user.Address;
            ViewBag.City = user.City;

            ViewBag.DonationHistory = GetDonationHistory(userId.Value);
            ViewBag.TotalDonations = ViewBag.DonationHistory.Count;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DonateFoodAction(int NumberOfPeople, string Description, DateTime PickupDateTime)
        {
            System.Diagnostics.Debug.WriteLine("=== FOOD DONATION ACTION ===");
            System.Diagnostics.Debug.WriteLine($"People: {NumberOfPeople}");
            System.Diagnostics.Debug.WriteLine($"Description: {Description}");
            System.Diagnostics.Debug.WriteLine($"PickupDateTime: {PickupDateTime}");

            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                TempData["Error"] = "Session expired. Please login again.";
                return RedirectToAction("Index", "UserLogin");
            }

            System.Diagnostics.Debug.WriteLine($"UserId: {userId.Value}");

            if (NumberOfPeople <= 0 || PickupDateTime <= DateTime.Now.AddHours(-1))
            {
                TempData["Error"] = "Invalid data. Check number of people and pickup time.";
                return RedirectToAction("Index");
            }

            try
            {
                var fund = await _context.WelfareFunds.FirstOrDefaultAsync();
                if (fund == null)
                {
                    fund = new WelfareFund
                    {
                        CurrentBalance = 0,
                        FoodInventoryUnits = 0,
                        MaleClothesInventory = 0,
                        FemaleClothesInventory = 0,
                        KidsClothesInventory = 0,
                        LastUpdated = DateTime.Now
                    };
                    _context.WelfareFunds.Add(fund);
                    await _context.SaveChangesAsync();
                    System.Diagnostics.Debug.WriteLine("Created new WelfareFund");
                }

                fund.FoodInventoryUnits += NumberOfPeople;
                fund.LastUpdated = DateTime.Now;

                var tx = new DonorToWelfareTransaction
                {
                    DonorUserId = userId.Value,
                    DonationType = "Food",
                    FoodQuantity = NumberOfPeople,
                    FoodDescription = Description ?? "Food donation",
                    DonationDate = DateTime.Now,
                    ItemDescription = $"Food for {NumberOfPeople} people - Pickup: {PickupDateTime:dd MMM yyyy hh:mm tt}",
                    FoodInventoryAfter = fund.FoodInventoryUnits,
                    WelfareBalanceAfter = fund.CurrentBalance
                };

                _context.DonorToWelfareTransactions.Add(tx);
                await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine($"SAVED! Transaction ID: {tx.TransactionId}");

                TempData["Success"] = $"Thank you! Food for {NumberOfPeople} people recorded.";
                TempData["JustDonated"] = "food";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("=== FULL ERROR DETAILS ===");
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.ToString());
                System.Diagnostics.Debug.WriteLine("Message: " + ex.Message);
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine("Inner Exception: " + ex.InnerException.ToString());
                    System.Diagnostics.Debug.WriteLine("Inner Message: " + ex.InnerException.Message);
                }
                System.Diagnostics.Debug.WriteLine("Stack Trace: " + ex.StackTrace);

                TempData["Error"] = "Database error: " + ex.Message + (ex.InnerException != null ? " | Inner: " + ex.InnerException.Message : "");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DonateClothesAction(string Category, int Quantity, string Condition,
            string Description, string PickupLocation, DateTime PickupDateTime)
        {
            System.Diagnostics.Debug.WriteLine("=== CLOTHES DONATION ACTION ===");
            System.Diagnostics.Debug.WriteLine($"Category: {Category}");
            System.Diagnostics.Debug.WriteLine($"Quantity: {Quantity}");
            System.Diagnostics.Debug.WriteLine($"Condition/Type: {Condition}");
            System.Diagnostics.Debug.WriteLine($"Description: {Description}");
            System.Diagnostics.Debug.WriteLine($"PickupLocation: {PickupLocation}");
            System.Diagnostics.Debug.WriteLine($"PickupDateTime: {PickupDateTime}");

            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                TempData["Error"] = "Session expired. Please login again.";
                return RedirectToAction("Index", "UserLogin");
            }

            System.Diagnostics.Debug.WriteLine($"UserId: {userId.Value}");

            if (string.IsNullOrEmpty(Category) || Quantity <= 0 || PickupDateTime <= DateTime.Now.AddHours(-1))
            {
                TempData["Error"] = "Invalid data. Check all required fields.";
                return RedirectToAction("Index");
            }

            try
            {
                var fund = await _context.WelfareFunds.FirstOrDefaultAsync();
                if (fund == null)
                {
                    fund = new WelfareFund
                    {
                        CurrentBalance = 0,
                        FoodInventoryUnits = 0,
                        MaleClothesInventory = 0,
                        FemaleClothesInventory = 0,
                        KidsClothesInventory = 0,
                        LastUpdated = DateTime.Now
                    };
                    _context.WelfareFunds.Add(fund);
                    await _context.SaveChangesAsync();
                }

                // Update inventory based on category
                if (Category == "Male")
                    fund.MaleClothesInventory += Quantity;
                else if (Category == "Female")
                    fund.FemaleClothesInventory += Quantity;
                else if (Category == "Kids")
                    fund.KidsClothesInventory += Quantity;

                fund.LastUpdated = DateTime.Now;

                var tx = new DonorToWelfareTransaction
                {
                    DonorUserId = userId.Value,
                    DonationType = "Clothes",
                    MaleClothesQuantity = Category == "Male" ? Quantity : (int?)null,
                    FemaleClothesQuantity = Category == "Female" ? Quantity : (int?)null,
                    KidsClothesQuantity = Category == "Kids" ? Quantity : (int?)null,
                    ClothesType = Condition,
                    ClothesDescription = Description ?? $"{Category} clothes donation",
                    DonationDate = DateTime.Now,
                    ItemDescription = $"{Quantity} {Category} clothes - {Condition} - Pickup: {PickupDateTime:dd MMM yyyy hh:mm tt} at {PickupLocation}",
                    MaleClothesInventoryAfter = fund.MaleClothesInventory,
                    FemaleClothesInventoryAfter = fund.FemaleClothesInventory,
                    KidsClothesInventoryAfter = fund.KidsClothesInventory,
                    WelfareBalanceAfter = fund.CurrentBalance
                };

                _context.DonorToWelfareTransactions.Add(tx);
                await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine($"SAVED! Transaction ID: {tx.TransactionId}");

                TempData["Success"] = $"Thank you! {Quantity} {Category} clothes recorded.";
                TempData["JustDonated"] = "clothes";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("=== FULL ERROR DETAILS ===");
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.ToString());
                System.Diagnostics.Debug.WriteLine("Message: " + ex.Message);
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine("Inner Exception: " + ex.InnerException.ToString());
                    System.Diagnostics.Debug.WriteLine("Inner Message: " + ex.InnerException.Message);
                }
                System.Diagnostics.Debug.WriteLine("Stack Trace: " + ex.StackTrace);

                TempData["Error"] = "Database error: " + ex.Message + (ex.InnerException != null ? " | Inner: " + ex.InnerException.Message : "");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DonateMoneyAction(decimal Amount, string Comments)
        {
            System.Diagnostics.Debug.WriteLine("=== MONEY DONATION ACTION ===");
            System.Diagnostics.Debug.WriteLine($"Amount: {Amount}");
            System.Diagnostics.Debug.WriteLine($"Comments: {Comments}");

            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                TempData["Error"] = "Session expired. Please login again.";
                return RedirectToAction("Index", "UserLogin");
            }

            System.Diagnostics.Debug.WriteLine($"UserId: {userId.Value}");

            if (Amount < 100)
            {
                TempData["Error"] = "Minimum donation amount is PKR 100.";
                return RedirectToAction("Index");
            }

            try
            {
                var fund = await _context.WelfareFunds.FirstOrDefaultAsync();
                if (fund == null)
                {
                    fund = new WelfareFund
                    {
                        CurrentBalance = 0,
                        FoodInventoryUnits = 0,
                        MaleClothesInventory = 0,
                        FemaleClothesInventory = 0,
                        KidsClothesInventory = 0,
                        LastUpdated = DateTime.Now
                    };
                    _context.WelfareFunds.Add(fund);
                    await _context.SaveChangesAsync();
                }

                fund.CurrentBalance += Amount;
                fund.LastUpdated = DateTime.Now;

                var tx = new DonorToWelfareTransaction
                {
                    DonorUserId = userId.Value,
                    DonationType = "Money",
                    MonetaryAmount = Amount,
                    DonationDate = DateTime.Now,
                    ItemDescription = Comments ?? $"Monetary donation of PKR {Amount:N0}",
                    WelfareBalanceAfter = fund.CurrentBalance,
                    FoodInventoryAfter = fund.FoodInventoryUnits
                };

                _context.DonorToWelfareTransactions.Add(tx);
                await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine($"SAVED! Transaction ID: {tx.TransactionId}");

                TempData["Success"] = $"Thank you! PKR {Amount:N0} donated successfully.";
                TempData["JustDonated"] = "money";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("=== FULL ERROR DETAILS ===");
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.ToString());
                System.Diagnostics.Debug.WriteLine("Message: " + ex.Message);
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine("Inner Exception: " + ex.InnerException.ToString());
                    System.Diagnostics.Debug.WriteLine("Inner Message: " + ex.InnerException.Message);
                }
                System.Diagnostics.Debug.WriteLine("Stack Trace: " + ex.StackTrace);

                TempData["Error"] = "Database error: " + ex.Message + (ex.InnerException != null ? " | Inner: " + ex.InnerException.Message : "");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(string FullName, string Email, string Phone, DateTime DateOfBirth,
            string Gender, string City, string Address, string CurrentPassword, string NewPassword, string ConfirmPassword)
        {
            System.Diagnostics.Debug.WriteLine("=== UPDATE PROFILE ACTION ===");
            System.Diagnostics.Debug.WriteLine($"FullName: {FullName}");
            System.Diagnostics.Debug.WriteLine($"Email: {Email}");
            System.Diagnostics.Debug.WriteLine($"Phone: {Phone}");
            System.Diagnostics.Debug.WriteLine($"DateOfBirth: {DateOfBirth}");
            System.Diagnostics.Debug.WriteLine($"Gender: {Gender}");
            System.Diagnostics.Debug.WriteLine($"City: {City}");
            System.Diagnostics.Debug.WriteLine($"Address: {Address}");
            System.Diagnostics.Debug.WriteLine($"Password Change Attempted: {!string.IsNullOrEmpty(CurrentPassword)}");

            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                TempData["Error"] = "Session expired. Please login again.";
                return RedirectToAction("Index", "UserLogin");
            }

            System.Diagnostics.Debug.WriteLine($"UserId: {userId.Value}");

            // Validate inputs
            if (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Phone) || string.IsNullOrWhiteSpace(Gender) ||
                string.IsNullOrWhiteSpace(City) || string.IsNullOrWhiteSpace(Address))
            {
                TempData["Error"] = "All basic information fields are required.";
                return RedirectToAction("Index");
            }

            if (Phone.Length != 11 || !Phone.All(char.IsDigit))
            {
                TempData["Error"] = "Phone number must be exactly 11 digits.";
                return RedirectToAction("Index");
            }

            // Validate date of birth (must be on or before Dec 31, 2010)
            var maxDate = new DateTime(2010, 12, 31);
            if (DateOfBirth > maxDate)
            {
                TempData["Error"] = "You must be born on or before December 31, 2010 (at least 15 years old).";
                return RedirectToAction("Index");
            }

            // Validate password change if attempted
            bool passwordChangeAttempted = !string.IsNullOrEmpty(CurrentPassword) ||
                                          !string.IsNullOrEmpty(NewPassword) ||
                                          !string.IsNullOrEmpty(ConfirmPassword);

            if (passwordChangeAttempted)
            {
                if (string.IsNullOrEmpty(CurrentPassword))
                {
                    TempData["Error"] = "Please enter your current password to change it.";
                    return RedirectToAction("Index");
                }
                if (string.IsNullOrEmpty(NewPassword))
                {
                    TempData["Error"] = "Please enter a new password.";
                    return RedirectToAction("Index");
                }
                if (string.IsNullOrEmpty(ConfirmPassword))
                {
                    TempData["Error"] = "Please confirm your new password.";
                    return RedirectToAction("Index");
                }
                if (NewPassword != ConfirmPassword)
                {
                    TempData["Error"] = "New passwords do not match.";
                    return RedirectToAction("Index");
                }
                if (NewPassword.Length < 6)
                {
                    TempData["Error"] = "New password must be at least 6 characters.";
                    return RedirectToAction("Index");
                }
            }

            try
            {
                var user = await _context.UserLoginConfidentials.FindAsync(userId.Value);
                if (user == null)
                {
                    TempData["Error"] = "User not found.";
                    return RedirectToAction("Index", "UserLogin");
                }

                // If password change is attempted, verify current password
                if (passwordChangeAttempted)
                {
                    if (user.PasswordHash != CurrentPassword)
                    {
                        TempData["Error"] = "Current password is incorrect.";
                        return RedirectToAction("Index");
                    }

                    // Update password
                    user.PasswordHash = NewPassword;
                    System.Diagnostics.Debug.WriteLine("Password updated successfully!");
                }

                // Update user information
                user.FullName = FullName.Trim();
                user.Email = Email.Trim();
                user.Phone = Phone.Trim();
                user.DateOfBirth = DateOfBirth;
                user.Gender = Gender.Trim();
                user.City = City.Trim();
                user.Address = Address.Trim();

                _context.UserLoginConfidentials.Update(user);
                await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine("Profile updated successfully!");

                // Update session
                HttpContext.Session.SetString("UserName", user.FullName);

                if (passwordChangeAttempted)
                {
                    TempData["Success"] = "Profile and password updated successfully!";
                }
                else
                {
                    TempData["Success"] = "Profile updated successfully!";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("=== PROFILE UPDATE ERROR ===");
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.ToString());
                System.Diagnostics.Debug.WriteLine("Message: " + ex.Message);
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }

                TempData["Error"] = "Failed to update profile: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        private List<object> GetDonationHistory(int userId)
        {
            var list = new List<object>();
            var transactions = _context.DonorToWelfareTransactions
                .Where(t => t.DonorUserId == userId)
                .OrderByDescending(t => t.DonationDate)
                .Take(50)
                .ToList();

            foreach (var t in transactions)
            {
                string desc = t.DonationType switch
                {
                    "Food" => $"{t.FoodQuantity ?? 0} people",
                    "Money" => $"PKR {t.MonetaryAmount:N0}",
                    "Clothes" => $"{((t.MaleClothesQuantity ?? 0) + (t.FemaleClothesQuantity ?? 0) + (t.KidsClothesQuantity ?? 0))} items",
                    _ => t.ItemDescription ?? "Donation"
                };

                list.Add(new
                {
                    Date = t.DonationDate.ToString("dd MMM yyyy, hh:mm tt"),
                    Type = t.DonationType,
                    Description = desc,
                    Details = t.FoodDescription ?? t.ClothesDescription ?? t.ItemDescription ?? ""
                });
            }
            return list;
        }
    }
}