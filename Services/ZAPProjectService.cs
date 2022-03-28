using Microsoft.EntityFrameworkCore;
using ZapperBugTracker.Data;
using ZapperBugTracker.Models;
using ZapperBugTracker.Services.Interfaces;

namespace ZapperBugTracker.Services
{
    public class ZAPProjectService : IZAPProjectService
    {
        // Private vars of DI data
        private readonly ApplicationDbContext _context;

        // Constructor
        public ZAPProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Methods
        // Add new project
        public async Task AddNewProjectAsync(Project project)
        {
            _context.Add(project);
            await _context.SaveChangesAsync();
        }

        // Add project manager
        public async Task<bool> AddProjectManagerAsync(string userId, int projectId)
        {
            throw new NotImplementedException();
        }

        // Add user to project
        public async Task<bool> AddUserToProjectAsync(string userId, int projectId)
        {
            throw new NotImplementedException();
        }

        // Archive project
        public async Task ArchiveProjectAsync(Project project)
        {
            // Updated project's archived property
            project.Archived = true;

            // Update DB
            _context.Update(project);
            await _context.SaveChangesAsync();
        }

        // 
        public async Task<List<ZUser>> GetAllProjectMembersExceptPMAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Project>> GetAllProjectsByCompany(int companyId)
        {
            // Create blank list of projects to store query results
            List<Project> projects = new();

            // Query DB for specific projects and associated foreign navigation properties
            projects = await _context.Projects.Where(p => p.CompanyId == companyId && p.Archived == false)
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

            return projects;
        }

        // Get projects by company and priority name
        public async Task<List<Project>> GetAllProjectsByPriority(int companyId, string priorityName)
        {
            List<Project> projects = await GetAllProjectsByCompany(companyId);
            int priorityId = await LookupProjectPriorityId(priorityName);

            return projects.Where(p => p.ProjectPriorityId == priorityId).ToList();
        }

        // Get archived projects
        public async Task<List<Project>> GetArchivedProjectsByCompany(int companyId)
        {
            // Create blank list of projects to store query results from above method
            List<Project> projects = await GetAllProjectsByCompany(companyId);

            // Return list of projects where Archived prop is true
            return projects.Where(p => p.Archived == true).ToList();

        }

        public async Task<List<ZUser>> GetDevelopersOnProjectAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        // Get project info
        public async Task<Project> GetProjectByIdAsync(int projectId, int companyId)
        {
            // Include navigation properties linked to other tables: Tickets, Members, & Project Pri
            Project project = await _context.Projects
                                            .Include(p => p.Tickets)
                                            .Include(p => p.Members)
                                            .Include(p => p.ProjectPriority)
                                            .FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == companyId);

            return project;
        }

        public async Task<ZUser> GetProjectManagerAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ZUser>> GetProjectMembersByRoleAsync(int projectId, string role)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ZUser>> GetSubmittersOnProjectAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Project>> GetUserProjectsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ZUser>> GetUsersNotOnProjectAsync(int projectId, int companyId)
        {
            throw new NotImplementedException();
        }

        // Is user on project?
        public async Task<bool> IsUserOnProjectAsync(string userId, int projectId)
        {
            // Store project query in an instance of Project
            Project project = await _context.Projects
                                            .Include(p => p.Members)
                                            .FirstOrDefaultAsync(p => p.Id == projectId);

            bool result = false;

            // Check if any member Ids match input userId
            if (project != null)
            {
                result = project.Members.Any(m => m.Id == userId);
            }

            return result;
        }

        // Lookup project priority Id
        public async Task<int> LookupProjectPriorityId(string priorityName)
        {
            // Query DB, retrieve Id
            int priorityId = (await _context.ProjectPriorities.FirstOrDefaultAsync(p => p.Name == priorityName)).Id;

            return priorityId;
        }

        public async Task RemoveProjectManagerAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveUserFromProjectAsync(string userId, int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveUsersFromProjectByRoleAsync(string role, int projectId)
        {
            throw new NotImplementedException();
        }

        // Update project
        public async Task UpdateProjectAsync(Project project)
        {
            _context.Update(project);
            await _context.SaveChangesAsync();
        }
    }
}
