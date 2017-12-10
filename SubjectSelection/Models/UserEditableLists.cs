using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubjectSelection.Models
{
    public class UserEditableLists //Listy które może edytować użytkownik
    {
        public int UserEditableListsId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public int SubjectListId { get; set; }
        public SubjectList SubjectList { get; set; }

    }
}
