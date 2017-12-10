using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubjectSelection.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }

        public int ParentListId { get; set; }
        public SubjectList ParentList { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Group> Groups { get; set; }
        public ICollection<UserEditableSubjects> UsersWhoCanEdit { get; set; }
    }
}
