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
    public class SubjectsController : Controller
    {

        private readonly UserManager<User> _userManager;

        private readonly SubjectSelectionDbContext _context;

        public SubjectsController(SubjectSelectionDbContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Subjects
        public async Task<IActionResult> Index(int parentListId)
        {
            try
            {
                var user = await this.GetCurrentUserAsync();
                ViewBag.currentUserId = user.Id;

                var parentList = await _context.SubjectLists
                    .Include(sl => sl.Subjects)
                    .Include(sl => sl.UsersWhoCanEdit)
                    .ThenInclude(uel => uel.User)
                    .FirstAsync(sl => sl.SubjectListId == parentListId);

                ViewBag.parentListName = parentList.Name;
                ViewBag.parentList = parentList;

                var usersWhoCanEdit = parentList.UsersWhoCanEdit
                .Where(uel => uel.SubjectListId == parentListId)
                .Select(uel => uel.User)
                .Select(u => u.Id)
                .ToList();

                usersWhoCanEdit.AddRange(usersWhoCanEdit);

                usersWhoCanEdit.Insert(0, parentList.OwnerId);

                ViewBag.usersWhoCanEdit = usersWhoCanEdit;//user.Id;


                //SubjectLists do dropdownu
                var subjectLists = _context.SubjectLists.Where(sl => sl.OwnerId == parentList.OwnerId && sl.SubjectListId != parentListId);
                ViewBag.subjectLists = new SelectList(subjectLists, "SubjectListId", "Name");

                return View(parentList.Subjects);
            }
            catch (InvalidOperationException ex)
            {
                return RedirectToAction(nameof(Error));
            }
        }


        // GET: Subjects/CreateSubject
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubject(string subjectName, string subjectDescription, int parentListId)
        {
            var user = await this.GetCurrentUserAsync();

            var parentList = await _context.SubjectLists.FirstOrDefaultAsync(sl => sl.SubjectListId == parentListId);

            if (parentList.OwnerId == user.Id)
            {
                var subject = new Subject()
                {
                    Name = subjectName,
                    Description = subjectDescription,
                    ParentListId = parentListId,
                    ParentList = parentList
                };

                if (ModelState.IsValid)
                {
                    _context.Add(subject);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", new { parentListId = parentListId });
                }
            }


            //TODO
            //Jak model invalid
            //return View();
            return RedirectToAction("Index", new { parentListId = parentListId });
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(int userId, int parentListId)
        {
            try
            {
                var user = await this.GetCurrentUserAsync();
                var subjectList = await _context.SubjectLists.FirstOrDefaultAsync(sl => sl.SubjectListId == parentListId);

                if (subjectList.OwnerId == user.Id)
                {
                    var userEditableList = new UserEditableLists()
                    {
                        SubjectListId = parentListId,
                        UserId = userId
                    };

                    if (ModelState.IsValid)
                    {
                        _context.Add(userEditableList);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", new { parentListId = parentListId });
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                return RedirectToAction(nameof(Error));
            }
            return RedirectToAction(nameof(Error));
        }

        public async Task<IActionResult> AddExlusiveList(int subjectListId, int parentListId)
        {
            var user = await this.GetCurrentUserAsync();
            var parentList = await _context.SubjectLists.FirstOrDefaultAsync(sl => sl.SubjectListId == parentListId);
            if (parentList.OwnerId == user.Id)
            {
                var exclusiveList = new ExclusiveSubjectLists()
                {
                    SubjectListAId = parentListId,
                    SubjectListBId = subjectListId
                };

                if (ModelState.IsValid)
                {
                    _context.Add(exclusiveList);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", new { parentListId = parentListId });
                }
            }

            return RedirectToAction(nameof(Error));
        }

        public async Task<IActionResult> DeleteExclusiveList(int subjectListId, int parentListId)
        {
            var user = await this.GetCurrentUserAsync();
            var parentList = await _context.SubjectLists.FirstOrDefaultAsync(sl => sl.SubjectListId == parentListId);

            if (parentList.OwnerId == user.Id)
            {
                var exclusiveSubjectList = await _context.ExclusiveSubjectLists.FirstOrDefaultAsync(esl => (esl.SubjectListAId == subjectListId && esl.SubjectListBId == parentListId) || (esl.SubjectListAId == parentListId && esl.SubjectListBId == subjectListId));

                if (ModelState.IsValid)
                {
                    _context.Remove(exclusiveSubjectList);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", new { parentListId = parentListId });
                }
            }

            return RedirectToAction(nameof(Error));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }





        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.ParentList)
                .SingleOrDefaultAsync(m => m.SubjectId == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            ViewData["ParentListId"] = new SelectList(_context.SubjectLists, "SubjectListId", "SubjectListId");
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubjectId,ParentListId,Name,Description")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentListId"] = new SelectList(_context.SubjectLists, "SubjectListId", "SubjectListId", subject.ParentListId);
            return View(subject);
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.SingleOrDefaultAsync(m => m.SubjectId == id);
            if (subject == null)
            {
                return NotFound();
            }
            ViewData["ParentListId"] = new SelectList(_context.SubjectLists, "SubjectListId", "SubjectListId", subject.ParentListId);
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubjectId,ParentListId,Name,Description")] Subject subject)
        {
            if (id != subject.SubjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.SubjectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", new { parentListId = subject.ParentListId });

            }
            //ViewData["ParentListId"] = new SelectList(_context.SubjectLists, "SubjectListId", "SubjectListId", subject.ParentListId);
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.ParentList)
                .SingleOrDefaultAsync(m => m.SubjectId == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int parentListId)
        {
            var subject = await _context.Subjects.SingleOrDefaultAsync(m => m.SubjectId == id);
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", new { parentListId = parentListId });
        }

        private bool SubjectExists(int id)
        {
            return _context.Subjects.Any(e => e.SubjectId == id);
        }
    }
}
