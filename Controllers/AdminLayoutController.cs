using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace Health_Insurance.Controllers
{
    public class AdminLayoutController : Controller
    {
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        public IActionResult Index()
        {

            ViewBag.Layout=HttpContext.Session.GetString("Layout");
            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.Address = HttpContext.Session.GetString("Address");
            ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Passwd = HttpContext.Session.GetString("Passwd");
            ViewBag.PhoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Login = HttpContext.Session.GetString("Login");
            ViewBag.subscriptionID = HttpContext.Session.GetInt32("subscriptionID");

            

            return View();
        }
    }
}
