using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

namespace web.Controllers
{
    public class NGOController : Controller
    {
        private readonly WelfareDb _context;

        public NGOController(WelfareDb context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ngoId = HttpContext.Session.GetInt32("NgoId");
            if (ngoId == null)
            {
                TempData["Error"] = "Please login first.";
                return RedirectToAction("Index", "UserLogin");
            }

            var ngo = await _context.NGOsLogins.FindAsync(ngoId.Value);
            if (ngo == null)
            {
                HttpContext.Session.Clear();
                TempData["Error"] = "Access denied.";
                return RedirectToAction("Index", "UserLogin");
            }

            if (!ngo.IsActive)
            {
                HttpContext.Session.Clear();
                TempData["Error"] = "Your account has been deactivated. Please contact admin.";
                return RedirectToAction("Index", "UserLogin");
            }

            ViewBag.NgoId = ngo.NgoId;
            ViewBag.OrganizationName = ngo.OrganizationName;
            ViewBag.Email = ngo.Email;
            ViewBag.Phone = ngo.Phone;
            ViewBag.Address = ngo.Address;
            ViewBag.City = ngo.City;
            ViewBag.RegistrationNumber = ngo.RegistrationNumber;
            ViewBag.RegistrationDate = ngo.RegistrationDate.ToString("dd MMM yyyy");
            ViewBag.IsVerified = ngo.IsVerified;

            // NGO's donation history
            var donations = await _context.NGOToWelfareTransactions
                .Where(t => t.DonorNgoId == ngoId.Value)
                .OrderByDescending(t => t.DonationDate)
                .ToListAsync();

            ViewBag.TotalDonations = donations.Count;
            ViewBag.TotalMoneyDonated = donations.Where(d => d.DonationType == "Money").Sum(d => d.MonetaryAmount);
            ViewBag.TotalFoodDonated = donations.Where(d => d.DonationType == "Food").Sum(d => d.FoodQuantity ?? 0);
            ViewBag.TotalClothesDonated = donations.Where(d => d.DonationType == "Clothes")
                .Sum(d => (d.MaleClothesQuantity ?? 0) + (d.FemaleClothesQuantity ?? 0) + (d.KidsClothesQuantity ?? 0));
            ViewBag.TotalShelterDonated = donations.Where(d => d.DonationType == "Shelter").Sum(d => d.ShelterBeds ?? 0);

            ViewBag.DonationHistory = donations.Select(d => new
            {
                TransactionId = d.TransactionId,
                Type = d.DonationType,
                Date = d.DonationDate.ToString("dd MMM yyyy"),
                Details = GetDonationDetails(d),
                Status = "Completed"
            }).ToList<object>();

            // Welfare requests from admin
            var requests = await _context.WelfareToNGORequests
                .Include(r => r.Admin)
                .Where(r => r.NgoId == ngoId.Value)
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();

            ViewBag.TotalRequests = requests.Count;
            ViewBag.PendingRequests = requests.Count(r => r.Status == "Pending");
            ViewBag.AcceptedRequests = requests.Count(r => r.Status == "Accepted");
            ViewBag.FulfilledRequests = requests.Count(r => r.Status == "Fulfilled");
            ViewBag.RejectedRequests = requests.Count(r => r.Status == "Rejected");

            ViewBag.WelfareRequests = requests.Select(r => new
            {
                RequestId = r.RequestId,
                AdminName = r.Admin.FullName,
                Type = r.RequestType,
                Description = r.Description,
                RequestDate = r.RequestDate.ToString("dd MMM yyyy"),
                Status = r.Status,
                Details = GetRequestDetails(r),
                CanFulfill = r.Status == "Pending"
            }).ToList<object>();

            return View();
        }

        #region Donation Management

        [HttpPost]
        public async Task<IActionResult> DonateMoney(decimal amount, string description)
        {
            var ngoId = HttpContext.Session.GetInt32("NgoId");
            if (ngoId == null) return RedirectToAction("Index", "UserLogin");

            // ✅ FIX: Round to remove decimals (5000.0 becomes 5000, not 50000)
            amount = Math.Round(amount, 0);

            if (amount <= 0)
            {
                TempData["ErrorType"] = "INVALID_AMOUNT";
                TempData["Error"] = "Donation amount must be greater than zero.";
                return RedirectToAction("Index");
            }

            // ✅ FIX: Validate that amount is a round number (no .01, .99, etc)
            if (amount != Math.Floor(amount))
            {
                TempData["ErrorType"] = "INVALID_AMOUNT";
                TempData["Error"] = "Please enter a whole number amount (e.g., 5000, not 5000.01).";
                return RedirectToAction("Index");
            }

            var fund = await _context.WelfareFunds.FirstOrDefaultAsync();
            if (fund == null)
            {
                TempData["Error"] = "Welfare fund not found.";
                return RedirectToAction("Index");
            }

            fund.CurrentBalance += amount;
            fund.LastUpdated = DateTime.Now;

            var transaction = new NGOToWelfareTransaction
            {
                DonorNgoId = ngoId.Value,
                DonationType = "Money",
                MonetaryAmount = amount,
                DonationDate = DateTime.Now,
                ItemDescription = description ?? "Monetary donation",
                WelfareBalanceAfter = fund.CurrentBalance
            };

            _context.NGOToWelfareTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"✓ Successfully donated PKR {amount:N0} to welfare fund!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DonateFood(int quantity, string unit, string description)
        {
            var ngoId = HttpContext.Session.GetInt32("NgoId");
            if (ngoId == null) return RedirectToAction("Index", "UserLogin");

            if (quantity <= 0)
            {
                TempData["ErrorType"] = "INVALID_QUANTITY";
                TempData["Error"] = "Food quantity must be greater than zero.";
                return RedirectToAction("Index");
            }

            var fund = await _context.WelfareFunds.FirstOrDefaultAsync();
            if (fund == null)
            {
                TempData["Error"] = "Welfare fund not found.";
                return RedirectToAction("Index");
            }

            fund.FoodInventoryUnits += quantity;
            fund.LastUpdated = DateTime.Now;

            var transaction = new NGOToWelfareTransaction
            {
                DonorNgoId = ngoId.Value,
                DonationType = "Food",
                MonetaryAmount = 0,
                FoodQuantity = quantity,
                FoodUnit = unit ?? "units",
                FoodDescription = description ?? "Food donation",
                DonationDate = DateTime.Now,
                WelfareBalanceAfter = fund.CurrentBalance,
                FoodInventoryAfter = fund.FoodInventoryUnits
            };

            _context.NGOToWelfareTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"✓ Successfully donated {quantity} {unit} of food!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DonateClothes(
            int? maleQty,
            int? femaleQty,
            int? kidsQty,
            string clothesType,
            string description)
        {
            var ngoId = HttpContext.Session.GetInt32("NgoId");
            if (ngoId == null) return RedirectToAction("Index", "UserLogin");

            if ((maleQty ?? 0) == 0 && (femaleQty ?? 0) == 0 && (kidsQty ?? 0) == 0)
            {
                TempData["ErrorType"] = "NO_CLOTHES";
                TempData["Error"] = "Please specify at least one type of clothes to donate.";
                return RedirectToAction("Index");
            }

            var fund = await _context.WelfareFunds.FirstOrDefaultAsync();
            if (fund == null)
            {
                TempData["Error"] = "Welfare fund not found.";
                return RedirectToAction("Index");
            }

            fund.MaleClothesInventory += maleQty ?? 0;
            fund.FemaleClothesInventory += femaleQty ?? 0;
            fund.KidsClothesInventory += kidsQty ?? 0;
            fund.LastUpdated = DateTime.Now;

            var transaction = new NGOToWelfareTransaction
            {
                DonorNgoId = ngoId.Value,
                DonationType = "Clothes",
                MonetaryAmount = 0,
                MaleClothesQuantity = maleQty,
                FemaleClothesQuantity = femaleQty,
                KidsClothesQuantity = kidsQty,
                ClothesType = clothesType ?? "Mixed",
                ClothesDescription = description ?? "Clothing donation",
                DonationDate = DateTime.Now,
                WelfareBalanceAfter = fund.CurrentBalance,
                MaleClothesInventoryAfter = fund.MaleClothesInventory,
                FemaleClothesInventoryAfter = fund.FemaleClothesInventory,
                KidsClothesInventoryAfter = fund.KidsClothesInventory
            };

            _context.NGOToWelfareTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            TempData["Success"] = "✓ Successfully donated clothes to welfare fund!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DonateShelter(int beds, string description)
        {
            var ngoId = HttpContext.Session.GetInt32("NgoId");
            if (ngoId == null) return RedirectToAction("Index", "UserLogin");

            if (beds <= 0)
            {
                TempData["ErrorType"] = "INVALID_BEDS";
                TempData["Error"] = "Number of shelter beds must be greater than zero.";
                return RedirectToAction("Index");
            }

            var fund = await _context.WelfareFunds.FirstOrDefaultAsync();
            if (fund == null)
            {
                TempData["Error"] = "Welfare fund not found.";
                return RedirectToAction("Index");
            }

            fund.ShelterCapacity += beds;
            fund.ShelterAvailable += beds;
            fund.LastUpdated = DateTime.Now;

            var transaction = new NGOToWelfareTransaction
            {
                DonorNgoId = ngoId.Value,
                DonationType = "Shelter",
                MonetaryAmount = 0,
                ShelterBeds = beds,
                ShelterDescription = description ?? "Shelter bed donation",
                DonationDate = DateTime.Now,
                WelfareBalanceAfter = fund.CurrentBalance
            };

            _context.NGOToWelfareTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"✓ Successfully donated {beds} shelter beds!";
            return RedirectToAction("Index");
        }

        #endregion

        #region Welfare Request Management

        [HttpPost]
        public async Task<IActionResult> AcceptRequest(int requestId)
        {
            var ngoId = HttpContext.Session.GetInt32("NgoId");
            if (ngoId == null) return RedirectToAction("Index", "UserLogin");

            var request = await _context.WelfareToNGORequests.FindAsync(requestId);
            if (request == null)
            {
                TempData["Error"] = "Request not found.";
                return RedirectToAction("Index");
            }

            if (request.NgoId != ngoId.Value)
            {
                TempData["Error"] = "This request is not for your organization.";
                return RedirectToAction("Index");
            }

            if (request.Status != "Pending")
            {
                TempData["Error"] = $"Request is already {request.Status.ToLower()}.";
                return RedirectToAction("Index");
            }

            request.Status = "Accepted";
            request.ResponseDate = DateTime.Now;
            await _context.SaveChangesAsync();

            TempData["Success"] = "✓ Request accepted! Please fulfill it.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RejectRequest(int requestId, string reason)
        {
            var ngoId = HttpContext.Session.GetInt32("NgoId");
            if (ngoId == null) return RedirectToAction("Index", "UserLogin");

            var request = await _context.WelfareToNGORequests.FindAsync(requestId);
            if (request == null)
            {
                TempData["Error"] = "Request not found.";
                return RedirectToAction("Index");
            }

            if (request.NgoId != ngoId.Value)
            {
                TempData["Error"] = "This request is not for your organization.";
                return RedirectToAction("Index");
            }

            if (request.Status != "Pending")
            {
                TempData["Error"] = $"Request is already {request.Status.ToLower()}.";
                return RedirectToAction("Index");
            }

            request.Status = "Rejected";
            request.ResponseDate = DateTime.Now;
            request.NgoResponse = reason ?? "Request rejected";
            await _context.SaveChangesAsync();

            TempData["Success"] = "Request rejected successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> FulfillRequest(int requestId)
        {
            var ngoId = HttpContext.Session.GetInt32("NgoId");
            if (ngoId == null) return RedirectToAction("Index", "UserLogin");

            var request = await _context.WelfareToNGORequests.FindAsync(requestId);
            if (request == null)
            {
                TempData["Error"] = "Request not found.";
                return RedirectToAction("Index");
            }

            if (request.NgoId != ngoId.Value)
            {
                TempData["Error"] = "This request is not for your organization.";
                return RedirectToAction("Index");
            }

            if (request.Status != "Pending" && request.Status != "Accepted")
            {
                TempData["Error"] = $"Cannot fulfill a request that is {request.Status.ToLower()}.";
                return RedirectToAction("Index");
            }

            var fund = await _context.WelfareFunds.FirstOrDefaultAsync();
            if (fund == null)
            {
                TempData["Error"] = "Welfare fund not initialized.";
                return RedirectToAction("Index");
            }

            // Update welfare fund based on request type
            switch (request.RequestType)
            {
                case "Financial":
                    fund.CurrentBalance += request.RequestedAmount;
                    request.FulfilledAmount = request.RequestedAmount;
                    break;

                case "Food":
                    fund.FoodInventoryUnits += request.FoodQuantity ?? 0;
                    request.FulfilledFoodQuantity = request.FoodQuantity;
                    break;

                case "Clothes":
                    fund.MaleClothesInventory += request.MaleClothesQuantity ?? 0;
                    fund.FemaleClothesInventory += request.FemaleClothesQuantity ?? 0;
                    fund.KidsClothesInventory += request.KidsClothesQuantity ?? 0;
                    request.FulfilledMaleClothes = request.MaleClothesQuantity;
                    request.FulfilledFemaleClothes = request.FemaleClothesQuantity;
                    request.FulfilledKidsClothes = request.KidsClothesQuantity;
                    break;

                case "Shelter":
                    fund.ShelterCapacity += request.ShelterBeds ?? 0;
                    fund.ShelterAvailable += request.ShelterBeds ?? 0;
                    request.FulfilledShelterBeds = request.ShelterBeds;
                    break;
            }

            fund.LastUpdated = DateTime.Now;
            request.Status = "Fulfilled";
            request.ResponseDate = DateTime.Now;
            request.FulfilledDate = DateTime.Now;

            // Create a transaction record
            var transaction = new NGOToWelfareTransaction
            {
                DonorNgoId = ngoId.Value,
                DonationType = request.RequestType,
                MonetaryAmount = request.RequestedAmount,
                FoodQuantity = request.FoodQuantity,
                MaleClothesQuantity = request.MaleClothesQuantity,
                FemaleClothesQuantity = request.FemaleClothesQuantity,
                KidsClothesQuantity = request.KidsClothesQuantity,
                ShelterBeds = request.ShelterBeds,
                DonationDate = DateTime.Now,
                ItemDescription = $"Fulfilling Admin Request #{request.RequestId}: {request.Description}",
                WelfareBalanceAfter = fund.CurrentBalance
            };

            _context.NGOToWelfareTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"✓ {request.RequestType} request fulfilled successfully!";
            return RedirectToAction("Index");
        }

        #endregion

        #region Profile Management

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(
            string organizationName,
            string phone,
            string address,
            string city)
        {
            var ngoId = HttpContext.Session.GetInt32("NgoId");
            if (ngoId == null) return RedirectToAction("Index", "UserLogin");

            var ngo = await _context.NGOsLogins.FindAsync(ngoId.Value);
            if (ngo == null)
            {
                TempData["Error"] = "NGO not found.";
                return RedirectToAction("Index");
            }

            ngo.OrganizationName = organizationName ?? ngo.OrganizationName;
            ngo.Phone = phone ?? ngo.Phone;
            ngo.Address = address ?? ngo.Address;
            ngo.City = city ?? ngo.City;

            await _context.SaveChangesAsync();

            TempData["Success"] = "✓ Profile updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmail(string newEmail)
        {
            var ngoId = HttpContext.Session.GetInt32("NgoId");
            if (ngoId == null) return RedirectToAction("Index", "UserLogin");

            // Check if email already exists
            if (await _context.NGOsLogins.AnyAsync(n => n.Email == newEmail && n.NgoId != ngoId.Value))
            {
                TempData["ErrorType"] = "EMAIL_EXISTS";
                TempData["Error"] = "This email is already in use by another NGO.";
                return RedirectToAction("Index");
            }

            var ngo = await _context.NGOsLogins.FindAsync(ngoId.Value);
            if (ngo == null)
            {
                TempData["Error"] = "NGO not found.";
                return RedirectToAction("Index");
            }

            ngo.Email = newEmail;
            await _context.SaveChangesAsync();

            TempData["Success"] = "✓ Email updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(string currentPassword, string newPassword)
        {
            var ngoId = HttpContext.Session.GetInt32("NgoId");
            if (ngoId == null) return RedirectToAction("Index", "UserLogin");

            var ngo = await _context.NGOsLogins.FindAsync(ngoId.Value);
            if (ngo == null)
            {
                TempData["Error"] = "NGO not found.";
                return RedirectToAction("Index");
            }

            if (ngo.PasswordHash != currentPassword)
            {
                TempData["ErrorType"] = "WRONG_PASSWORD";
                TempData["Error"] = "Current password is incorrect.";
                return RedirectToAction("Index");
            }

            if (newPassword.Length < 6)
            {
                TempData["ErrorType"] = "WEAK_PASSWORD";
                TempData["Error"] = "New password must be at least 6 characters long.";
                return RedirectToAction("Index");
            }

            ngo.PasswordHash = newPassword;
            await _context.SaveChangesAsync();

            TempData["Success"] = "✓ Password updated successfully!";
            return RedirectToAction("Index");
        }

        #endregion

        #region Helper Methods

        private string GetDonationDetails(NGOToWelfareTransaction transaction)
        {
            switch (transaction.DonationType)
            {
                case "Food":
                    return $"{transaction.FoodQuantity ?? 0} {transaction.FoodUnit ?? "units"}";
                case "Clothes":
                    var parts = new List<string>();
                    if ((transaction.MaleClothesQuantity ?? 0) > 0) parts.Add($"M:{transaction.MaleClothesQuantity}");
                    if ((transaction.FemaleClothesQuantity ?? 0) > 0) parts.Add($"F:{transaction.FemaleClothesQuantity}");
                    if ((transaction.KidsClothesQuantity ?? 0) > 0) parts.Add($"K:{transaction.KidsClothesQuantity}");
                    return string.Join(", ", parts);
                case "Money":
                    return $"PKR {transaction.MonetaryAmount:N0}";
                case "Shelter":
                    return $"{transaction.ShelterBeds ?? 0} beds";
                default:
                    return transaction.ItemDescription ?? "N/A";
            }
        }

        private string GetRequestDetails(WelfareToNGORequest request)
        {
            switch (request.RequestType)
            {
                case "Financial":
                    return $"PKR {request.RequestedAmount:N0}";
                case "Food":
                    return $"{request.FoodQuantity} {request.FoodUnit ?? "units"}";
                case "Clothes":
                    var parts = new List<string>();
                    if ((request.MaleClothesQuantity ?? 0) > 0) parts.Add($"M:{request.MaleClothesQuantity}");
                    if ((request.FemaleClothesQuantity ?? 0) > 0) parts.Add($"F:{request.FemaleClothesQuantity}");
                    if ((request.KidsClothesQuantity ?? 0) > 0) parts.Add($"K:{request.KidsClothesQuantity}");
                    return string.Join(", ", parts);
                case "Shelter":
                    return $"{request.ShelterBeds} beds";
                default:
                    return request.Description;
            }
        }

        #endregion
    }
}