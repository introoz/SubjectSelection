using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SubjectSelection.Models;

namespace SubjectSelection.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;

        private readonly SubjectSelectionDbContext _context;

        public HomeController(UserManager<User> userManager, SubjectSelectionDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public async Task<IActionResult> Index()
        {
            ViewBag.search = false;

            var user = await this.GetCurrentUserAsync();

            ViewBag.currentUserId = user.Id;

            var dbUser = _context.Users.Include(u => u.UserOwnedSubjectLists).First(us => us.Id == user.Id);

            if (dbUser.UserOwnedSubjectLists == null)
                return View(new List<SubjectList>());

            return View(dbUser.UserOwnedSubjectLists);
        }

        [HttpGet]
        public async Task<IActionResult> SearchSubjectLists(string subjectListName)
        {
            ViewBag.search = true;

            var foundSubjectLists = await _context.SubjectLists.Where(sl => sl.Name.Contains(subjectListName)).ToListAsync();
            return View("Index", foundSubjectLists);
        }

        // POST: Home/CreateSubjectList
        [HttpPost]
        public async Task<IActionResult> CreateSubjectList(string subjectListName)
        {
            if (!String.IsNullOrEmpty(subjectListName))
            {
                var user = await this.GetCurrentUserAsync();
                var subjectList = new SubjectList()
                {
                    Name = subjectListName,
                    OwnerId = user.Id,
                    Owner = user
                };

                if (ModelState.IsValid)
                {
                    _context.Add(subjectList);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            //TODO
            //Tutaj wchodzi jak ModelState invalid, zmienic
            //ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", subjectList.OwnerId);
            //return View();
            return RedirectToAction(nameof(Index));
        }

        // GET: Home/DeleteSubjectList
        public async Task<IActionResult> DeleteSubjectList(int? subjectListId)
        {
            if (subjectListId == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                var user = await this.GetCurrentUserAsync();

                var subjectListToDelete = await _context.SubjectLists.FirstOrDefaultAsync(sl => sl.SubjectListId == subjectListId && sl.OwnerId == user.Id);

                if (subjectListToDelete != null)
                {

                    _context.SubjectLists.Remove(subjectListToDelete);

                    await _context.SaveChangesAsync();

                }
                return RedirectToAction(nameof(Index));
            }

            //TODO
            //Tutaj wchodzi jak ModelState invalid, zmienic
            return View();
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(int? subjectListId)
        {
            if (subjectListId == null)
            {
                return NotFound();
            }

            var subjectList = await _context.SubjectLists
                .Include(sl => sl.Owner)
                .SingleOrDefaultAsync(m => m.SubjectListId == subjectListId);
            if (subjectList == null)
            {
                return NotFound();
            }

            return View(subjectList);
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(int? parentListId)
        {
            if (parentListId == null)
            {
                return NotFound();
            }

            var subjectList = await _context.SubjectLists.SingleOrDefaultAsync(m => m.SubjectListId == parentListId);
            if (subjectList == null)
            {
                return NotFound();
            }
            //ViewData["ParentListId"] = new SelectList(_context.SubjectLists, "SubjectListId", "SubjectListId", SubjectList.ParentListId);
            return View(subjectList);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int subjectListId, [Bind("SubjectListId,OwnerId,Name")] SubjectList subjectList)
        {
            if (subjectListId != subjectList.SubjectListId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subjectList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectListExists(subjectList.SubjectListId))
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
            //ViewData["ParentListId"] = new SelectList(_context.SubjectLists, "SubjectListId", "SubjectListId", subject.ParentListId);
            return View(subjectList);
        }

        private bool SubjectListExists(int id)
        {
            return _context.SubjectLists.Any(e => e.SubjectListId == id);
        }


        //public ActionResult DeleteModalPopUp(int subjectListId)
        //{
        //    var subjectList = _context.SubjectLists.Include(sl => sl.Subjects).FirstOrDefault(sl => sl.SubjectListId == subjectListId);
        //    if(subjectList.Subjects.Count > 0)
        //    return View();
        //}

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Test()
        {
            ViewData["Message"] = "Your profile page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
