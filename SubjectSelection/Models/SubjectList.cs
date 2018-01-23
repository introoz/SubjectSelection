using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubjectSelection.Models
{
    public class SubjectList
    {
        public int SubjectListId { get; set; }

        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public string Name { get; set; }

        public ICollection<Subject> Subjects { get; set; }

        public ICollection<ExclusiveSubjectLists> ExclusiveSubjectListsA { get; set; }
        public ICollection<ExclusiveSubjectLists> ExclusiveSubjectListsB { get; set; }

        public ICollection<UserEditableLists> UsersWhoCanEdit { get; set; } 
    }
}
