using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZapperBugTracker.Models
{
    // Custom ZUser: extend and inherit props/methods from IdentityUser
    public class ZUser : IdentityUser
    {
        // User profile info
        // Data annotations
        [Required]
        [Display(Name = "First Name")] // For the View
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // Not mapped to database:
        [NotMapped]
        [Display(Name = "Full Name")]
        public string FullName { get { return $"{FirstName} {LastName}"; } }

    }
}
