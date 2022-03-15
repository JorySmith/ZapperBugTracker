using System.ComponentModel;

namespace ZapperBugTracker.Models
{
    public class Company
    {
        // Id primary key
        public int Id { get; set; }

        // Name
        [DisplayName("Company Name")]
        public string Name { get; set; }

        // Description
        [DisplayName("Company Description")]
        public string Description { get; set; }

        // Navigation collection properties
        // ZUsers and Projects
        public virtual ICollection<ZUser> Users { get; set; } = new HashSet<ZUser>();
        public virtual ICollection<Project> Projects { get; set; } = new HashSet<Project>();

        // Relationship to User invites
    }
}
