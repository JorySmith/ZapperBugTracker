using Microsoft.EntityFrameworkCore;
using ZapperBugTracker.Data;
using ZapperBugTracker.Models;
using ZapperBugTracker.Services.Interfaces;

namespace ZapperBugTracker.Services
{
    public class ZAPCompanyInfoService : IZAPCompanyInfoService
    {
        // Implementation of company info interface
        // Private vars to store dependency injected instance data
        private readonly ApplicationDbContext _context;

        // Dependency injection into class constructor
        public ZAPCompanyInfoService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all members for a companyId
        public async Task<List<ZUser>> GetAllMembersAsync(int companyId)
        {
            // Instantiate a new blank list of ZUsers to store and return the user query results
            List<ZUser> result = new();

            // Store async list of users that match input companyId
            result = await _context.Users.Where(u => u.CompanyId == companyId).ToListAsync();

            return result;
        }

        // Get all projects for a companyId
        public async Task<List<Project>> GetAllProjectsAsync(int companyId)
        {
            // Instantiate a blank list of type project to store and return project query results
            List<Project> result = new();

            // Await DB query of projects that match input companyId, store in a list async
            // Eager load/Include navigation properties for each project:
            // Members, Tickets (then include their comments/status/pri), and ProjectPri 
            result = await _context.Projects.Where(p => p.CompanyId == companyId)
                                            .Include(p => p.Members)
                                            .Include(p => p.Tickets)
                                                .ThenInclude(t => t.Comments)
                                            .Include(p => p.Tickets)
                                                .ThenInclude(t => t.Attachments)
                                            .Include(p => p.Tickets)
                                                .ThenInclude(t => t.History)
                                            .Include(p => p.Tickets)
                                                .ThenInclude(t => t.Notifications)
                                            .Include(p => p.Tickets)
                                                .ThenInclude(t => t.DeveloperUser)
                                            .Include(p => p.Tickets)
                                                .ThenInclude(t => t.OwnerUser)
                                            .Include(p => p.Tickets)
                                                .ThenInclude(t => t.TicketStatus)
                                            .Include(p => p.Tickets)
                                                .ThenInclude(t => t.TicketPriority)
                                            .Include(p => p.Tickets)
                                                .ThenInclude(t => t.TicketType)
                                            .Include(p => p.ProjectPriority)                                            
                                            .ToListAsync();

            return result;
        }

        // Get all tickets for a companyId
        public async Task<List<Ticket>> GetAllTicketsAsync(int companyId)
        {
            // Instantiate a blank list of type Ticket to store and return ticket query results
            List<Ticket> result = new();

            // Get all company projects async first, then associated tickets
            List<Project> projects = new();

            projects = await GetAllProjectsAsync(companyId);

            // SelectMany tickets from each project, save to list
            result = projects.SelectMany(p => p.Tickets).ToList();

            return result;
        }

        // Get company info using companyId, nullable to enable input validation checks
        public async Task<Company> GetCompanyInfoByIdAsync(int? companyId)
        {
            // Instantiate a blank type of Company to store resulting company info
            Company result = new();

            if (companyId != null)
            {
                // Find first or default company in DB, include 
                result = await _context.Companies
                    .Include(c => c.Members)
                    .FirstOrDefaultAsync(c => c.Id == companyId);
            }

            return result;
        }
    }
}
