using Microsoft.EntityFrameworkCore;
using ZapperBugTracker.Data;
using ZapperBugTracker.Models;
using ZapperBugTracker.Models.Enums;
using ZapperBugTracker.Services.Interfaces;

namespace ZapperBugTracker.Services
{
    public class ZAPTicketService : IZAPTicketService
    {
        private readonly ApplicationDbContext _context;
        private readonly IZAPRolesService _rolesService;
        private readonly IZAPProjectService _projectService;

        public ZAPTicketService(ApplicationDbContext context, 
                                 IZAPRolesService rolesService,
                                 IZAPProjectService projectService)
        {
            _context = context;
            _rolesService = rolesService;
            _projectService = projectService;
        }

        // All async tasks because generally querying the context database, await results
        public async Task AddNewTicketAsync(Ticket ticket)
        {
            _context.Add(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task ArchiveTicketAsync(Ticket ticket)
        {
            ticket.Archived = true;
            _context.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public Task AssignTicketAsync(int ticketId, string userId)
        {
            throw new NotImplementedException();
        }

        // Tickets are assigned to Projects, Projects to Companies
        public async Task<List<Ticket>> GetAllTicketsByCompanyAsync(int companyId)
        {
            try
            {
                List<Ticket> tickets = await _context.Projects
                                                     .Where(p => p.CompanyId == companyId)
                                                     .SelectMany(p => p.Tickets) // Select many/all tickets and their local props
                                                        .Include(t => t.Attachments) // Add ticket info from foreign tables
                                                        .Include(t => t.Comments)
                                                        .Include(t => t.DeveloperUser)
                                                        .Include(t => t.History)
                                                        .Include(t => t.OwnerUser)
                                                        .Include(t => t.TicketPriority)
                                                        .Include(t => t.Comments)
                                                        .Include(t => t.TicketStatus)
                                                        .Include(t => t.TicketType)
                                                        .Include(t => t.Project)
                                                     .ToListAsync(); // Save results to a list async
                return tickets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        // Tickets are assigned to TicketPriority (IDs) not string pri names, so LookupTicketPriorityIdAsync()
        // Still Linq search through Projects because it contains Company and Tickets collections
        public async Task<List<Ticket>> GetAllTicketsByPriorityAsync(int companyId, string priorityName)
        {
            int priorityId = (await LookupTicketPriorityIdAsync(priorityName)).Value; // Since return type is int?, get .Value

            try
            {
                List<Ticket> tickets = await _context.Projects
                                                     .Where(p => p.CompanyId == companyId)
                                                     .SelectMany(p => p.Tickets) // Select many/all tickets and their local props
                                                        .Include(t => t.Attachments) // Add ticket info from foreign tables
                                                        .Include(t => t.Comments)
                                                        .Include(t => t.DeveloperUser)
                                                        .Include(t => t.History)
                                                        .Include(t => t.OwnerUser)
                                                        .Include(t => t.TicketPriority)
                                                        .Include(t => t.Comments)
                                                        .Include(t => t.TicketStatus)
                                                        .Include(t => t.TicketType)
                                                        .Include(t => t.Project)
                                                     .Where(t => t.TicketPriorityId == priorityId) // Where tickets match priorityId
                                                     .ToListAsync(); // Save results to a list async
                return tickets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByStatusAsync(int companyId, string statusName)
        {
            // Convert string statusName to statusId, use Id for Linq query
            int statusId = (await LookupTicketStatusIdAsync(statusName)).Value;

            try
            {
                List<Ticket> tickets = await _context.Projects
                                                     .Where(p => p.CompanyId == companyId)
                                                     .SelectMany(p => p.Tickets) // Select many/all tickets and their local props
                                                        .Include(t => t.Attachments) // Add ticket info from foreign tables
                                                        .Include(t => t.Comments)
                                                        .Include(t => t.DeveloperUser)
                                                        .Include(t => t.History)
                                                        .Include(t => t.OwnerUser)
                                                        .Include(t => t.TicketPriority)
                                                        .Include(t => t.Comments)
                                                        .Include(t => t.TicketStatus)
                                                        .Include(t => t.TicketType)
                                                        .Include(t => t.Project)
                                                     .Where(t => t.TicketStatusId == statusId) // Where tickets match statusId
                                                     .ToListAsync(); // Save results to a list async
                return tickets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByTypeAsync(int companyId, string typeName)
        {
            int typeId = (await LookupTicketTypeIdAsync(typeName)).Value;

            try
            {
                List<Ticket> tickets = await _context.Projects
                                                     .Where(p => p.CompanyId == companyId)
                                                     .SelectMany(p => p.Tickets) // Select many/all tickets and their local props
                                                        .Include(t => t.Attachments) // Add ticket info from foreign tables
                                                        .Include(t => t.Comments)
                                                        .Include(t => t.DeveloperUser)
                                                        .Include(t => t.History)
                                                        .Include(t => t.OwnerUser)
                                                        .Include(t => t.TicketPriority)
                                                        .Include(t => t.Comments)
                                                        .Include(t => t.TicketStatus)
                                                        .Include(t => t.TicketType)
                                                        .Include(t => t.Project)
                                                     .Where(t => t.TicketTypeId == typeId) // Where tickets match statusId
                                                     .ToListAsync(); // Save results to a list async
                return tickets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<Ticket>> GetArchivedTicketsAsync(int companyId)
        {
            // Get archived tickets for one company            
            try
            {
                List<Ticket> tickets = (await GetAllTicketsByCompanyAsync(companyId)).Where(t => t.Archived == true).ToList();

                return tickets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public Task<List<Ticket>> GetProjectTicketsByPriorityAsync(string priorityName, int companyId, int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetProjectTicketsByRoleAsync(string role, string userId, int projectId, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetProjectTicketsByStatusAsync(string statusName, int companyId, int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetProjectTicketsByTypeAsync(string typeName, int companyId, int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            // Return the first matching ticket or an empty default Ticket ticket
            return await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
        }

        public Task<ZUser> GetTicketDeveloperAsync(int ticketId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Ticket>> GetTicketsByRoleAsync(string role, string userId, int companyId)
        {

            List<Ticket> tickets = new();
            
            try
            {
                if(role == Roles.Admin.ToString())
                {
                    tickets = await GetAllTicketsByCompanyAsync(companyId);
                }
                else if (role == Roles.Developer.ToString())
                {
                    tickets = (await GetAllTicketsByCompanyAsync(companyId)).Where(t => t.DeveloperUserId == userId).ToList();
                }
                else if (role == Roles.Submitter.ToString())
                {
                    tickets = (await GetAllTicketsByCompanyAsync(companyId)).Where(t => t.OwnerUserId == userId).ToList();
                }
                else if(role == Roles.ProjectManager.ToString())
                {
                    tickets = await GetTicketsByUserIdAsync(userId, companyId);
                }

                return tickets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<Ticket>> GetTicketsByUserIdAsync(string userId, int companyId)
        {
            ZUser zUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            List<Ticket> tickets = new();

            try
            {
                if (await _rolesService.IsUserInRoleAsync(zUser, Roles.Admin.ToString()))
                {
                    tickets = (await _projectService.GetAllProjectsByCompany(companyId))
                                                    .SelectMany(p => p.Tickets).ToList();
                }
                else if (await _rolesService.IsUserInRoleAsync(zUser, Roles.Developer.ToString()))
                {
                    tickets = (await _projectService.GetAllProjectsByCompany(companyId))
                                                    .SelectMany(p => p.Tickets)
                                                    .Where(t => t.DeveloperUserId == userId).ToList();
                }
                else if (await _rolesService.IsUserInRoleAsync(zUser, Roles.Submitter.ToString()))
                {
                    tickets = (await _projectService.GetAllProjectsByCompany(companyId))
                                                    .SelectMany(p => p.Tickets)
                                                    .Where(t => t.OwnerUserId == userId).ToList();
                }
                else if (await _rolesService.IsUserInRoleAsync(zUser, Roles.ProjectManager.ToString()))
                {
                    tickets = (await _projectService.GetUserProjectsAsync(userId))
                                                    .SelectMany(t => t.Tickets).ToList();
                }

                return tickets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        // Nullable int? because it may not return an int
        public async Task<int?> LookupTicketPriorityIdAsync(string priorityName)
        {
            try
            {
                TicketPriority priority = await _context.TicketPriorities.FirstOrDefaultAsync(p => p.Name == priorityName);
                return priority?.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<int?> LookupTicketStatusIdAsync(string statusName)
        {
            try
            {
                TicketStatus status = await _context.TicketStatuses.FirstOrDefaultAsync(s => s.Name == statusName);
                return status?.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<int?> LookupTicketTypeIdAsync(string typeName)
        {
            try
            {
                TicketType type = await _context.TicketTypes.FirstOrDefaultAsync(t => t.Name == typeName);
                return type?.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            _context.Add(ticket);
            await _context.SaveChangesAsync();
        }
    }
}
