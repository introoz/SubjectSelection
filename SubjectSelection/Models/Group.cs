using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubjectSelection.Models
{
    public class Group // Sekcja
    {
        public int GroupId { get; set; }
        public int Capacity { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public string Name { get; set; }
        public string ClassDate { get; set; } //Dzień i godzina zajęć

        public ICollection<UserGroups> UsersInGroup { get; set; }
    }
}
