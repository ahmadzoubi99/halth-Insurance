using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Health_Insurance.Models;
using Microsoft.AspNetCore.Hosting;

namespace Health_Insurance.Controllers
{
    public class HomesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomesController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;   
        }

        // GET: Homes
        public async Task<IActionResult> Index()
        {
            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.Address = HttpContext.Session.GetString("Address");
            ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");
            return _context.Homes != null ? 
                          View(await _context.Homes.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Homes'  is null.");
        }

        // GET: Homes/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Homes == null)
            {
                return NotFound();
            }

            var home = await _context.Homes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (home == null)
            {
                return NotFound();
            }

            return View(home);
        }

        // GET: Homes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Homes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LogoName,Text1,Text2,Text3,Text4,EmailContactUsFooter,AboutUsTextFooter,PhoneNumberFooter,Image1,Image2,Image3,ImageFile1,ImageFile2,ImageFile3")] Home home)
        {
            if (ModelState.IsValid)
            {
                _context.Add(home);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(home);
        }

        // GET: Homes/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.Address = HttpContext.Session.GetString("Address");
            ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");
            if (id == null || _context.Homes == null)
            {
                return NotFound();
            }

            var home = await _context.Homes.FindAsync(id);
            if (home == null)
            {
                return NotFound();
            }
            return View(home);
        }

        // POST: Homes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,LogoName,Text1,Text2,Text3,Text4,EmailContactUsFooter,AboutUsTextFooter,PhoneNumberFooter,Image1,Image2,Image3,ImageFile1,ImageFile2,ImageFile3,ImageLogoFile")] Home home)
        {
            if (id != home.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (home.ImageFile2 != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;

                        string fileName = Guid.NewGuid().ToString() + home.ImageLogoFile.FileName;

                        string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await home.ImageLogoFile.CopyToAsync(fileStream);
                        }

                        home.LogoName = fileName;
                    }
                        if (home.ImageFile1 != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;

                        string fileName = Guid.NewGuid().ToString() + home.ImageFile1.FileName;

                        string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await home.ImageFile1.CopyToAsync(fileStream);
                        }

                        home.Image1 = fileName;
                    }
                    if (home.ImageFile2 != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;

                        string fileName = Guid.NewGuid().ToString() + home.ImageFile2.FileName;

                        string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await home.ImageFile2.CopyToAsync(fileStream);
                        }

                        home.Image2 = fileName;
                    }
                    if (home.ImageFile3 != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;

                        string fileName = Guid.NewGuid().ToString() + home.ImageFile3.FileName;

                        string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await home.ImageFile3.CopyToAsync(fileStream);
                        }

                        home.Image3 = fileName;
                    }


                    _context.Update(home);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeExists(home.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit", "Homes", new { id = 1 });
            }
            return RedirectToAction("Edit", "Homes", new { id = 1 });
        }

        // GET: Homes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Homes == null)
            {
                return NotFound();
            }

            var home = await _context.Homes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (home == null)
            {
                return NotFound();
            }

            return View(home);
        }

        // POST: Homes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Homes == null)
            {
                return Problem("Entity set 'ModelContext.Homes'  is null.");
            }
            var home = await _context.Homes.FindAsync(id);
            if (home != null)
            {
                _context.Homes.Remove(home);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeExists(decimal id)
        {
          return (_context.Homes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
