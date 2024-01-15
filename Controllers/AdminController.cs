using Health_Insurance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Bcpg.OpenPgp;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace Health_Insurance.Controllers
{
    //edit
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly EmailService _emailService;

        public AdminController(ModelContext context, IWebHostEnvironment webHostEnvironment, EmailService emailService, PdfService pdfService)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
            _emailService = emailService;
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        private byte[] CreatePdfWithMessage(string message)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Create a PDF document
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                document.Open();
                document.Add(new Paragraph(message));
                document.Close();

                return ms.ToArray();
            }
        }

        public IActionResult Beneficiaries(decimal? id)
        {
            if (HttpContext.Session.GetInt32("UserId") != null && HttpContext.Session.GetInt32("RoleId") == 1)
            {
                ViewBag.CountBeneficiaries=_context.Beneficiaries.Where(x=>x.SubscriptionsId==id).Count();

                ViewBag.CountRegistrer = _context.Useraccounts.Count();
                ViewBag.SubscriptionAmount = _context.Subscriptions.Sum(x => x.SubscriptionAmount);
                ViewBag.CountSubscriptions = _context.Subscriptions.Count();
                ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
                ViewBag.Address = HttpContext.Session.GetString("Address");
                ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");
                ViewBag.Email = HttpContext.Session.GetString("Email");
                ViewBag.Passwd = HttpContext.Session.GetString("Passwd");
                ViewBag.PhoneNumber = HttpContext.Session.GetString("PhoneNumber");
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.Layout = HttpContext.Session.GetString("Layout");
                ViewBag.subscriptionID = HttpContext.Session.GetInt32("subscriptionID");
                ViewBag.CountBeneficiariesAccepted = _context.Beneficiaries.Where(b => b.BeneficiaryState == "Accept"&& b.SubscriptionsId == id).Count();
                ViewBag.CountBeneficiarieswaiting = _context.Beneficiaries.Where(b => b.BeneficiaryState == "waiting" && b.SubscriptionsId == id).Count();

                var beneficiaries = _context.Beneficiaries.Where(x => x.SubscriptionsId == id).Include(b => b.Subscriptions).ThenInclude(u=>u.UserAccount).ToList();

                return View(beneficiaries);
            }

            else
            {
                return RedirectToAction("Login", "Authentication");
            }



        }

        //public IActionResult Beneficiaries(decimal? id)
        //{
        //    var beneficiaries = _context.Beneficiaries.Where(x=>x.SubscriptionsId==id).ToList();
        //    ViewBag.subscriptionID = HttpContext.Session.GetInt32("subscriptionID");
        //    return View(beneficiaries);
        //}


        private bool SubscriptionExists(decimal id)
        {
            return (_context.Subscriptions?.Any(e => e.Id == id)).GetValueOrDefault();
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Reject(decimal id, [Bind("Id,UserAccountId,SubscriptionDate,SubscriptionsStatus,SubscriptionDuration,SubscriptionAmount")] Subscription subscription)
        //{
        //    if (id != subscription.Id)
        //    {
        //        return NotFound();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            subscription.SubscriptionsStatus = "Reject";
        //            _context.Update(subscription);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!SubscriptionExists(subscription.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["UserAccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", subscription.UserAccountId);
        //    return View(subscription);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Accept(decimal id, [Bind("Id,UserAccountId,SubscriptionDate,SubscriptionsStatus,SubscriptionDuration,SubscriptionAmount")] Subscription subscription)
        //{
        //    if (id != subscription.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            subscription.SubscriptionsStatus = "Accept";
        //            _context.Update(subscription);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!SubscriptionExists(subscription.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["UserAccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", subscription.UserAccountId);
        //    return View(subscription);
        //}



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(decimal id, [Bind("Id,BeneficiaryName,BeneficiaryRelation,BeneficiaryState,BeneficiaryImageProof,SubscriptionsId,DateBeneficiary,ImageFile")] Beneficiary beneficiary)
        {
            if (id != beneficiary.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    beneficiary.BeneficiaryState = "Reject";
                    _context.Update(beneficiary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscriptionExists(beneficiary.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
               
                return RedirectToAction("Beneficiaries", "Admin", new { id = beneficiary.SubscriptionsId });
            }

            ViewData["UserAccountId"] = new SelectList(_context.Subscriptions, "Id", "Id", beneficiary.SubscriptionsId);

            return View(beneficiary.Id);
        }



        //[HttpPost]  **reject**
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Reject(decimal id, [Bind("Id,BeneficiaryName,BeneficiaryRelation,BeneficiaryState,BeneficiaryImageProof,SubscriptionsId,DateBeneficiary,ImageFile")] Beneficiary beneficiary)
        //{
        //    if (id != beneficiary.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            beneficiary.BeneficiaryState = "Reject";
        //            _context.Update(beneficiary);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!SubscriptionExists(beneficiary.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction("Beneficiaries", "Admin");
        //    }
        //    ViewData["UserAccountId"] = new SelectList(_context.Subscriptions, "Id", "Id", beneficiary.SubscriptionsId);

        //    return View(beneficiary.Id);
        //}

        //rerurn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(decimal id, [Bind("Id,BeneficiaryName,BeneficiaryRelation,BeneficiaryState,BeneficiaryImageProof,SubscriptionsId,DateBeneficiary,ImageFile")] Beneficiary beneficiary)
        {
            if (id != beneficiary.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (beneficiary.BeneficiaryState != "Accept")
                    {
                        beneficiary.BeneficiaryState = "Accept";
                        _context.Update(beneficiary);
                        await _context.SaveChangesAsync();

                        var sub = _context.Subscriptions.Where(u => u.Id == beneficiary.SubscriptionsId).Include(x => x.UserAccount).FirstOrDefault();
                        var user = _context.Useraccounts.Where(x => x.Id == sub.UserAccountId).FirstOrDefault();
                        string message = "Hello, I would like to inform you that  " + beneficiary.BeneficiaryName + " has been accepted into your health insurance and that he can benefit from its benefits";
                        var pdfBytes = CreatePdfWithMessage(message);
                        _emailService.SendEmailWithAttachment(user.Email, "Test Subject", message, pdfBytes);

                    }
                    else
                    {
                        ViewBag.ErorrAccepted = "this  is already  Accepted";
                        return RedirectToAction("Beneficiaries", "Admin", new { id = beneficiary.SubscriptionsId });

                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscriptionExists(beneficiary.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Beneficiaries", "Admin", new { id = beneficiary.SubscriptionsId });
            }
            ViewData["UserAccountId"] = new SelectList(_context.Subscriptions, "Id", "Id", beneficiary.SubscriptionsId);
            return View(beneficiary.Id);
        }

        public IActionResult EditProfile()
        {
            var user = _context.Useraccounts.FirstOrDefault();
            return View(user);
        }


        public IActionResult Index()
        {

            if (HttpContext.Session.GetInt32("UserId") != null && HttpContext.Session.GetInt32("RoleId") == 1)
            {
                ViewBag.CountRegistrer = _context.Useraccounts.Count();
                ViewBag.CountBeneficiaries=_context.Beneficiaries.Count();
                ViewBag.SubscriptionAmount = _context.Subscriptions.Sum(x => x.SubscriptionAmount);
                ViewBag.CountSubscriptions = _context.Subscriptions.Count();
                ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
                ViewBag.Address = HttpContext.Session.GetString("Address");
                ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");
                ViewBag.Email = HttpContext.Session.GetString("Email");
                ViewBag.Passwd = HttpContext.Session.GetString("Passwd");
                ViewBag.PhoneNumber = HttpContext.Session.GetString("PhoneNumber");
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.Layout = HttpContext.Session.GetString("Layout");
                 int userId= ViewBag.UserId;
                //var sub = _context.Subscriptions.Where(s => s.UserAccountId == userId).FirstOrDefault();
                //var subscriptionss = _context.Subscriptions.ToList().FirstOrDefault();
  
                //var beneficiariess = _context.Beneficiaries.Where(x => x.SubscriptionsId == sub.Id).ToList().FirstOrDefault()
                var userAccount = _context.Useraccounts.ToList();
                var subscriptions = _context.Subscriptions.Include(s=>s.UserAccount).ToList();
                var beneficiaries = _context.Beneficiaries.Include(b=>b.Subscriptions).ToList();

                var model =
                           from s in subscriptions
                           join u in userAccount on s.UserAccountId equals u.Id
                           orderby s.SubscriptionDate
                           select new JoinSubscription { subscription = s, userAccounts = u };
                return View(model);



            }
            else
            {
                return RedirectToAction("Login", "Authentication");
            }

        }

        public IActionResult Search()
        {

            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.Address = HttpContext.Session.GetString("Address");
            ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Passwd = HttpContext.Session.GetString("Passwd");
            ViewBag.PhoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Layout = HttpContext.Session.GetString("Layout");
            var model = _context.Subscriptions.Include(s => s.UserAccount).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Search(DateTime? startDate, DateTime? endDate)
        {


            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.Address = HttpContext.Session.GetString("Address");
            ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");

            var model = _context.Subscriptions.OrderBy(d=>d.SubscriptionDate).Include(s => s.UserAccount).ToList();

            if (startDate == null && endDate == null)
            {
                return View(model);
            }
            else if (startDate != null&& endDate == null  )
            {
                var result = model.Where(x => x.SubscriptionDate.Value.Date >= startDate).ToList();
                return View(result);
            }
            else if (startDate == null && endDate != null)
            {
                var result = model.Where(x => x.SubscriptionDate <= endDate).ToList();
                return View(result);
            }
            else
            {
                var result = model.Where(x => x.SubscriptionDate >= startDate && x.SubscriptionDate <= endDate).ToList();
                return View(result);
            }

        }

        public IActionResult Report()
        {

            ViewBag.CountSubscriptionsPaid = _context.Subscriptions.Where(s => s.SubscriptionsStatus == "paid").Count();
            ViewBag.CountSubscriptionsNotPaid = _context.Subscriptions.Where(s => s.SubscriptionsStatus == "not Paid").Count();
            ViewBag.SubscriptionAmount = _context.Subscriptions.Sum(x => x.SubscriptionAmount);
            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.Address = HttpContext.Session.GetString("Address");
            ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Passwd = HttpContext.Session.GetString("Passwd");
            ViewBag.PhoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.subscriptionID = HttpContext.Session.GetInt32("subscriptionID");

            var userAccount = _context.Useraccounts.ToList();
            var subscriptions = _context.Subscriptions.ToList();
            var beneficiaries = _context.Beneficiaries.ToList();

            var model =
                       from s in subscriptions
                       join u in userAccount on s.UserAccountId equals u.Id
                       orderby s.SubscriptionDate
                       select new JoinSubscription { subscription = s, userAccounts = u };
            var result = Tuple.Create<IEnumerable<Beneficiary>, IEnumerable<Subscription>, IEnumerable<Useraccount>, IEnumerable<JoinSubscription>>(beneficiaries, subscriptions, userAccount, model);

            return View(result);


        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Report(DateTime? startDate, DateTime? endDate)
        {
            var userAccount = _context.Useraccounts.ToList();
            var subscriptions = _context.Subscriptions.ToList();
            var beneficiaries = _context.Beneficiaries.ToList();

            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.Address = HttpContext.Session.GetString("Address");
            ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");


            var model =
                       from s in subscriptions
                       join u in userAccount on s.UserAccountId equals u.Id
                       select new JoinSubscription { subscription = s, userAccounts = u };
            var result = Tuple.Create<IEnumerable<Beneficiary>, IEnumerable<Subscription>, IEnumerable<Useraccount>, IEnumerable<JoinSubscription>>(beneficiaries, subscriptions, userAccount, model);

            var model1 = _context.Subscriptions.Where(x => x.SubscriptionsStatus == "paid").Include(s => s.UserAccount).ToList();
            if (startDate == null && endDate == null)
            {
                ViewBag.SubscriptionAmount = model.Sum(x => x.subscription.SubscriptionAmount);
                ViewBag.CountSubscriptionsPaid = model.Where(s => s.subscription.SubscriptionsStatus == "paid").Count();
                ViewBag.CountSubscriptionsNotPaid = model.Where(s => s.subscription.SubscriptionsStatus == "not Paid").Count();
                return View(result);
            }
            else if (endDate == null && startDate != null)
            {
                var finalResult = model.Where(x => x.subscription.SubscriptionDate.Value.Date >= startDate.Value.Date).ToList();
                result = Tuple.Create<IEnumerable<Beneficiary>, IEnumerable<Subscription>, IEnumerable<Useraccount>, IEnumerable<JoinSubscription>>(beneficiaries, subscriptions, userAccount, finalResult);
                ViewBag.SubscriptionAmount = finalResult.Sum(x => x.subscription.SubscriptionAmount);
                ViewBag.CountSubscriptionsPaid = finalResult.Where(s => s.subscription.SubscriptionsStatus == "paid").Count();
                ViewBag.CountSubscriptionsNotPaid = finalResult.Where(s => s.subscription.SubscriptionsStatus == "not Paid").Count();
                return View(result);
            }
            else if (endDate != null && startDate == null)
            {
                var finalResult = model.Where(x => x.subscription.SubscriptionDate.Value.Date <= endDate.Value.Date).ToList();
                result = Tuple.Create<IEnumerable<Beneficiary>, IEnumerable<Subscription>, IEnumerable<Useraccount>, IEnumerable<JoinSubscription>>(beneficiaries, subscriptions, userAccount, finalResult);
                ViewBag.SubscriptionAmount = finalResult.Sum(x => x.subscription.SubscriptionAmount);
                ViewBag.CountSubscriptionsPaid = finalResult.Where(s => s.subscription.SubscriptionsStatus == "paid").Count();
                ViewBag.CountSubscriptionsNotPaid = finalResult.Where(s => s.subscription.SubscriptionsStatus == "not Paid").Count();
                return View(result);
            }
            else
            {
                var finalResult = model.Where(x => x.subscription.SubscriptionDate.Value.Date >= startDate&& x.subscription.SubscriptionDate.Value.Date <= endDate).ToList();
                result = Tuple.Create<IEnumerable<Beneficiary>, IEnumerable<Subscription>, IEnumerable<Useraccount>, IEnumerable<JoinSubscription>>(beneficiaries, subscriptions, userAccount, finalResult);
                ViewBag.SubscriptionAmount = finalResult.Sum(x => x.subscription.SubscriptionAmount);
                ViewBag.CountSubscriptionsPaid = finalResult.Where(s=>s.subscription.SubscriptionsStatus== "paid").Count();
                ViewBag.CountSubscriptionsNotPaid = finalResult.Where(s => s.subscription.SubscriptionsStatus == "not Paid").Count();

                return View(result);
            
            }
        }

    }


}

        //    public IActionResult Index()
        //        {
        //            if(HttpContext.Session.GetInt32("UserId")!=null)
        //            {
        //                return View();

        //            }
        //            else
        //            {
        //                RedirectToAction("Index", "Home");
        //            }
        //            return View();
        //        }
        //    }
 //   }
