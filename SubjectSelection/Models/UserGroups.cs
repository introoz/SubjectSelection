using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubjectSelection.Models
{
    public class UserGroups //Sekcje do których zapisany jest użytkownik
    {
        public int UserGroupsId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
