using Microsoft.AspNetCore.Http;
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
    public class UsersWhoCanEditViewComponent : ViewComponent
    {
        private readonly SubjectSelectionDbContext _context;

        private readonly UserManager<User> _userManager;

        public UsersWhoCanEditViewComponent(UserManager<User> userManager, SubjectSelectionDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public async Task<IViewComponentResult> InvokeAsync(int parentListId)
        {
            var user = await this.GetCurrentUserAsync();
            ViewBag.currentUserId = user.Id;

            var subjectList = await _context.SubjectLists
                .Include(sl => sl.UsersWhoCanEdit)
                    .ThenInclude(uel => uel.User)//x => x. //.Select(uel => uel.User))
                .Include(sl => sl.Owner)
                .FirstOrDefaultAsync(sl => sl.SubjectListId == parentListId);

            var usersWhoCanEdit = subjectList.UsersWhoCanEdit
                .Where(uel => uel.SubjectListId == parentListId)
                .Select(uel => uel.User)
                .ToList();

            usersWhoCanEdit.Insert(0, subjectList.Owner);

            ViewBag.parentList = subjectList;

            return View(usersWhoCanEdit);
        }
    }
}
