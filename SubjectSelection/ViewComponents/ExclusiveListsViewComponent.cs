using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubjectSelection.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SubjectSelection.ViewComponents
{
    public class ExclusiveListsViewComponent : ViewComponent
    {
        private readonly SubjectSelectionDbContext _context;

        private readonly UserManager<User> _userManager;

        public ExclusiveListsViewComponent(UserManager<User> userManager, SubjectSelectionDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public async Task<IViewComponentResult> InvokeAsync(int parentListId)
        {
            var user = await this.GetCurrentUserAsync();
            ViewBag.currentUserId = user.Id;

            var parentSubjectList = await _context.SubjectLists.Include(sl => sl.ExclusiveSubjectListsA).Include(sl => sl.ExclusiveSubjectListsB).FirstOrDefaultAsync(sl => sl.SubjectListId == parentListId);
            ViewBag.parentList = parentSubjectList;

            var exclusiveLists = parentSubjectList.ExclusiveSubjectListsA.Select(esl => esl.SubjectListBId).ToList();
            exclusiveLists.AddRange(parentSubjectList.ExclusiveSubjectListsB.Select(esl => esl.SubjectListAId));

            var subjectLists = _context.SubjectLists.Where(sl => exclusiveLists.Contains(sl.SubjectListId));

            return View(subjectLists);
        }
    }
}
