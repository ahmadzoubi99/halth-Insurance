using Health_Insurance.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Health_Insurance.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ModelContext _context;

        private readonly IWebHostEnvironment webHostEnvironment;

        public AuthenticationController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login([Bind("Id,FullName,Username,Passwd,Email,PhoneNumber,Address,RolesId,ImagePath")] Useraccount useraccount)
        {
            var user = _context.Useraccounts.Where(x => x.Username == useraccount.Username && x.Passwd == useraccount.Passwd).FirstOrDefault();
            if (user != null)
            {
                HttpContext.Session.SetString("Fullname", user.FullName);
                if (user.Imagepath != null)
                {
                    HttpContext.Session.SetString("ImagePath", user.Imagepath);

                }
                var sub=_context.Subscriptions.Where(x => x.UserAccountId==user.Id).FirstOrDefault();
                if (sub != null)
                {
                    HttpContext.Session.SetInt32("subscriptionID", (int)sub.Id);
                }
                HttpContext.Session.SetString("Address", user.Address);
                HttpContext.Session.SetInt32("UserId", (int)user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("PhoneNumber", user.PhoneNumber);
                HttpContext.Session.SetString("Passwd", user.Passwd);
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetInt32("RoleId", (int)user.RolesId);


                string layout;
                  
                
                switch (user.RolesId)
                {


                    case 1:
                         layout = "AdminLayout";
                        HttpContext.Session.SetString("Layout", layout);
                        return RedirectToAction("Index", "Admin");
                    case 2:

                         layout = "HomeLayout";
                        HttpContext.Session.SetString("Layout", layout);
                        return RedirectToAction("HomeUser", "User");
                }

            }
            else
            {
             
                    ViewBag.Error = "username or password is incoorect pleace try again .";
               
            }

            return View(useraccount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,FullName,Username,Passwd,Email,PhoneNumber,Address,RolesId,ImagePath,ImageFile")] Useraccount useraccount)
        {

            if (ModelState.IsValid)
            {
                if (useraccount.ImageFile != null)
                {
                    string wwwRootPath = webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + useraccount.ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/" + fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await useraccount.ImageFile.CopyToAsync(fileStream);
                    }

                    useraccount.Imagepath = fileName;
                }

                var user=_context.Useraccounts.Where(x=> x.Username == useraccount.Username).FirstOrDefault();
                var email=_context.Useraccounts.Where(x=> x.Email == useraccount.Email).FirstOrDefault();
                if (useraccount.Passwd != null && useraccount.Username != null && useraccount.Address != null &&useraccount.Email!=null&&useraccount.Imagepath!=null)
                {

                    if (user == null && email == null)
                    {
                        useraccount.RolesId = 2;
                        _context.Add(useraccount);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Login", "Authentication");
                    }
                    else
                    {
                        ViewBag.Error = "Email  or username is already used, pleace try  another one.";
                    }
                }
                else {
                    ViewBag.Error = "Make sure you fill out all your information.";

                }


            }
            ViewData["RolesId"] = new SelectList(_context.Roles, "Id", "Id", useraccount.RolesId);
            return View(useraccount);
        }


    }
}
