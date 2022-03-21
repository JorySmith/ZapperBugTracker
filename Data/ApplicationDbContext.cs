using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZapperBugTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ZapperBugTracker.Data
{
  // Add ZUser type context to IdentityDbContext
  public class ApplicationDbContext : IdentityDbContext<ZUser>
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
        // Map model classes to DB, enable CRUD operations with DB
        // DBSet of each model class except ZUser
        public DbSet<Company> Companies { get; set; }
        public DbSet<Invite> Invites { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ProjectPriority> ProjectPriorities { get; set; }
        public DbSet<TicketAttachment> TicketAttachments { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
        public DbSet<TicketHistory> TicketHistories { get; set; }
        public DbSet<TicketPriority> TicketPriorities { get; set;}
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }

    }
}