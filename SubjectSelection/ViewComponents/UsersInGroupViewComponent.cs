using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubjectSelection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubjectSelection.ViewComponents
{
    public class UsersInGroupViewComponent : ViewComponent
    {
        private readonly SubjectSelectionDbContext _context;
        private readonly UserManager<User> _userManager;

        public UsersInGroupViewComponent(UserManager<User> userManager, SubjectSelectionDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public async Task<IViewComponentResult> InvokeAsync(int groupId)
        {
            var user = await this.GetCurrentUserAsync();

            var group = await _context.Groups
                .Include(g => g.Subject)
                .FirstOrDefaultAsync(g => g.GroupId == groupId);

            var parentListId = group.Subject.ParentListId;

            var parentList = await _context.SubjectLists
                .Include(sl => sl.UsersWhoCanEdit)
                .ThenInclude(uel => uel.User)
                .FirstOrDefaultAsync(sl => sl.SubjectListId == parentListId);

            var usersWhoCanEdit = parentList.UsersWhoCanEdit
                .Where(uel => uel.SubjectListId == parentList.SubjectListId)
                .Select(uel => uel.User)
                .Select(u => u.Id)
                .ToList();

            usersWhoCanEdit.AddRange(usersWhoCanEdit);

            usersWhoCanEdit.Insert(0, parentList.OwnerId);

            ViewBag.usersWhoCanEdit = usersWhoCanEdit;
            ViewBag.currentUserId = user.Id;

            var usersInGroupIds = _context.UserGroups.Where(ug => ug.GroupId == groupId).Select(ug => ug.UserId);
            var usersInGroup = _context.Users.Where(u => usersInGroupIds.Contains(u.Id));

            return View(usersInGroup);
        }
    }
}