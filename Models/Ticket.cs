using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZapperBugTracker.Models
{
    public class Ticket
    {
        // Id primary key
        public int Id { get; set; }

        // Title
        [Required]
        [StringLength(50)]
        [DisplayName("Title")]
        public string Title { get; set; }

        // Description
        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }

        // Created date
        // Add DataType.Date
        [DataType(DataType.Date)]
        [DisplayName("Created")]
        public DateTimeOffset Created { get; set; }

        // Updated date
        // DateTimeOffset can be nullable if ticket created for first time
        [DataType(DataType.Date)]
        [DisplayName("Updated")]
        public DateTimeOffset? Updated { get; set; }

        // Archived
        [DisplayName("Archived")]
        public bool Archived { get; set; }

        // ProjectId foreign key
        [DisplayName("Project")]
        public int ProjectId { get; set; }

        // Ticket type foreign key to TicketType table
        [DisplayName("Ticket Type")]
        public int TicketTypeId { get; set; }

        // Ticket priority foreign key
        [DisplayName("Ticket Priority")]
        public int TicketPriorityId { get; set; }

        // Ticket status foreign key
        [DisplayName("Ticket Status")]
        public int TicketStatusId { get; set; }

        // Ticket owner string foreign key - ZUser
        [DisplayName("Ticket Owner")]
        public string OwnerUserId { get; set; }

        // Ticket developer string foreign key - ZUser
        [DisplayName("Ticket Developer")]
        public string DeveloperUserId { get; set; }

        // Navigation properties references
        // Virtual properties allow for lazy loading as opposed to eager loading
        // Get access to entities and their properties/methods
        // Project, TicketType, Priority, Status, Owner, and Developer
        public virtual Project Project { get; set; }
        public virtual TicketType TicketType { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }
        public virtual ZUser OwnerUser { get; set; }
        public virtual ZUser DeveloperUser { get; set; }

        // Navigation property collections (not going to DB, just references bc virtual)
        // Assign respective HashSet<Type> to each one 
        // Comments, Attachments, Nofitications, and History
        public virtual ICollection<TicketComment> Comments { get; set;} = new HashSet<TicketComment>();
        public virtual ICollection<TicketAttachment> Attachments { get; set; } = new HashSet<TicketAttachment>();
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public virtual ICollection<TicketHistory> History { get; set; } = new HashSet<TicketHistory>();

    }
}
