using System.ComponentModel;

namespace ZapperBugTracker.Models
{
    public class TicketComment
    {
        // Primary key Id
        public int Id { get; set; }

        // Comment
        [DisplayName("Member Comment")]
        public string Comment { get; set; }

        // Created date
        [DisplayName("Date")]
        public DateTimeOffset Created { get; set; }

        // TicketId foreign key
        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        // UserId *string* not int - ZUser foreign key
        [DisplayName("Team Member")]
        public string UserId { get; set; }

        // Navigation properties/objects
        // Get access to entire entities and their props
        // Name vars should have same naming conventions as their respective foreign key above
        // Ticket Ticket
        public virtual Ticket Ticket { get; set; }

        // ZUser User
        public virtual ZUser User { get; set; }
    }
}
