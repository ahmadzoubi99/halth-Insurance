using Health_Insurance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Health_Insurance.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;

        public HomeController(ILogger<HomeController> logger, ModelContext context)
        {
            _logger = logger;
            this._context = context;
        }


        public async Task<IActionResult> Index()
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


            var userAccounts = _context.Useraccounts.ToList();
            var testimonial = _context.Testimonials.Where(x=>x.Status=="Accept").ToList();
            var model = from t in testimonial
                        join u in userAccounts on t.UseraccountId equals u.Id
                        select new JoinTestimonial { userAccounts = u, testimonials = t };

            //var result=Tuple.Create<IEnumerable<JoinTestimonial>,IEnumerable<Home>>(model, home);


            //  var model=Tuple.Create<IEnumerable<Useraccount>, IEnumerable<Testimonial>>(userAccounts, testimonial);
            ViewBag.Layout = HttpContext.Session.GetString("Layout");
            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.Address = HttpContext.Session.GetString("Address");
            ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Passwd = HttpContext.Session.GetString("Passwd");
            ViewBag.PhoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.Username = HttpContext.Session.GetString("Username");
            var modelContext = _context.Testimonials.Include(t => t.Useraccount);
            return View(model);
        }

        public IActionResult About()
        {
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

            var about = _context.Abouts.Where(a => a.Id == 1).FirstOrDefault();
            ViewBag.aboutText1 = about.Text1;
            ViewBag.aboutText2 = about.Text2;
            ViewBag.aboutText3 = about.Text3;
            ViewBag.aboutText4 = about.Text4;
            ViewBag.aboutText5 = about.Text5;
            ViewBag.aboutImage1 = about.Image1;
            ViewBag.aboutImage2 = about.Image2;
            ViewBag.aboutImage3 = about.Image3;



            ViewBag.Layout = HttpContext.Session.GetString("Layout");
            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.Address = HttpContext.Session.GetString("Address");
            ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Passwd = HttpContext.Session.GetString("Passwd");
            ViewBag.PhoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.Username = HttpContext.Session.GetString("Username");

            return View();
        }
        public IActionResult ContatUS()
        {
            var home = _context.Homes.Where(h => h.Id == 1).FirstOrDefault();
            ViewBag.EmailContactUsFooter = home.EmailContactUsFooter;
            ViewBag.PhoneNumberFooter = home.PhoneNumberFooter;
            ViewBag.AboutUsTextFooter = home.AboutUsTextFooter;


            ViewBag.Layout = HttpContext.Session.GetString("Layout");
            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.Address = HttpContext.Session.GetString("Address");
            ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Passwd = HttpContext.Session.GetString("Passwd");
            ViewBag.PhoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.Username = HttpContext.Session.GetString("Username");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([Bind("Id,Name,Text1,AddressText,PhoneNumber,Email")] ContactU contactu)
        {
            ViewBag.CurrentDate = DateTime.Now;
            _context.Add(contactu);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
      
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult testimonials()
        {
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


            var userAccounts = _context.Useraccounts.ToList();
            var testimonial = _context.Testimonials.Where(x => x.Status == "Accept").ToList();
            var model = from t in testimonial
                        join u in userAccounts on t.UseraccountId equals u.Id
                        select new JoinTestimonial { userAccounts = u, testimonials = t };

            return View(model);
        }


    }


}