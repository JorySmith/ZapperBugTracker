using Microsoft.EntityFrameworkCore;
using System.Data;
using ZapperBugTracker.Data;
using ZapperBugTracker.Models;
using ZapperBugTracker.Models.Enums;
using ZapperBugTracker.Services.Interfaces;

namespace ZapperBugTracker.Services
{
    public class ZAPTicketService : IZAPTicketService
    {
        private readonly ApplicationDbContext context;
        private readonly IZAPRolesService rolesService;
        private readonly IZAPProjectService projectService;

        public ZAPTicketService(ApplicationDbContext context, 
                                 IZAPRolesService rolesService,
                                 IZAPProjectService projectService)
        {
            this.context = context;
            this.rolesService = rolesService;
            this.projectService = projectService;
        }

        // All async tasks because generally querying the context database, await results
        public async Task AddNewTicketAsync(Ticket ticket)
        {
            try
            {
                context.Add(ticket);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }

        public async Task ArchiveTicketAsync(Ticket ticket)
        {
            try
            {
                ticket.Archived = true;
                context.Update(ticket);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }

        public async Task AssignTicketAsync(int ticketId, string userId)
        {
            Ticket ticket = await context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);

            try
            {
                if (ticket != null)
                {
                    // Assign userId to the ticket
                    ticket.DeveloperUserId = userId;

                    // Update ticket status id via lookup method
                    ticket.TicketStatusId = (await LookupTicketStatusIdAsync("Development")).Value;

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        // Tickets are assigned to Projects, Projects to Companies
        public async Task<List<Ticket>> GetAllTicketsByCompanyAsync(int companyId)
        {
            try
            {
                List<Ticket> tickets = await context.Projects
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
                throw new Exception(ex.Message);
            }
        }

        // Tickets are assigned to TicketPriority (IDs) not string pri names, so LookupTicketPriorityIdAsync()
        // Still Linq search through Projects because it contains Company and Tickets collections
        public async Task<List<Ticket>> GetAllTicketsByPriorityAsync(int companyId, string priorityName)
        {
            int priorityId = (await LookupTicketPriorityIdAsync(priorityName)).Value; // Since return type is int?, get .Value

            try
            {
                List<Ticket> tickets = await context.Projects
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
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByStatusAsync(int companyId, string statusName)
        {
            // Convert string statusName to statusId, use Id for Linq query
            int statusId = (await LookupTicketStatusIdAsync(statusName)).Value;

            try
            {
                List<Ticket> tickets = await context.Projects
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
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByTypeAsync(int companyId, string typeName)
        {
            int typeId = (await LookupTicketTypeIdAsync(typeName)).Value;

            try
            {
                List<Ticket> tickets = await context.Projects
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
                throw new Exception(ex.Message);
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
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Ticket>> GetProjectTicketsByPriorityAsync(string priorityName, int companyId, int projectId)
        {
            List<Ticket> tickets = new();

            try
            {
                tickets = (await GetAllTicketsByPriorityAsync(companyId, priorityName)).Where(t => t.ProjectId == projectId).ToList();

                return tickets;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Ticket>> GetProjectTicketsByRoleAsync(string role, string userId, int projectId, int companyId)
        {
            List<Ticket> tickets = new();

            try
            {
                tickets = (await GetTicketsByRoleAsync(role, userId, companyId)).Where(t => t.ProjectId == projectId).ToList();

                return tickets;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Ticket>> GetProjectTicketsByStatusAsync(string statusName, int companyId, int projectId)
        {
            List<Ticket> tickets = new();

            try
            {
                tickets = (await GetAllTicketsByStatusAsync(companyId, statusName)).Where(t => t.ProjectId == projectId).ToList();

                return tickets;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Ticket>> GetProjectTicketsByTypeAsync(string typeName, int companyId, int projectId)
        {
            List<Ticket> tickets = new();

            try
            {
                tickets = (await GetAllTicketsByTypeAsync(companyId, typeName)).Where(t => t.ProjectId == projectId).ToList();

                return tickets;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            try
            {
                // Return the first matching ticket or an empty default Ticket ticket
                return await context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }

        public async Task<ZUser> GetTicketDeveloperAsync(int ticketId, int companyId)
        {
            ZUser developer = new();

            try
            {
                Ticket ticket = (await GetAllTicketsByCompanyAsync(companyId)).FirstOrDefault(t => t.Id == ticketId);

                if(ticket?.DeveloperUserId != null)
                {
                    developer = ticket.DeveloperUser;
                }

                return developer;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);                
            }            
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
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Ticket>> GetTicketsByUserIdAsync(string userId, int companyId)
        {
            ZUser zUser = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            List<Ticket> tickets = new();

            try
            {
                if (await rolesService.IsUserInRoleAsync(zUser, Roles.Admin.ToString()))
                {
                    tickets = (await projectService.GetAllProjectsByCompany(companyId))
                                                    .SelectMany(p => p.Tickets).ToList();
                }
                else if (await rolesService.IsUserInRoleAsync(zUser, Roles.Developer.ToString()))
                {
                    tickets = (await projectService.GetAllProjectsByCompany(companyId))
                                                    .SelectMany(p => p.Tickets)
                                                    .Where(t => t.DeveloperUserId == userId).ToList();
                }
                else if (await rolesService.IsUserInRoleAsync(zUser, Roles.Submitter.ToString()))
                {
                    tickets = (await projectService.GetAllProjectsByCompany(companyId))
                                                    .SelectMany(p => p.Tickets)
                                                    .Where(t => t.OwnerUserId == userId).ToList();
                }
                else if (await rolesService.IsUserInRoleAsync(zUser, Roles.ProjectManager.ToString()))
                {
                    tickets = (await projectService.GetUserProjectsAsync(userId))
                                                    .SelectMany(t => t.Tickets).ToList();
                }

                return tickets;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Nullable int? because it may not return an int
        public async Task<int?> LookupTicketPriorityIdAsync(string priorityName)
        {
            try
            {
                TicketPriority priority = await context.TicketPriorities.FirstOrDefaultAsync(p => p.Name == priorityName);
                return priority?.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int?> LookupTicketStatusIdAsync(string statusName)
        {
            try
            {
                TicketStatus status = await context.TicketStatuses.FirstOrDefaultAsync(s => s.Name == statusName);
                return status?.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);                
            }
        }

        public async Task<int?> LookupTicketTypeIdAsync(string typeName)
        {
            try
            {
                TicketType type = await context.TicketTypes.FirstOrDefaultAsync(t => t.Name == typeName);
                return type?.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            try
            {
                context.Add(ticket);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }
    }
}
