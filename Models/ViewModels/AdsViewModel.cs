using Lab4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment1.Models.ViewModels
{
    public class AdsViewModel
    {
        public Advertisement Advertisement { get; set; }
        public Community Community { get; set; }
        public IEnumerable<Advertisement> Advertisements { get; set; }
    }
}
