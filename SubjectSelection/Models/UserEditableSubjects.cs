using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubjectSelection.Models
{
    public class UserEditableSubjects //Przedmioty, które może edytowac użytkownik
    {
        public int UserEditableSubjectsId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}
