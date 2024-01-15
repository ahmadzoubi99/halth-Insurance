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
    public class BeneficiariesController : Controller
    {
        private readonly ModelContext _context;

        public BeneficiariesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Beneficiaries
        public async Task<IActionResult> Index()
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
            ViewBag.Login = HttpContext.Session.GetString("Login");
            ViewBag.subscriptionID = HttpContext.Session.GetInt32("subscriptionID");
            ViewBag.CountBeneficiariesAccepted = _context.Beneficiaries.Where(b=>b.BeneficiaryState== "Accept").Count();
            ViewBag.CountBeneficiarieswaiting = _context.Beneficiaries.Where(b => b.BeneficiaryState == "waiting").Count();


            var modelContext = _context.Beneficiaries.Include(b => b.Subscriptions).ThenInclude(u=>u.UserAccount);
            return View(await modelContext.ToListAsync());
        }

        // GET: Beneficiaries/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Beneficiaries == null)
            {
                return NotFound();
            }

            var beneficiary = await _context.Beneficiaries
                .Include(b => b.Subscriptions)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (beneficiary == null)
            {
                return NotFound();
            }

            return View(beneficiary);
        }

        // GET: Beneficiaries/Create
        public IActionResult Create()
        {
            ViewData["SubscriptionsId"] = new SelectList(_context.Subscriptions, "Id", "Id");
            return View();
        }

        // POST: Beneficiaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BeneficiaryName,BeneficiaryRelation,BeneficiaryState,BeneficiaryImageProof,SubscriptionsId,DateBeneficiary")] Beneficiary beneficiary,int subscriptionID)
        {
            if (ModelState.IsValid)
            {

                _context.Add(beneficiary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubscriptionsId"] = new SelectList(_context.Subscriptions, "Id", "Id", beneficiary.SubscriptionsId);
            return View(beneficiary);
        }

        // GET: Beneficiaries/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Beneficiaries == null)
            {
                return NotFound();
            }

            var beneficiary = await _context.Beneficiaries.FindAsync(id);
            if (beneficiary == null)
            {
                return NotFound();
            }
            ViewData["SubscriptionsId"] = new SelectList(_context.Subscriptions, "Id", "Id", beneficiary.SubscriptionsId);
            return View(beneficiary);
        }

        // POST: Beneficiaries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,BeneficiaryName,BeneficiaryRelation,BeneficiaryState,BeneficiaryImageProof,SubscriptionsId,DateBeneficiary")] Beneficiary beneficiary)
        {
            if (id != beneficiary.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(beneficiary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BeneficiaryExists(beneficiary.Id))
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
            ViewData["SubscriptionsId"] = new SelectList(_context.Subscriptions, "Id", "Id", beneficiary.SubscriptionsId);
            return View(beneficiary);
        }

        // GET: Beneficiaries/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {

            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.Address = HttpContext.Session.GetString("Address");
            ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");
            if (id == null || _context.Beneficiaries == null)
            {
                return NotFound();
            }

            var beneficiary = await _context.Beneficiaries
                .Include(b => b.Subscriptions)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (beneficiary == null)
            {
                return NotFound();
            }

            return View(beneficiary);
        }

        // POST: Beneficiaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {

            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.Address = HttpContext.Session.GetString("Address");
            ViewBag.ImagePath = HttpContext.Session.GetString("ImagePath");
            if (_context.Beneficiaries == null)
            {
                return Problem("Entity set 'ModelContext.Beneficiaries'  is null.");
            }
            var beneficiary = await _context.Beneficiaries.FindAsync(id);
            if (beneficiary != null)
            {
                _context.Beneficiaries.Remove(beneficiary);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BeneficiaryExists(decimal id)
        {
          return (_context.Beneficiaries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
