using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Health_Insurance.Models;

namespace Health_Insurance.Controllers
{
    public class TestimonialsController : Controller
    {
        private readonly ModelContext _context;

        public TestimonialsController(ModelContext _context)
        {
            this._context = _context;
        }
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(decimal id, [Bind("Id,UseraccountId,TestimonialText,TestimonialDate,Status")] Testimonial testimonial)
        {
            if (id != testimonial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    testimonial.Status = "Accept";
                    _context.Update(testimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(testimonial.Id))
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
            ViewData["UserAccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", testimonial.UseraccountId);
            return View(testimonial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(decimal id, [Bind("Id,UseraccountId,TestimonialText,TestimonialDate,Status")] Testimonial testimonial)
        {
            if (id != testimonial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    testimonial.Status = "Reject";
                    _context.Update(testimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(testimonial.Id))
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
            ViewData["UserAccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", testimonial.UseraccountId);
            return View(testimonial);
        }
        // GET: Testimonials
        public async Task<IActionResult> Index()
        { 
            var userAccounts = _context.Useraccounts.ToList();
            var testimonial = _context.Testimonials.ToList();
            var model = from t in testimonial
                        join u in userAccounts on t.UseraccountId equals u.Id
                        orderby t.TestimonialDate
                        select new JoinTestimonial { userAccounts = u, testimonials = t };


            //  var model=Tuple.Create<IEnumerable<Useraccount>, IEnumerable<Testimonial>>(userAccounts, testimonial);
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



   
     


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,UserAccountId,SubscriptionDate,SubscriptionsStatus,SubscriptionDuration,SubscriptionAmount")] Subscription subscription)
        {
            if (id != subscription.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(subscription.Id))
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
            ViewData["UserAccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", subscription.UserAccountId);
            return View(subscription);
        }

   
        private bool TestimonialExists(decimal id)
        {
            return (_context.Testimonials?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        
        // GET: Testimonials/Details/5
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
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.Useraccount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // GET: Testimonials/Create
        public IActionResult Create()
        {
            ViewBag.Layout = HttpContext.Session.GetString("Layout");
            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.Address = HttpContext.Session.GetString("Address");
            ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Passwd = HttpContext.Session.GetString("Passwd");
            ViewBag.PhoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewData["UseraccountId"] = new SelectList(_context.Useraccounts, "Id", "Id");
            return View();
        }

        // POST: Testimonials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UseraccountId,TestimonialText,TestimonialDate,Status")] Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UseraccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", testimonial.UseraccountId);
            return View(testimonial);
        }

        // GET: Testimonials/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            ViewBag.Layout = HttpContext.Session.GetString("Layout");

            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.Address = HttpContext.Session.GetString("Address");
            ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Passwd = HttpContext.Session.GetString("Passwd");
            ViewBag.PhoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.Username = HttpContext.Session.GetString("Username");
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial == null)
            {
                return NotFound();
            }
            ViewData["UseraccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", testimonial.UseraccountId);
            return View(testimonial);
        }

        // POST: Testimonials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(decimal id, [Bind("Id,UseraccountId,TestimonialText,TestimonialDate,Status")] Testimonial testimonial)
        //{
        //    if (id != testimonial.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(testimonial);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TestimonialExists(testimonial.Id))
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
        //    ViewData["UseraccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", testimonial.UseraccountId);
        //    return View(testimonial);
        //}

        // GET: Testimonials/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.Useraccount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // POST: Testimonials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Testimonials == null)
            {
                return Problem("Entity set 'ModelContext.Testimonials'  is null.");
            }
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial != null)
            {
                _context.Testimonials.Remove(testimonial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //private bool TestimonialExists(decimal id)
        //{
        //  return (_context.Testimonials?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
