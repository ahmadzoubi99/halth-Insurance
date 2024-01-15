using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Health_Insurance.Models;
using System.Collections.Immutable;

namespace Health_Insurance.Controllers
{
    public class UseraccountsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public UseraccountsController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: Useraccounts
        public async Task<IActionResult> Index()
        {
                if (HttpContext.Session.GetInt32("UserId") != null)
                {
                var modelContext = _context.Useraccounts.Include(u => u.Roles);
                ViewBag.Layout = HttpContext.Session.GetString("Layout");
                ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
                ViewBag.Address = HttpContext.Session.GetString("Address");
                ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");
                ViewBag.Email = HttpContext.Session.GetString("Email");
                ViewBag.Passwd = HttpContext.Session.GetString("Passwd");
                ViewBag.PhoneNumber = HttpContext.Session.GetString("PhoneNumber");
                ViewBag.Username = HttpContext.Session.GetString("Username");
                return View(await modelContext.ToListAsync());
            }
            else
            {
               return RedirectToAction("Login","Authentication");
            }
              

             
        }

        // GET: Useraccounts/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.Address = HttpContext.Session.GetString("Address");
            ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Passwd = HttpContext.Session.GetString("Passwd");
            ViewBag.PhoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.Username = HttpContext.Session.GetString("Username");
            if (id == null || _context.Useraccounts == null)
            {
                return NotFound();
            }

            var useraccount = await _context.Useraccounts
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (useraccount == null)
            {
                return NotFound();
            }

            return View(useraccount);
        }

        // GET: Useraccounts/Create
        public IActionResult Create()
        {
            ViewData["RolesId"] = new SelectList(_context.Roles, "Id", "Id");
            return View();
        }

        // POST: Useraccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Username,Passwd,Email,PhoneNumber,Address,RolesId,Imagepath,ImageFile")] Useraccount useraccount)
        {
            if (ModelState.IsValid)
            {
                if(useraccount.ImageFile!=null)
                {
                    string WWWRootPath = webHostEnvironment.WebRootPath;
                    string fileName=Guid.NewGuid().ToString()+useraccount.ImageFile.FileName;
                    string path = Path.Combine(WWWRootPath + "/Images/" + fileName);


                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await useraccount.ImageFile.CopyToAsync(fileStream);
                    }

                    useraccount.Imagepath = fileName;
                }

                _context.Add(useraccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RolesId"] = new SelectList(_context.Roles, "Id", "Id", useraccount.RolesId);
            return View(useraccount);
        }

        // GET: Useraccounts/Edit/5
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

        // POST: Useraccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,FullName,Username,Passwd,Email,PhoneNumber,Address,RolesId,Imagepath,ImageFile")] Useraccount useraccount)
        {
           

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
                    HttpContext.Session.SetString("Fullname", useraccount.FullName);
                    HttpContext.Session.SetString("Address", useraccount.Address);
                    HttpContext.Session.SetInt32("UserId", (int)useraccount.Id);
                    HttpContext.Session.SetString("ImagePath", useraccount.Imagepath);
                    HttpContext.Session.SetString("Username", useraccount.Username);
                    HttpContext.Session.SetString("PhoneNumber", useraccount.PhoneNumber);
                    HttpContext.Session.SetString("Passwd", useraccount.Passwd);
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["RolesId"] = new SelectList(_context.Roles, "Id", "Id", useraccount.RolesId);
            return View(useraccount);
        }

        // GET: Useraccounts/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Useraccounts == null)
            {
                return NotFound();
            }

            var useraccount = await _context.Useraccounts
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (useraccount == null)
            {
                return NotFound();
            }

            return View(useraccount);
        }

        // POST: Useraccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Useraccounts == null)
            {
                return Problem("Entity set 'ModelContext.Useraccounts'  is null.");
            }
            var useraccount = await _context.Useraccounts.FindAsync(id);
            if (useraccount != null)
            {
                _context.Useraccounts.Remove(useraccount);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UseraccountExists(decimal id)
        {
          return (_context.Useraccounts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
