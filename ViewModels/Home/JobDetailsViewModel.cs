using JobPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobPortal.ViewModels.Home
{
    public class JobDetailsViewModel
    {
        public Job Job { get; set; }
        public bool IsApplied { get; set; }
    }
}
