using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SubjectSelection.Models;

namespace SubjectSelection.Controllers
{
    public class GroupsController : Controller
    {
        private readonly UserManager<User> _userManager;

        private readonly SubjectSelectionDbContext _context;

        public GroupsController(UserManager<User> userManager, SubjectSelectionDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Groups
        public async Task<IActionResult> Index(int parentSubjectId)
        {
            var parentSubject = await _context.Subjects
                .Include(s => s.ParentList)
                .ThenInclude(sl => sl.UsersWhoCanEdit)
                .ThenInclude(uel => uel.User)
                .Include(s => s.Groups)
                .FirstAsync(s => s.SubjectId == parentSubjectId);

            ViewBag.parentSubjectName = parentSubject.Name;
            ViewBag.parentSubjectId = parentSubjectId;

            var usersWhoCanEdit = parentSubject.ParentList.UsersWhoCanEdit
                .Where(uel => uel.SubjectListId == parentSubject.ParentList.SubjectListId)
                .Select(uel => uel.User)
                .Select(u => u.Id)
                .ToList();

            usersWhoCanEdit.AddRange(usersWhoCanEdit);

            usersWhoCanEdit.Insert(0, parentSubject.ParentList.OwnerId);

            ViewBag.usersWhoCanEdit = usersWhoCanEdit;            

            var currentUser = await this.GetCurrentUserAsync();
            ViewBag.currentUserId = currentUser.Id;


            return View(parentSubject.Groups);
        }

        // GET: Groups/CreateSubject
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGroup(string groupName, int groupCapacity, int parentSubjectId)
        {
            var parentSubject = await _context.Subjects.FirstOrDefaultAsync(s => s.SubjectId == parentSubjectId);

            var group = new Group()
            {
                Name = groupName,
                SubjectId = parentSubjectId,
                Subject = parentSubject,
                Capacity = groupCapacity
            };

            if (ModelState.IsValid)
            {
                _context.Add(group);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { parentSubjectId = parentSubjectId });
            }

            //TODO
            //Jak model invalid
            return View();
        }

        public async Task<IActionResult> Join(int groupId, int parentSubjectId)
        {
            var currentUser = await this.GetCurrentUserAsync();

            var parentList = (await _context.Subjects
                .Include(s => s.ParentList)
                .ThenInclude(sl => sl.ExclusiveSubjectListsA)
                .Include(s => s.ParentList)
                .ThenInclude(sl => sl.ExclusiveSubjectListsB)
                .FirstOrDefaultAsync(s => s.SubjectId == parentSubjectId)).ParentList;

            var exclusiveSubjectListsIds = parentList.ExclusiveSubjectListsA.Select(esl => esl.SubjectListBId)?.ToList();
            exclusiveSubjectListsIds.AddRange(parentList.ExclusiveSubjectListsB.Select(esl => esl.SubjectListAId));

            var exclusiveSubjectLists = _context.SubjectLists
                .Include(sl => sl.Subjects)
                .ThenInclude(s => s.Groups)
                .ThenInclude(g => g.UsersInGroup)
                .Where(sl => exclusiveSubjectListsIds.Contains(sl.SubjectListId) && sl.Subjects.Where(s => s.Groups.Where(g => g.UsersInGroup.Where(uig => uig.UserId == currentUser.Id).Select(uig => uig.GroupId).Contains(g.GroupId)).Select(g => g.SubjectId).Contains(s.SubjectId)).Select(s => s.ParentListId).Contains(sl.SubjectListId));

            if (!exclusiveSubjectLists.Any())
            {
                var userGroup = await _context.UserGroups?.FirstOrDefaultAsync(ug => ug.GroupId == groupId && ug.UserId == currentUser.Id);
                if (userGroup == null)
                {
                    var newUserGroup = new UserGroups()
                    {
                        GroupId = groupId,
                        UserId = currentUser.Id,
                        User = currentUser
                    };

                    if (ModelState.IsValid)
                    {
                        _context.Add(newUserGroup);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", new { parentSubjectId = parentSubjectId });
                    }
                }
                else
                    return RedirectToAction("Index", new { parentSubjectId = parentSubjectId });
            }


            //TODO
            //Tutaj wchodzi jak ModelState invalid, zmienic
            //return View();
            return RedirectToAction(nameof(Error));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }






        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Groups
                .Include(g => g.UsersInGroup)
                .SingleOrDefaultAsync(m => m.GroupId == id);
            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectId");
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupId,Capacity,SubjectId,Name,ClassDate")] Group @group)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@group);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectId", @group.SubjectId);
            return View(@group);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups.SingleOrDefaultAsync(m => m.GroupId == id);
            if (@group == null)
            {
                return NotFound();
            }
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectId", @group.SubjectId);
            return View(@group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupId,Capacity,SubjectId,Name,ClassDate")] Group group)
        {
            if (id != group.GroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(group.GroupId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { parentSubjectId = group.SubjectId });
            }
            //ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectId", group.SubjectId);
            return View(group);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Groups
                .Include(g => g.Subject)
                .SingleOrDefaultAsync(m => m.GroupId == id);
            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var group = await _context.Groups.SingleOrDefaultAsync(m => m.GroupId == id);
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { parentSubjectId = group.SubjectId });
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.GroupId == id);
        }
    }
}
