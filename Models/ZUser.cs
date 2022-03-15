using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZapperBugTracker.Models
{
    // Registered user's profile info
    // Custom ZUser: extend and inherit props/methods from IdentityUser
    public class ZUser : IdentityUser
    {        
        // IdentityUser already contants user id prop string
        // Data annotations (Display() is for the View)
        [Required]
        [Display(Name = "First Name")] 
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
