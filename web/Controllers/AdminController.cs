using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

namespace web.Controllers
{
    public class AdminController : Controller
    {
        private readonly WelfareDb _context;

        public AdminController(WelfareDb context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                TempData["Error"] = "Please login first.";
                return RedirectToAction("Index", "UserLogin");
            }

            var admin = await _context.AdminLogins.FindAsync(adminId.Value);
            if (admin == null)
            {
                HttpContext.Session.Clear();
                TempData["Error"] = "Access denied.";
                return RedirectToAction("Index", "UserLogin");
            }

            ViewBag.AdminId = admin.AdminId;
            ViewBag.AdminName = admin.FullName;

            // Initialize or get welfare fund
            var fund = await _context.WelfareFunds.FirstOrDefaultAsync();
            if (fund == null)
            {
                fund = new WelfareFund
                {
                    CurrentBalance = 1000000,
                    MonthlyAllocation = 1000000,
                    FoodInventoryUnits = 10000,
                    MonthlyFoodAllocation = 5000,
                    MaleClothesInventory = 1000,
                    FemaleClothesInventory = 1000,
                    KidsClothesInventory = 1000,
                    MonthlyMaleClothesAllocation = 500,
                    MonthlyFemaleClothesAllocation = 500,
                    MonthlyKidsClothesAllocation = 500,
                    ShelterCapacity = 100,
                    ShelterOccupied = 0,
                    ShelterAvailable = 100,
                    LastUpdated = DateTime.Now,
                    LastMonthlyReset = DateTime.Now
                };
                _context.WelfareFunds.Add(fund);
                await _context.SaveChangesAsync();
            }

            ViewBag.CurrentBalance = fund.CurrentBalance;
            ViewBag.FoodInventory = fund.FoodInventoryUnits;
            ViewBag.MaleClothes = fund.MaleClothesInventory;
            ViewBag.FemaleClothes = fund.FemaleClothesInventory;
            ViewBag.KidsClothes = fund.KidsClothesInventory;
            ViewBag.ShelterAvailable = fund.ShelterAvailable;

            // Pending Requests (include both Pending and Approved for fulfillment)
            var pendingRequests = await _context.ReceiverRequests
                .Include(r => r.Receiver)
                .Where(r => r.Status == "Pending" || r.Status == "Approved")
                .OrderBy(r => r.RequestDate)
                .ToListAsync();

            ViewBag.PendingRequests = pendingRequests.Select(r => new
            {
                RequestId = r.RequestId,
                ReceiverName = r.Receiver.FullName,
                ReceiverId = r.ReceiverId,
                Type = r.RequestType,
                Date = r.RequestDate.ToString("dd MMM yyyy"),
                Details = GetRequestDetails(r),
                CanFulfill = CanFulfillRequest(r, fund),
                Status = r.Status
            }).ToList<object>();

            // Statistics
            var allRequests = await _context.ReceiverRequests.ToListAsync();
            ViewBag.TotalRequests = allRequests.Count;
            ViewBag.PendingCount = allRequests.Count(r => r.Status == "Pending");
            ViewBag.ApprovedCount = allRequests.Count(r => r.Status == "Approved");
            ViewBag.FulfilledCount = allRequests.Count(r => r.Status == "Fulfilled");
            ViewBag.RejectedCount = allRequests.Count(r => r.Status == "Rejected");

            // All Users
            var users = await _context.UserLoginConfidentials.OrderBy(u => u.FullName).ToListAsync();
            ViewBag.AllUsers = users.Select(u => new
            {
                UserId = u.UserId,
                FullName = u.FullName,
                UserType = u.UserType,
                Email = u.Email,
                Phone = u.Phone,
                City = u.City,
                IsActive = u.IsActive,
                RegistrationDate = u.RegistrationDate.ToString("dd MMM yyyy")
            }).ToList<object>();

            // All NGOs
            var ngos = await _context.NGOsLogins.OrderBy(n => n.OrganizationName).ToListAsync();
            ViewBag.AllNGOs = ngos.Select(n => new
            {
                NgoId = n.NgoId,
                OrganizationName = n.OrganizationName,
                Email = n.Email,
                Phone = n.Phone,
                City = n.City,
                IsActive = n.IsActive,
                IsVerified = n.IsVerified,
                RegistrationDate = n.RegistrationDate.ToString("dd MMM yyyy")
            }).ToList<object>();

            // Verified and Active NGOs for NGO Request dropdown
            var verifiedNGOs = await _context.NGOsLogins
                .Where(n => n.IsVerified && n.IsActive)
                .OrderBy(n => n.OrganizationName)
                .ToListAsync();

            ViewBag.VerifiedNGOs = verifiedNGOs.Select(n => new
            {
                NgoId = n.NgoId,
                OrganizationName = n.OrganizationName,
                City = n.City
            }).ToList<object>();

            // All Admins
            var admins = await _context.AdminLogins.OrderBy(a => a.FullName).ToListAsync();
            ViewBag.AllAdmins = admins.Select(a => new
            {
                AdminId = a.AdminId,
                FullName = a.FullName,
                DateOfBirth = a.Dob.ToString("dd MMM yyyy")
            }).ToList<object>();

            // Recent Transactions
            var recentTransactions = await _context.WelfareToReceiverTransactions
                .Include(t => t.Receiver)
                .OrderByDescending(t => t.TransactionDate)
                .Take(10)
                .ToListAsync();

            ViewBag.RecentTransactions = recentTransactions.Select(t => new
            {
                TransactionId = t.TransactionId,
                ReceiverName = t.Receiver.FullName,
                Type = t.TransactionType,
                Date = t.TransactionDate.ToString("dd MMM yyyy"),
                Details = GetTransactionDetails(t)
            }).ToList<object>();

            // Donations - Donor Donations
            var donorDonations = await _context.DonorToWelfareTransactions
                .Include(d => d.Donor)
                .OrderByDescending(d => d.DonationDate)
                .ToListAsync();

            ViewBag.DonorDonations = donorDonations.Select(d => new
            {
                TransactionId = d.TransactionId,
                DonorName = d.Donor.FullName,
                DonorId = d.DonorUserId,
                Type = d.DonationType,
                Date = d.DonationDate.ToString("dd MMM yyyy"),
                Details = GetDonorTransactionDetails(d)
            }).ToList<object>();

            // Donations - NGO Donations
            var ngoDonations = await _context.NGOToWelfareTransactions
                .Include(n => n.DonorNgo)
                .OrderByDescending(n => n.DonationDate)
                .ToListAsync();

            ViewBag.NGODonations = ngoDonations.Select(n => new
            {
                TransactionId = n.TransactionId,
                NGOName = n.DonorNgo.OrganizationName,
                NGOId = n.DonorNgoId,
                Type = n.DonationType,
                Date = n.DonationDate.ToString("dd MMM yyyy"),
                Details = GetNGOTransactionDetails(n)
            }).ToList<object>();

            ViewBag.TotalDonorDonations = donorDonations.Count;
            ViewBag.TotalNGODonations = ngoDonations.Count;
            ViewBag.TotalDonations = donorDonations.Count + ngoDonations.Count;

            // NGO Requests - Requests sent to NGOs for support
            var ngoRequests = await _context.WelfareToNGORequests
                .Include(r => r.Ngo)
                .Include(r => r.Admin)
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();

            ViewBag.NGORequests = ngoRequests.Select(r => new
            {
                RequestId = r.RequestId,
                NgoId = r.NgoId,
                NgoName = r.Ngo.OrganizationName,
                RequestType = r.RequestType,
                Description = r.Description,
                RequestedAmount = r.RequestedAmount,
                FoodQuantity = r.FoodQuantity,
                MaleClothesQuantity = r.MaleClothesQuantity,
                FemaleClothesQuantity = r.FemaleClothesQuantity,
                KidsClothesQuantity = r.KidsClothesQuantity,
                ShelterBeds = r.ShelterBeds,
                Status = r.Status,
                RequestDate = r.RequestDate,
                ResponseDate = r.ResponseDate
            }).ToList<object>();

            return View();
        }

        #region Receiver Request Management

        [HttpPost]
        public async Task<IActionResult> ApproveRequest(int requestId)
        {
            Console.WriteLine($"🔵 ApproveRequest called with ID: {requestId}");

            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                Console.WriteLine("❌ No admin session found");
                return RedirectToAction("Index", "UserLogin");
            }

            var request = await _context.ReceiverRequests.FindAsync(requestId);
            if (request == null)
            {
                Console.WriteLine($"❌ Request {requestId} not found");
                TempData["Error"] = "Request not found.";
                return RedirectToAction("Index");
            }

            Console.WriteLine($"📝 Request found: ID={request.RequestId}, Status={request.Status}");

            if (request.Status != "Pending")
            {
                Console.WriteLine($"❌ Request already {request.Status}");
                TempData["Error"] = $"Request is already {request.Status.ToLower()}.";
                return RedirectToAction("Index");
            }

            request.Status = "Approved";
            request.ApprovedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            Console.WriteLine($"✅ Request {requestId} approved successfully");
            TempData["Success"] = "✓ Request approved successfully! You can now fulfill it.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RejectRequest(int requestId, string reason)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null) return RedirectToAction("Index", "UserLogin");

            var request = await _context.ReceiverRequests.FindAsync(requestId);
            if (request == null)
            {
                TempData["Error"] = "Request not found.";
                return RedirectToAction("Index");
            }

            if (request.Status != "Pending" && request.Status != "Approved")
            {
                TempData["Error"] = $"Request is already {request.Status.ToLower()}.";
                return RedirectToAction("Index");
            }

            request.Status = "Rejected";
            request.Description += $" | Rejected: {reason}";
            await _context.SaveChangesAsync();

            TempData["Success"] = "Request rejected successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> FulfillRequest(int requestId)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null) return RedirectToAction("Index", "UserLogin");

            var request = await _context.ReceiverRequests
                .Include(r => r.Receiver)
                .FirstOrDefaultAsync(r => r.RequestId == requestId);

            if (request == null)
            {
                TempData["Error"] = "Request not found.";
                return RedirectToAction("Index");
            }

            if (request.Status != "Pending" && request.Status != "Approved")
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

            if (!CanFulfillRequest(request, fund))
            {
                TempData["Error"] = "⚠️ Insufficient resources to fulfill this request.";
                return RedirectToAction("Index");
            }

            var transaction = new WelfareToReceiverTransaction
            {
                RequestId = requestId,
                ReceiverId = request.ReceiverId,
                ApprovedByAdminId = adminId.Value,
                TransactionType = request.RequestType,
                TransactionDate = DateTime.Now,
                Status = "Fulfilled",
                Description = request.Description
            };

            switch (request.RequestType)
            {
                case "Food":
                    fund.FoodInventoryUnits -= request.FoodQuantity ?? 0;
                    transaction.FoodUnitsProvided = request.FoodQuantity;
                    break;

                case "Clothes":
                    fund.MaleClothesInventory -= request.MaleClothesQuantity ?? 0;
                    fund.FemaleClothesInventory -= request.FemaleClothesQuantity ?? 0;
                    fund.KidsClothesInventory -= request.KidsClothesQuantity ?? 0;
                    transaction.MaleClothesProvided = request.MaleClothesQuantity;
                    transaction.FemaleClothesProvided = request.FemaleClothesQuantity;
                    transaction.KidsClothesProvided = request.KidsClothesQuantity;
                    break;

                case "Loan":
                    fund.CurrentBalance -= request.RequestedAmount;
                    transaction.MonetaryAmount = request.RequestedAmount;
                    transaction.LoanPurpose = request.LoanPurpose;
                    transaction.RepaymentMonths = request.RepaymentMonths;
                    break;

                case "Shelter":
                    fund.ShelterOccupied += request.RequiredRooms ?? 0;
                    fund.ShelterAvailable = fund.ShelterCapacity - fund.ShelterOccupied;
                    transaction.RoomsAllocated = request.RequiredRooms;
                    transaction.ShelterStartDate = DateTime.Now;
                    transaction.ShelterEndDate = DateTime.Now.AddDays(request.ShelterDurationDays ?? 0);
                    break;
            }

            transaction.WelfareBalanceAfter = fund.CurrentBalance;
            request.Status = "Fulfilled";
            request.FulfilledDate = DateTime.Now;

            if (request.ApprovedDate == null)
            {
                request.ApprovedDate = DateTime.Now;
            }

            fund.LastUpdated = DateTime.Now;

            _context.WelfareToReceiverTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"✓ {request.RequestType} request fulfilled successfully!";
            TempData["JustFulfilled"] = request.RequestType.ToLower();
            return RedirectToAction("Index");
        }

        #endregion

        #region User Management

        [HttpPost]
        public async Task<IActionResult> ToggleUserStatus(int userId)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null) return RedirectToAction("Index", "UserLogin");

            var user = await _context.UserLoginConfidentials.FindAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("Index");
            }

            user.IsActive = !user.IsActive;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"User {(user.IsActive ? "activated" : "deactivated")} successfully.";
            return RedirectToAction("Index");
        }

        #endregion

        #region NGO Management

        [HttpPost]
        public async Task<IActionResult> AddNGO(int NgoId, string OrganizationName, string Email, string Password, string Phone, string Address, string City, string RegistrationNumber)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null) return RedirectToAction("Index", "UserLogin");

            if (NgoId < 100000 || NgoId > 999999)
            {
                TempData["Error"] = "NGO ID must be a 6-digit number (100000-999999).";
                return RedirectToAction("Index");
            }

            // Check for duplicate NGO ID - SPECIFIC ERROR
            if (await _context.NGOsLogins.AnyAsync(n => n.NgoId == NgoId))
            {
                TempData["ErrorType"] = "NGO_ID_EXISTS";
                TempData["Error"] = $"An NGO with ID #{NgoId} already exists. Please use a different ID.";
                return RedirectToAction("Index");
            }

            // Check for duplicate Email - SPECIFIC ERROR
            if (await _context.NGOsLogins.AnyAsync(n => n.Email == Email))
            {
                TempData["ErrorType"] = "NGO_EMAIL_EXISTS";
                TempData["Error"] = $"An NGO with email '{Email}' already exists. Please use a different email.";
                return RedirectToAction("Index");
            }

            var ngo = new NGOsLogin
            {
                NgoId = NgoId,
                OrganizationName = OrganizationName,
                Email = Email,
                PasswordHash = Password,
                Phone = Phone,
                Address = Address,
                City = City,
                RegistrationNumber = RegistrationNumber,
                RegistrationDate = DateTime.Now,
                IsActive = true,
                IsVerified = true
            };

            _context.NGOsLogins.Add(ngo);
            await _context.SaveChangesAsync();

            TempData["Success"] = "✓ NGO added and verified successfully!";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> ToggleNGOStatus(int ngoId)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null) return RedirectToAction("Index", "UserLogin");

            var ngo = await _context.NGOsLogins.FindAsync(ngoId);
            if (ngo == null)
            {
                TempData["Error"] = "NGO not found.";
                return RedirectToAction("Index");
            }

            ngo.IsActive = !ngo.IsActive;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"NGO {(ngo.IsActive ? "activated" : "deactivated")} successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> VerifyNGO(int ngoId)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null) return RedirectToAction("Index", "UserLogin");

            var ngo = await _context.NGOsLogins.FindAsync(ngoId);
            if (ngo == null)
            {
                TempData["Error"] = "NGO not found.";
                return RedirectToAction("Index");
            }

            ngo.IsVerified = !ngo.IsVerified;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"NGO {(ngo.IsVerified ? "verified" : "unverified")} successfully.";
            return RedirectToAction("Index");
        }

        #endregion

        #region Admin Management

        [HttpPost]
        public async Task<IActionResult> AddAdmin(int AdminId, string FullName, string Password, DateTime Dob)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null) return RedirectToAction("Index", "UserLogin");

            if (await _context.AdminLogins.AnyAsync(a => a.AdminId == AdminId))
            {
                TempData["Error"] = "Admin ID already exists. Please use a different ID.";
                return RedirectToAction("Index");
            }

            var age = DateTime.Now.Year - Dob.Year;
            if (DateTime.Now < Dob.AddYears(age)) age--;

            if (age < 18)
            {
                TempData["Error"] = "Admin must be at least 18 years old.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
            {
                TempData["Error"] = "Password must be at least 6 characters long.";
                return RedirectToAction("Index");
            }

            var newAdmin = new AdminLogin
            {
                AdminId = AdminId,
                FullName = FullName,
                passwordHash = Password,
                Dob = Dob
            };

            _context.AdminLogins.Add(newAdmin);
            await _context.SaveChangesAsync();

            TempData["Success"] = "✓ Admin added successfully!";
            return RedirectToAction("Index");
        }

        #endregion

        #region NGO Request Management

        [HttpPost]
        public async Task<IActionResult> CreateNGORequest(
            int NgoId,
            string RequestType,
            string Description,
            decimal? RequestedAmount,
            int? FoodQuantity,
            int? MaleClothesQuantity,
            int? FemaleClothesQuantity,
            int? KidsClothesQuantity,
            int? ShelterBeds)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null) return RedirectToAction("Index", "UserLogin");

            // Validate NGO exists and is verified
            var ngo = await _context.NGOsLogins.FindAsync(NgoId);
            if (ngo == null || !ngo.IsVerified || !ngo.IsActive)
            {
                TempData["Error"] = "Selected NGO is not available or verified.";
                return RedirectToAction("Index");
            }

            // Validate based on request type
            if (RequestType == "Clothes")
            {
                if ((MaleClothesQuantity ?? 0) == 0 &&
                    (FemaleClothesQuantity ?? 0) == 0 &&
                    (KidsClothesQuantity ?? 0) == 0)
                {
                    TempData["Error"] = "Please specify at least one type of clothes quantity.";
                    return RedirectToAction("Index");
                }
            }

            var ngoRequest = new WelfareToNGORequest
            {
                AdminId = adminId.Value,
                NgoId = NgoId,
                RequestType = RequestType,
                Description = Description,
                RequestedAmount = RequestedAmount ?? 0,
                FoodQuantity = FoodQuantity,
                MaleClothesQuantity = MaleClothesQuantity,
                FemaleClothesQuantity = FemaleClothesQuantity,
                KidsClothesQuantity = KidsClothesQuantity,
                ShelterBeds = ShelterBeds,
                Status = "Pending",
                RequestDate = DateTime.Now
            };

            _context.WelfareToNGORequests.Add(ngoRequest);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"✓ {RequestType} request sent to {ngo.OrganizationName} successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CancelNGORequest(int requestId)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null) return RedirectToAction("Index", "UserLogin");

            var request = await _context.WelfareToNGORequests.FindAsync(requestId);
            if (request == null)
            {
                TempData["Error"] = "Request not found.";
                return RedirectToAction("Index");
            }

            // Only allow cancellation of pending requests
            if (request.Status != "Pending")
            {
                TempData["Error"] = "Only pending requests can be cancelled.";
                return RedirectToAction("Index");
            }

            // Verify this admin owns the request
            if (request.AdminId != adminId.Value)
            {
                TempData["Error"] = "You can only cancel your own requests.";
                return RedirectToAction("Index");
            }

            request.Status = "Cancelled";
            request.ResponseDate = DateTime.Now;
            await _context.SaveChangesAsync();

            TempData["Success"] = "NGO request cancelled successfully.";
            return RedirectToAction("Index");
        }

        #endregion

        #region Helper Methods

        private bool CanFulfillRequest(ReceiverRequest request, WelfareFund fund)
        {
            switch (request.RequestType)
            {
                case "Food":
                    return fund.FoodInventoryUnits >= (request.FoodQuantity ?? 0);
                case "Clothes":
                    return fund.MaleClothesInventory >= (request.MaleClothesQuantity ?? 0) &&
                           fund.FemaleClothesInventory >= (request.FemaleClothesQuantity ?? 0) &&
                           fund.KidsClothesInventory >= (request.KidsClothesQuantity ?? 0);
                case "Loan":
                    return fund.CurrentBalance >= request.RequestedAmount;
                case "Shelter":
                    return fund.ShelterAvailable >= (request.RequiredRooms ?? 0);
                default:
                    return false;
            }
        }

        private string GetRequestDetails(ReceiverRequest request)
        {
            switch (request.RequestType)
            {
                case "Food":
                    return $"{request.FamilyMembers} members, {request.FoodQuantity} units";
                case "Clothes":
                    var parts = new List<string>();
                    if (request.MaleClothesQuantity > 0) parts.Add($"M:{request.MaleClothesQuantity}");
                    if (request.FemaleClothesQuantity > 0) parts.Add($"F:{request.FemaleClothesQuantity}");
                    if (request.KidsClothesQuantity > 0) parts.Add($"K:{request.KidsClothesQuantity}");
                    return string.Join(", ", parts) + $" ({request.ClothesType})";
                case "Loan":
                    return $"PKR {request.RequestedAmount:N0} - {request.LoanPurpose}";
                case "Shelter":
                    return $"{request.ShelterDurationDays} days, {request.RequiredRooms} rooms";
                default:
                    return request.Description;
            }
        }

        private string GetTransactionDetails(WelfareToReceiverTransaction transaction)
        {
            switch (transaction.TransactionType)
            {
                case "Food":
                    return $"{transaction.FoodUnitsProvided} units";
                case "Clothes":
                    var parts = new List<string>();
                    if (transaction.MaleClothesProvided > 0) parts.Add($"M:{transaction.MaleClothesProvided}");
                    if (transaction.FemaleClothesProvided > 0) parts.Add($"F:{transaction.FemaleClothesProvided}");
                    if (transaction.KidsClothesProvided > 0) parts.Add($"K:{transaction.KidsClothesProvided}");
                    return string.Join(", ", parts);
                case "Loan":
                    return $"PKR {transaction.MonetaryAmount:N0}";
                case "Shelter":
                    return $"{transaction.RoomsAllocated} rooms";
                default:
                    return transaction.Description ?? "";
            }
        }

        private string GetDonorTransactionDetails(DonorToWelfareTransaction transaction)
        {
            switch (transaction.DonationType)
            {
                case "Food":
                    return $"{transaction.FoodQuantity ?? 0} units" +
                           (!string.IsNullOrEmpty(transaction.FoodDescription) ? $" - {transaction.FoodDescription}" : "");
                case "Clothes":
                    var parts = new List<string>();
                    if ((transaction.MaleClothesQuantity ?? 0) > 0) parts.Add($"M:{transaction.MaleClothesQuantity}");
                    if ((transaction.FemaleClothesQuantity ?? 0) > 0) parts.Add($"F:{transaction.FemaleClothesQuantity}");
                    if ((transaction.KidsClothesQuantity ?? 0) > 0) parts.Add($"K:{transaction.KidsClothesQuantity}");
                    var clothesDetail = string.Join(", ", parts);
                    if (!string.IsNullOrEmpty(transaction.ClothesType))
                        clothesDetail += $" ({transaction.ClothesType})";
                    return clothesDetail;
                case "Money":
                    return $"PKR {transaction.MonetaryAmount:N0}";
                case "Shelter":
                    return $"{transaction.ShelterBeds ?? 0} beds" +
                           (!string.IsNullOrEmpty(transaction.ShelterDescription) ? $" - {transaction.ShelterDescription}" : "");
                default:
                    return transaction.ItemDescription ?? "N/A";
            }
        }

        private string GetNGOTransactionDetails(NGOToWelfareTransaction transaction)
        {
            switch (transaction.DonationType)
            {
                case "Food":
                    return $"{transaction.FoodQuantity ?? 0} {transaction.FoodUnit ?? "units"}" +
                           (!string.IsNullOrEmpty(transaction.FoodDescription) ? $" - {transaction.FoodDescription}" : "");
                case "Clothes":
                    var parts = new List<string>();
                    if ((transaction.MaleClothesQuantity ?? 0) > 0) parts.Add($"M:{transaction.MaleClothesQuantity}");
                    if ((transaction.FemaleClothesQuantity ?? 0) > 0) parts.Add($"F:{transaction.FemaleClothesQuantity}");
                    if ((transaction.KidsClothesQuantity ?? 0) > 0) parts.Add($"K:{transaction.KidsClothesQuantity}");
                    var clothesDetail = string.Join(", ", parts);
                    if (!string.IsNullOrEmpty(transaction.ClothesType))
                        clothesDetail += $" ({transaction.ClothesType})";
                    return clothesDetail;
                case "Money":
                    return $"PKR {transaction.MonetaryAmount:N0}";
                case "Shelter":
                    return $"{transaction.ShelterBeds ?? 0} beds" +
                           (!string.IsNullOrEmpty(transaction.ShelterDescription) ? $" - {transaction.ShelterDescription}" : "");
                default:
                    return transaction.ItemDescription ?? "N/A";
            }
        }

        #endregion
    }
}