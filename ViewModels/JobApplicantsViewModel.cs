using System.Collections.Generic;
using JobPortal.Models;

namespace JobPortal.ViewModels
{
    public class JobApplicantsViewModel
    {
        public Job Job { get; set; }

        public List<Applicant> Applicants { get; set; }
    }
}