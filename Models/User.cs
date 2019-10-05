using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class User : IdentityUser
    {
        [MaxLength(20)]
        public string FirstName { get; set; }
        [MaxLength(60)]
        public string LastName { get; set; }
        [MaxLength(10)]
        public string Gender { get; set; }
    }
}
