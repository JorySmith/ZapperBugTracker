using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZapperBugTracker.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }

        // Foreign key to the ticket
        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        // Track date of attachment
        [DisplayName("File Date")]
        public DateTimeOffset Created { get; set; }

        // Track user that made attachment as a string
        // Foreign key UserId / ZUser : IU
        [DisplayName("Team Member")]
        public string UserId { get; set; }

        // File description
        [DisplayName("File Description")]
        public string Description { get; set; }

        // File attachment
        // Don't map file to DB, add DataType.Upload annotation
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile FormFile { get; set; }

        // File name
        [DisplayName("File Name")]
        public string FileName { get; set; }

        // File data
        public byte[] FileData { get; set; }

        // File extension content type
        [DisplayName("File Extension")]
        public string FileContentType { get; set; }

        // Navigation virtual properties not stored in database
        // Shows relationship between models
        public virtual Ticket Ticket { get; set; }
        public virtual ZUser User { get; set; }
    }
}
