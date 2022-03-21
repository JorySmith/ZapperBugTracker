using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZapperBugTracker.Models
{
    public class Notification
    {
        // Id primary key
        public int Id { get; set; }

        // Ticket foreign key
        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        // Title
        [Required]
        [DisplayName("Title")]
        public string Title { get; set; }

        // Message
        [Required]
        [DisplayName("Message")]
        public string Message { get; set; }

        // Created date
        [DataType(DataType.Date)]
        [DisplayName("Date")]
        public DateTimeOffset Created { get; set; }

        // RecipientId *string GUID* foreign key
        [Required]
        [DisplayName("Recipient")]
        public string RecipientId { get; set; }

        // SenderId *string GUID* foreign key
        [Required]
        [DisplayName("Sender")]
        public string SenderId { get; set; }

        // Has been viewed
        [DisplayName("Has been viewed")]
        public bool Viewed { get; set; }

        // Navigation properties
        // Ticket, Recipient, Sender
        public virtual Ticket Ticket { get; set; }
        public virtual ZUser Recipient { get; set; }
        public virtual ZUser Sender { get; set; }

    }
}
