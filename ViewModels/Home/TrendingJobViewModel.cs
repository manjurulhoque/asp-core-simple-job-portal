using JobPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobPortal.ViewModels.Home
{
    public class TrendingJobViewModel
    {
        public List<Job> Jobs { get; set; }
        public List<Job> Trendings { get; set; }
    }
}
