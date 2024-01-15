using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Health_Insurance.Models;

namespace Health_Insurance.Controllers
{


    public class EditProfileController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EditProfileController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Edit(decimal? id)
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
            ViewData["RolesId"] = new SelectList(_context.Roles, "Id", "Id", useraccount.RolesId);
            return View(useraccount);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,FullName,Username,Passwd,Email,PhoneNumber,Address,RolesId,Imagepath,ImageFile")] Useraccount useraccount)
        {
            HttpContext.Session.SetString("Fullname", useraccount.FullName);
            HttpContext.Session.SetString("Address", useraccount.Address);
            HttpContext.Session.SetInt32("UserId", (int)useraccount.Id);
            HttpContext.Session.SetString("ImagePath", useraccount.Imagepath);
            HttpContext.Session.SetString("Username", useraccount.Username);
            HttpContext.Session.SetString("PhoneNumber", useraccount.PhoneNumber);
            HttpContext.Session.SetString("Passwd", useraccount.Passwd);
            HttpContext.Session.SetString("Email", useraccount.Email);



            if (id != useraccount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["RolesId"] = new SelectList(_context.Roles, "Id", "Id", useraccount.RolesId);
            return View(useraccount);
        }

        private bool UseraccountExists(decimal id)
        {
            return _context.Useraccounts.Any(e => e.Id == id);
        }


    }
}
