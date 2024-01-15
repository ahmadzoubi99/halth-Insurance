using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Health_Insurance.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Http;

namespace Health_Insurance.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly ModelContext _context;

        public SubscriptionsController(ModelContext context)
        {
            _context = context;
        }




        // GET: Subscriptions
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("UserId") != null && HttpContext.Session.GetInt32("RoleId") == 1)
            {

                ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
                ViewBag.Address = HttpContext.Session.GetString("Address");
                ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");
                var modelContext = _context.Subscriptions.Include(s => s.UserAccount).OrderBy(s => s.SubscriptionDate);
                return View(await modelContext.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Authentication");
            }
        }

        

        // GET: Subscriptions/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Subscriptions == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                .Include(s => s.UserAccount).OrderBy(s=>s.SubscriptionDate)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        // GET: Subscriptions/Create
        public IActionResult Create()
        {
            ViewData["UserAccountId"] = new SelectList(_context.Useraccounts, "Id", "Id");
            return View();
        }

        // POST: Subscriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserAccountId,SubscriptionDate,SubscriptionsStatus,SubscriptionDuration,SubscriptionAmount")] Subscription subscription ,decimal id)
        {


            if (ModelState.IsValid)
            {
                var user = _context.Subscriptions.Where(s => s.UserAccountId == id).FirstOrDefault();

                if (user == null)
                {
                    subscription.UserAccountId = id;
                    subscription.SubscriptionDate= DateTime.Now;
                    subscription.SubscriptionsStatus = "not Paid";
                    subscription.SubscriptionDuration = 2;
                    subscription.SubscriptionAmount = 0;

                    _context.Add(subscription);
                    await _context.SaveChangesAsync();
                    var subid = HttpContext.Session.GetInt32("UserId");
                    var subeId = _context.Subscriptions.Where(s => s.UserAccountId == id).FirstOrDefault();
                    if (subeId != null)
                    {
                        HttpContext.Session.SetInt32("subscriptionId", (int)subeId.Id);
                        ViewBag.subscriptionID = HttpContext.Session.GetInt32("subscriptionID");

                    }
                    return RedirectToAction("HomeUser","User");
                }
                else
                {
                    ViewBag.erorr = "you are already Subscriptions";
                    return RedirectToAction("HomeUser","User");
                }
          
                
            }
            ViewData["UserAccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", subscription.UserAccountId);
            return RedirectToAction("HomeUser", "User");
        }

        // GET: Subscriptions/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Subscriptions == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }
            ViewData["UserAccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", subscription.UserAccountId);
            return View(subscription);
        }

        // POST: Subscriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    if (!SubscriptionExists(subscription.Id))
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

        // GET: Subscriptions/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Subscriptions == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                .Include(s => s.UserAccount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        // POST: Subscriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Subscriptions == null)
            {
                return Problem("Entity set 'ModelContext.Subscriptions'  is null.");
            }
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubscriptionExists(decimal id)
        {
          return (_context.Subscriptions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
