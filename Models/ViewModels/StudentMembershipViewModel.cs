using Lab4.Models;
using Lab4.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment1.Models.ViewModels
{
    public class StudentMembershipViewModel
    {
        public Student Student { get; set; }
        public IEnumerable<StudentViewModel> Memberships { get; set; }


    }
}
