using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZapperBugTracker.Models
{
    public class Project
    {
        // Id primary key
        public int Id { get; set; }

        // CompanyId nullable foreign key
        public int? CompanyId { get; set; }

        // Name, length limited to 50 chars
        [Required]
        [StringLength(50)]
        [DisplayName("Project Name")]
        public string Name { get; set; }

        // Description
        [DisplayName("Description")]
        public string Description { get; set; }

        // Start date
        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateTimeOffset StartDate { get; set; }

        // End date
        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        public DateTimeOffset EndDate { get; set; }

        // Project Priority Id, nullable
        [DisplayName("Priority")]
        public int ProjectProjectId { get; set; }

        // Upload file description
        //[DisplayName("File Description")]
        //public string Description { get; set; }

        // Image IFormFile upload, don't map it to db
        // DataType.DataUpload
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile ImageFormFile { get; set; }

        // Image DB storage schema
        // Name, byte[] data, content type/extension
        public string ImageFileName { get; set; }
        public byte[] ImageFileData { get; set; }

        public string ImageContentType { get; set; }

        // Archived
        [DisplayName("Archived")]
        public bool Archived { get; set; }

        // Navigation virtual property references
        // Company
        public virtual Company Company { get; set; }

        // Project priority
        public virtual ProjectPriority ProjectPriority { get; set;}

        // Members - ZUsers
        public ICollection<ZUser> Members { get; set;} = new HashSet<ZUser>();

        // Tickets
        public ICollection<Ticket> Tickets { get; set;} = new HashSet<Ticket>();
    }
}
