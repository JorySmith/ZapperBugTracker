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
        // ZUser Members, Projects, and User Invites
        public virtual ICollection<ZUser> Members { get; set; } = new HashSet<ZUser>();
        public virtual ICollection<Project> Projects { get; set; } = new HashSet<Project>();        
        public virtual ICollection<Invite> Invites { get; set; } = new HashSet<Invite>();


    }
}
