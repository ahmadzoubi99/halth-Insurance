using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Health_Insurance.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace Health_Insurance.Controllers
{
    public class EdirProfileeController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EdirProfileeController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Edit(decimal? id)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                if (id == null || _context.Useraccounts == null)
                {
                    return NotFound();
                }

                var useraccount = await _context.Useraccounts.FindAsync(id);
                if (useraccount == null)
                {
                    return NotFound();
                }
                ViewBag.Layout = HttpContext.Session.GetString("Layout");
                ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
                ViewBag.Address = HttpContext.Session.GetString("Address");
                if(useraccount.Imagepath!=null)
                {
                    ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");
                }
                ViewBag.Email = HttpContext.Session.GetString("Email");
                ViewBag.Passwd = HttpContext.Session.GetString("Passwd");
                ViewBag.PhoneNumber = HttpContext.Session.GetString("PhoneNumber");
                ViewBag.Username = HttpContext.Session.GetString("Username");

                ViewData["RolesId"] = new SelectList(_context.Roles, "Id", "Id", useraccount.RolesId);
                return View(useraccount);
            }
            else
            {
                return RedirectToAction("Login", "Authentication");

                
            }

            return View();
         
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,FullName,Username,Passwd,Email,PhoneNumber,Address,RolesId,Imagepath,ImageFile")] Useraccount useraccount)
        {
            string layout;

            if (useraccount.RolesId == 1)
            {
                layout = "AdminLayout";
            }
            else
            {
                layout = "HomeLayout";
            }
            HttpContext.Session.SetString("Layout", layout);


            HttpContext.Session.SetString("ImagePath", useraccount.Imagepath);
            if (id != useraccount.Id)
            {
                return NotFound();
            }

            _context.Entry(useraccount).State = EntityState.Detached;


            //var existingUser = _context.Useraccounts.FirstOrDefault(u => u.Id != useraccount.Id && u.Username == useraccount.Username);
            //var existingEmail = _context.Useraccounts.FirstOrDefault(u => u.Id != useraccount.Id && u.Email == useraccount.Email);

            var existingUser = _context.Useraccounts.Where(u => u.Id != useraccount.Id && u.Username == useraccount.Username).FirstOrDefault();
            var existingEmail = _context.Useraccounts.Where(u => u.Id != useraccount.Id && u.Email == useraccount.Email).FirstOrDefault();

            if (existingUser != null || existingEmail != null)
            {
                ViewBag.Error = "Email or username is already used. Please try another one.";
                ViewData["RolesId"] = new SelectList(_context.Roles, "Id", "Id", useraccount.RolesId);
                
                return RedirectToAction("Edit", "EdirProfilee");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //if (useraccount.Passwd == null)
                    //{
                    //    var oldPassword = _context.Useraccounts.Where(x => x.Id == useraccount.Id).FirstOrDefault();
                    //    useraccount.Passwd = oldPassword.Passwd;
                    //}


                    if (useraccount.ImageFile != null)
                    {
                        string WWWRootPath = webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + useraccount.ImageFile.FileName;
                        string path = Path.Combine(WWWRootPath + "/Images/" + fileName);


                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await useraccount.ImageFile.CopyToAsync(fileStream);
                        }

                        useraccount.Imagepath = fileName;
                    }



                 
                    HttpContext.Session.SetString("Fullname", useraccount.FullName);
                    HttpContext.Session.SetString("Address", useraccount.Address);
                    HttpContext.Session.SetInt32("UserId", (int)useraccount.Id);
                    HttpContext.Session.SetString("ImagePath", useraccount.Imagepath);
                    HttpContext.Session.SetString("Username", useraccount.Username);
                    HttpContext.Session.SetString("PhoneNumber", useraccount.PhoneNumber);
                    HttpContext.Session.SetString("Email", useraccount.Email);

                    _context.Update(useraccount);
                    await _context.SaveChangesAsync();


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UseraccountExists(useraccount.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if(useraccount.RolesId==1)
                {
                    return RedirectToAction("Edit", "EdirProfilee");
                }
                else
                {
                    return RedirectToAction("Edit", "EdirProfilee");
                }

            }
            ViewData["RolesId"] = new SelectList(_context.Roles, "Id", "Id", useraccount.RolesId);
            return View(useraccount);
        }

        private bool UseraccountExists(decimal id)
        {
          return (_context.Useraccounts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
