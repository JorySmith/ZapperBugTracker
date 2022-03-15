using System.ComponentModel;

namespace ZapperBugTracker.Models
{
    public class Invite
    {
        // Id primary key
        public int Id { get; set; }

        // Date using DTO, which uses standardized UTC time zone
        [DisplayName("Date Sent")]
        public DateTimeOffset InviteDate { get; set; }

        // Join date when user accepts invite
        [DisplayName("Join Date")]
        public DateTimeOffset JoinDate { get; set; }

        // Company token GUID - Globally Unique Identifier
        [DisplayName("Code")]
        public Guid CompanyToken { get; set; }

        // CompanyId foreign key
        [DisplayName("Company")]
        public int CompanyId { get; set; }

        // ProjectId foreign key
        [DisplayName("Project")]
        public int ProjectId { get; set; }

        // InvitorId *string* ZUser foreign key
        [DisplayName("Invitor")]
        public string InvitorId { get; set; }

        // InviteeId *string* ZUser foreign key
        [DisplayName("Invitee")]
        public string InviteeId { get; set; }

        // Invitee email
        [DisplayName("Invitee Email")]
        public string InviteeEmail { get; set; }

        // Invitee first name
        [DisplayName("Invitee First Name")]
        public string InviteeFirstName { get; set; }

        // Invitee last name
        [DisplayName("Invitee Last Name")]
        public string InviteeLastName { get; set; }

        // IsValid flag, invite valid still or not depending on days from invite
        public bool IsValid { get; set; }

        // Navigation properties
        // Company, Project, Invitor, Invitee
        public virtual Company Company { get; set; }
        public virtual Project Project { get; set; }
        public ZUser Invitor { get; set; }
        public ZUser Invitee { get; set; }
    }
}
