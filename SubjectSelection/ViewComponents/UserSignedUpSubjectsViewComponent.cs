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
    public class UserSignedUpSubjectsViewComponent : ViewComponent
    {
        private readonly SubjectSelectionDbContext _context;

        private readonly UserManager<User> _userManager;

        public UserSignedUpSubjectsViewComponent(UserManager<User> userManager, SubjectSelectionDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public IViewComponentResult Invoke()
        {
            var user = this.GetCurrentUserAsync().Result;
            //_context.UserGroups.Where(ug => ug.UserId == user.Id).Select(ug => ug.GroupId)
            var currentUserDb = _context.User.Include(u => u.UserGroups).SingleOrDefault(u => u.Id == user.Id);
            var groupIds = currentUserDb.UserGroups.Select(ug => ug.GroupId);
            var subjectsSignedUpTo = _context.Groups.Include(g => g.Subject).Where(g => groupIds.Contains(g.GroupId)).Select(g => g.Subject);

            //var subjectsSignedUpTo = user.UserGroups?.Select(ug => ug.Group)?.Select(g => g.Subject);

            if (subjectsSignedUpTo == null)
                return View(new List<Subject>());

            return View(subjectsSignedUpTo);
        }
    }
}
