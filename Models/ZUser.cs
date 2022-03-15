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

        // Avatar File
        public IFormFile AvatarFormFile { get; set; }

        // Avatar Name
        [Display(Name = "Avatar")]
        public string AvatarFileName { get; set; }

        // Avatar Data
        public byte[] AvatarFileData { get; set; }

        // Avatar File Extension/Content Type
        [Display(Name = "File Extension")]
        public string AvatarContentType { get; set; }

        // CompanyId foreign key, nullable will exist temporarily 
        public int? CompanyId { get; set; }

        // Navigation properties (relationships between models/class/tables)
        // Company (single associated company, no need for hashset)
        public virtual Company Company { get; set; }

        // Navigation collection properties
        // Projects
        public ICollection<Project> Projects { get; set; } = new HashSet<Project>();


    }
}
