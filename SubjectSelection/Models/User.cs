using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubjectSelection.Models
{
    public class User: IdentityUser<int>
    {
        //public int UserId { get; set; }
        //public string Username { get; set; }
        public int StudentCardId { get; set; } //Numer albumu
       
        public ICollection<UserEditableLists> ListsEditableByUser { get; set; }
        public ICollection<UserEditableSubjects> SubjectsEditableByUser { get; set; }
        public ICollection<UserGroups> UserGroups { get; set; }
        public ICollection<SubjectList> UserOwnedSubjectLists { get; set; }
    }
}
