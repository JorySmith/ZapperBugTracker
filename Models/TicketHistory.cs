using System.ComponentModel;

namespace ZapperBugTracker.Models
{
    // Track history of changes to tickets
    public class TicketHistory
    {
        // Primary key
        public int Id { get; set; }

        // Foreign key to the ticket
        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        // Track changes of properties
        [DisplayName("Updated Item")]
        public string Property { get; set; }

        // Track old prop value
        [DisplayName("Previous")]
        public string OldValue { get; set; }

        // Track new prop value
        [DisplayName("Current")]
        public string NewValue { get; set; }

        // Track date of change using UTC standard
        [DisplayName("Date Modified")]
        public DateTimeOffset Created { get; set; }

        // Track prop change description
        [DisplayName("Description")]
        public string Description { get; set; }

        // Track user that made change as a string
        // Foreign key to ZUser : IU
        [DisplayName("Team Member")]
        public string UserId { get; set; }

        // Store instances of Ticket and ZUser using their foregin keys above
        // Virtual helps are navigation properties and specify class relationships
        public virtual Ticket Ticket { get; set; }
        public virtual ZUser User { get; set; }


    }
}
