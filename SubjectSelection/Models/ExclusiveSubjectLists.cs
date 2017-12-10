using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SubjectSelection.Models
{
    public class ExclusiveSubjectLists //Wykluczające się listy przedmiotów (np. różne specjalizacje)
    {
        [Key]
        [Required]
        public int ExclusiveSubjectListsId { get; set; }

        public int SubjectListAId { get; set; }
        public SubjectList SubjectListA { get; set; }
        public int SubjectListBId { get; set; }
        public SubjectList SubjectListB { get; set; }

    }
}
