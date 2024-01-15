using Health_Insurance.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Health_Insurance.Controllers
{
    public class UserController : Controller
    {

        private readonly EmailService _emailService;


        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public UserController(ModelContext context, IWebHostEnvironment webHostEnvironment, EmailService emailService)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
            _emailService = emailService;
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
   

        public IActionResult PayforSubscroption()
        {
            if (HttpContext.Session.GetInt32("UserId") != null && HttpContext.Session.GetInt32("RoleId") == 2 ) {

                if (HttpContext.Session.GetInt32("subscriptionId") != null|| HttpContext.Session.GetInt32("subscriptionID")!=null)
                {
                    var home = _context.Homes.Where(h => h.Id == 1).FirstOrDefault();
                    ViewBag.CurrentDate = DateTime.Now;
                    ViewBag.LogoName = home.LogoName;
                    ViewBag.EmailContactUsFooter = home.EmailContactUsFooter;
                    ViewBag.PhoneNumberFooter = home.PhoneNumberFooter;
                    ViewBag.AboutUsTextFooter = home.AboutUsTextFooter;

                    ViewBag.Layout = HttpContext.Session.GetString("Layout");
                    ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
                    ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
                    ViewBag.Address = HttpContext.Session.GetString("Address");
                    ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");
                    ViewBag.PhoneNumber = HttpContext.Session.GetString("PhoneNumber");
                    ViewBag.Username = HttpContext.Session.GetString("Username");
                    //  ViewBag.subscriptionID = HttpContext.Session.GetInt32("subscriptionID");
                    if (HttpContext.Session.GetInt32("subscriptionID") != null)
                    {
                        var z = HttpContext.Session.GetInt32("subscriptionID");
                        ViewBag.ID = z;

                    }
                    else if (HttpContext.Session.GetInt32("subscriptionId") != null)
                    {
                        var z = HttpContext.Session.GetInt32("subscriptionId");
                        ViewBag.ID = z;

                    }
                    var bank = _context.Banks.ToList();


                    return View();
                }
                else 
                {
                    ViewBag.NotSubcription = "you are not subscription";
                    return  RedirectToAction("HomeUser","User"); 
                }
            }
        
            else
            {
                return RedirectToAction("Login", "Authentication");
            }
        }

  

        public IActionResult HomeUser()
        {
            if (HttpContext.Session.GetInt32("UserId")!=null)
            {
                var about = _context.Abouts.Where(a => a.Id == 1).FirstOrDefault();
                ViewBag.aboutText1 = about.Text1;
                ViewBag.aboutText2 = about.Text2;
                ViewBag.aboutText3 = about.Text3;
                ViewBag.aboutText4 = about.Text4;
                ViewBag.aboutText5 = about.Text5;
                ViewBag.aboutImage1 = about.Image1;
                ViewBag.aboutImage2 = about.Image2;
                ViewBag.aboutImage3 = about.Image3;


                var home = _context.Homes.Where(h => h.Id == 1).FirstOrDefault();

                ViewBag.CurrentDate = DateTime.Now;
                ViewBag.LogoName = home.LogoName;
                ViewBag.text1 = home.Text1;
                ViewBag.text2 = home.Text2;
                ViewBag.text3 = home.Text3;
                ViewBag.text4 = home.Text4;
                ViewBag.EmailContactUsFooter = home.EmailContactUsFooter;
                ViewBag.PhoneNumberFooter = home.PhoneNumberFooter;
                ViewBag.AboutUsTextFooter = home.AboutUsTextFooter;
                ViewBag.Image1 = home.Image1;
                ViewBag.Image2 = home.Image2;
                ViewBag.Image3 = home.Image3;

                ViewBag.Layout = HttpContext.Session.GetString("Layout");
                ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
                ViewBag.Address = HttpContext.Session.GetString("Address");
                ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");
                ViewBag.PhoneNumber = HttpContext.Session.GetString("PhoneNumber");
                ViewBag.Username = HttpContext.Session.GetString("Username");
              
                ViewBag.subscriptionId = HttpContext.Session.GetInt32("subscriptionId");
             

                int id = ViewBag.UserId;

                var subId = _context.Subscriptions.Where(s => s.UserAccountId == id).FirstOrDefault();
                if(subId != null)
                {
                    ViewBag.subscriptionId = subId.Id;
                }
                else
                {
                    ViewBag.subscriptionId = null;
                }
                var subscription = _context.Subscriptions.Where(x => x.SubscriptionsStatus == "paid");
                ViewBag.subscription = subscription != null ? "Subscription" : "Add Testimonial";

                return View();

            }
            else
            {
                return RedirectToAction("Login", "Authentication");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PayForSubscriptionit([Bind("Id,OwnerName,CardNumber,Cvv,ExpirationDateMonth,ExpirationDateYear")] Bank bank, int id)
        {
            if (id != bank.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userInBank = _context.Banks.Where(x => x.Cvv == bank.Cvv && x.CardNumber == bank.CardNumber).FirstOrDefault();

                    if (userInBank != null && userInBank.Balance >= 50)
                    {
                        var userSubscreption = _context.Subscriptions.Where(x => x.Id == id).Include(x=>x.UserAccount).FirstOrDefault();
                        if (userSubscreption.SubscriptionsStatus != "paid")
                        {
                            userInBank.Balance -= 50;
                            userSubscreption.SubscriptionsStatus = "paid";
                            userSubscreption.SubscriptionAmount = 50;
                            _context.Update(userInBank);
                            await _context.SaveChangesAsync();


                        
                            //Edit
                            string massege = "Hello " + userSubscreption.UserAccount.FullName + ", You are now subscribed to our health insurance and you can submit a request to add your family for free. This is the subscription invoice, the amount paid is 50$";

                            var pdfBytes = CreatePdfWithMessage(massege);


                            _emailService.SendEmailWithAttachment(userSubscreption.UserAccount.Email, "Test Subject", massege, pdfBytes);


                            ViewBag.notPaidCase = "You have successfully paid for your subscription.";
                            return RedirectToAction("PayforSubscroption", "User");

                        }
                        else
                        {
                            ViewBag.PaidCase = "You are already subscribed.";
                            return RedirectToAction("PayforSubscroption", "User");
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Invalid card information.";
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BankExists(bank.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.notPaidCase = "You have successfully paid for your subscription.";
            return RedirectToAction("PayforSubscroption", "User");


        }


        private bool BankExists(decimal id)
        {
            return (_context.Banks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HomeUser([Bind("Id,UseraccountId,TestimonialText,TestimonialDate,Status")] Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                testimonial.TestimonialDate = DateTime.Now;
                _context.Add(testimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction("HomeUser", "User");
            }
            ViewData["UseraccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", testimonial.UseraccountId);
            RedirectToAction("HomeUser", "User");
            return View();

        }


        public IActionResult Beneficiaries()
        {
            if (HttpContext.Session.GetInt32("UserId") != null && HttpContext.Session.GetInt32("RoleId") == 2)
            {
                var z=HttpContext.Session.GetInt32("UserId");
                var subid=_context.Subscriptions.Where(x=>x.UserAccountId== HttpContext.Session.GetInt32("UserId")).Include(u=>u.UserAccount).FirstOrDefault();

            if(subid != null)
               {
                   if (subid.SubscriptionsStatus == "paid")
                   {
                       var home = _context.Homes.Where(h => h.Id == 1).FirstOrDefault();
                       ViewBag.CurrentDate = DateTime.Now;
                       ViewBag.LogoName = home.LogoName;
                       ViewBag.EmailContactUsFooter = home.EmailContactUsFooter;
                       ViewBag.PhoneNumberFooter = home.PhoneNumberFooter;
                       ViewBag.AboutUsTextFooter = home.AboutUsTextFooter;
          
          
                       ViewBag.Layout = HttpContext.Session.GetString("Layout");
                       ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
                       ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
                       ViewBag.Address = HttpContext.Session.GetString("Address");
                       ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");
                       ViewBag.Email = HttpContext.Session.GetString("Email");
                      if (HttpContext.Session.GetInt32("subscriptionID") != null)
                        {
                            ViewBag.ID = HttpContext.Session.GetInt32("subscriptionID");

                        }
                        else if (HttpContext.Session.GetInt32("subscriptionId") != null)
                        {
                            ViewBag.ID = HttpContext.Session.GetInt32("subscriptionId");
                        }

                        int UserId = ViewBag.UserId;
                        var sub = _context.Subscriptions.Where(x => x.UserAccountId == UserId).FirstOrDefault();
                        if (HttpContext.Session.GetInt32("UserId") != null)
                        {
                            ViewBag.SubscriptionsId = HttpContext.Session.GetInt32("UserId");
                        }
                        else
                        {
                            return RedirectToAction("UserHome", "User");
                        }
                        ViewData["SubscriptionsId"] = new SelectList(_context.Subscriptions, "Id", "Id");


                        return View();

                    }
                    else
                    {
                        ViewBag.NotPaid = "you are not paid for Subscriptions";
                        return RedirectToAction("HomeUser", "User");
                    }
                }
            else
               {
                    ViewBag.NotPaid = "you are not Subscriptions";

                    return RedirectToAction("HomeUser", "User");
               }
              
            }
        else
        {
            return RedirectToAction("Login", "Authentication");
        }
    }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Beneficiaries([Bind("Id,BeneficiaryName,BeneficiaryRelation,BeneficiaryState,ImageFile,SubscriptionsId,DateBeneficiary")] Beneficiary beneficiary )
        {
            if (ModelState.IsValid)
            {
                 
                var sub=_context.Subscriptions.Where(x=>x.Id==beneficiary.SubscriptionsId).FirstOrDefault();
                if(beneficiary.SubscriptionsId != null &&beneficiary.SubscriptionsId!=0)
                {
                    if (sub.SubscriptionsStatus == "paid")
                    {
                        if (beneficiary.ImageFile != null)
                        {
                            string wwwRootPath = webHostEnvironment.WebRootPath;

                            string fileName = Guid.NewGuid().ToString() + beneficiary.ImageFile.FileName;

                            string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                await beneficiary.ImageFile.CopyToAsync(fileStream);
                            }

                            beneficiary.BeneficiaryImageProof = fileName;
                        }
                        beneficiary.DateBeneficiary = DateTime.Now;

                        _context.Add(beneficiary);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Beneficiaries", "User");


                    }
                    else
                    {
                        ViewBag.ErorrAddBeneficiaries = "you not Paid";
                    }
                    
                }
                else
                      
                {
                    ViewBag.ErorrAddBeneficiaries = "you not Subscriptions";
                }


            }
            ViewData["SubscriptionsId"] = new SelectList(_context.Subscriptions, "Id", "Id", beneficiary.SubscriptionsId);
            return View(beneficiary);
        }

     

    }



}
