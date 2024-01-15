using Health_Insurance.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace Health_Insurance.Controllers
{
    public class HomeLayoutController : Controller
    {
        private readonly ModelContext _context;
        public IActionResult Index()
        {
            string LoginOrLogout;


            var home = _context.Homes.Where(h => h.Id == 1).FirstOrDefault();


            ViewBag.CurrentDate = DateTime.Now;
            ViewBag.logoname = home.LogoName;
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
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Passwd = HttpContext.Session.GetString("Passwd");
            ViewBag.PhoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.Username = HttpContext.Session.GetString("Username");
            LoginOrLogout =HttpContext.Session.GetInt32("UserId") == null ? "Login" :  "Logout";
            ViewBag.LoginOrLogout = LoginOrLogout;
            return View();
        }
    }
}
