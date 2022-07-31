using Microsoft.EntityFrameworkCore;
using ZapperBugTracker.Data;
using ZapperBugTracker.Models;
using ZapperBugTracker.Models.Enums;
using ZapperBugTracker.Services.Interfaces;

namespace ZapperBugTracker.Services
{
    public class ZAPProjectService : IZAPProjectService
    {
        // Private vars of DI data
        private readonly ApplicationDbContext _context;
        private readonly IZAPRolesService _roleService;

        // Constructor
        public ZAPProjectService(ApplicationDbContext context, IZAPRolesService roleService)
        {
            _context = context;
            _roleService = roleService;
        }

        // Methods
        // Add new project
        public async Task AddNewProjectAsync(Project project)
        {
            _context.Add(project);
            await _context.SaveChangesAsync();
        }

        // Add project manager to project
        public async Task<bool> AddProjectManagerAsync(string userId, int projectId)
        {
            // Get current PM from project
            ZUser currentPM = await GetProjectManagerAsync(projectId);

            // If project already has a PM, remove that PM
            if (currentPM != null)
            {
                try
                {
                    await RemoveProjectManagerAsync(projectId);
                }
                catch (Exception ex)
                {
                    // If error, write ex.Message to console and return false
                    Console.WriteLine($"Error removing PM from project -- {ex.Message}");
                    return false;
                }
            }
            // Add new PM to project
            try
            {
                await AddProjectManagerAsync(userId, projectId);
                return true;
            }
            catch (Exception ex)
            {
                // If error, write ex.Message to console and return false
                Console.WriteLine($"Error adding PM to project -- {ex.Message}");
                return false;
            }

        }

        // Add user to project
        public async Task<bool> AddUserToProjectAsync(string userId, int projectId)
        {
            // Store input user in a blank ZUser instance, use to add user to project members
            ZUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                // Store project from DB in a Project instance
                // If user isn't already in project, then add user
                Project project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
                
                
                if (!await IsUserOnProjectAsync(userId, projectId))
                {
                    try
                    {
                        project.Members.Add(user);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
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

        // Get project members except the PM
        public async Task<List<ZUser>> GetAllProjectMembersExceptPMAsync(int projectId)
        {
            // Store list of developers, submitters, and admins
            List<ZUser> developers = await GetProjectMembersByRoleAsync(projectId, Roles.Developer.ToString());
            List<ZUser> submitters = await GetProjectMembersByRoleAsync(projectId, Roles.Submitter.ToString());
            List<ZUser> admins = await GetProjectMembersByRoleAsync(projectId, Roles.Admin.ToString());

            // Create a new list to concat and store all 3 lists from above
            List<ZUser> teamMembers = developers.Concat(submitters).Concat(admins).ToList();

            return teamMembers;
        }

        // Get all projects for a company
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

        // Get all projects by company and priority name
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

        // 
        public async Task<List<ZUser>> GetDevelopersOnProjectAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        // Get a project by ID and companyId
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

        // Get PM by projectId
        public async Task<ZUser> GetProjectManagerAsync(int projectId)
        {
            // Create instance of project, get project from DB, include associated Members
            Project project = await _context.Projects
                                            .Include(p => p.Members)
                                            .FirstOrDefaultAsync(p => p.Id == projectId);

            // For each ZUser in project.Members (if project exists), check if member is a PM
            foreach (ZUser member in project?.Members)
            {
                // User roleService to check member's role
                if (await _roleService.IsUserInRoleAsync(member, Roles.ProjectManager.ToString()))
                {
                    return member;
                }
            }
            // If no results, return null
            return null;
        }

        // Get project members by role
        public async Task<List<ZUser>> GetProjectMembersByRoleAsync(int projectId, string role)
        {
            // Find project in Projects table, include Members in query
            // Store results in Project project to access members 
            Project project = await _context.Projects
                                            .Include(p => p.Members)
                                            .FirstOrDefaultAsync(p => p.Id == projectId);
            // Create an instance of a list of ZUsers called members to store memebers
            List<ZUser> members = new();

            // Loop through project.Members, use roleService to check user role
            // Add to members if role matches input role
            foreach (var user in project.Members)
            {
                if (await _roleService.IsUserInRoleAsync(user, role))
                {
                    members.Add(user);
                }
            }
            return members;
        }

        // 
        public async Task<List<ZUser>> GetSubmittersOnProjectAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        // Get user's projects
        public async Task<List<Project>> GetUserProjectsAsync(string userId)
        {
            
            try
            {
                // Find and store list of user's projects from DB and include desired properties
                List<Project> userProjects = (await _context.Users
                                                               .Include(u => u.Projects)
                                                                   .ThenInclude(p => p.Company)
                                                               .Include(u => u.Projects)
                                                                   .ThenInclude(p => p.Members)
                                                               .Include(u => u.Projects)
                                                                   .ThenInclude(p => p.Tickets)
                                                               .Include(u => u.Projects)
                                                                   .ThenInclude(t => t.Tickets)
                                                                        .ThenInclude(t => t.DeveloperUser)
                                                                .Include(u => u.Projects)
                                                                   .ThenInclude(t => t.Tickets)
                                                                        .ThenInclude(t => t.OwnerUser)
                                                                .Include(u => u.Projects)
                                                                   .ThenInclude(t => t.Tickets)
                                                                        .ThenInclude(t => t.TicketPriority)
                                                                .Include(u => u.Projects)
                                                                   .ThenInclude(t => t.Tickets)
                                                                        .ThenInclude(t => t.TicketStatus)
                                                                .Include(u => u.Projects)
                                                                   .ThenInclude(t => t.Tickets)
                                                                        .ThenInclude(t => t.TicketType)
                                                               .FirstOrDefaultAsync(u => u.Id == userId)).Projects.ToList();

                return userProjects;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"**ERROR** Getting user's projects - {ex.Message}");
                throw;
            }
        }

        // Get user's not on a project
        public async Task<List<ZUser>> GetUsersNotOnProjectAsync(int projectId, int companyId)
        {
            // Store list of users if their projects don't match the projectId input
            List<ZUser> users = await _context.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToListAsync();

            // Return only users that are part of companyId
            return users.Where(u => u.CompanyId == companyId).ToList();
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

        // Remove PM
        public async Task RemoveProjectManagerAsync(int projectId)
        {
            // Get project and store it, include Members
            Project project = await _context.Projects
                                            .Include(p => p.Members)
                                            .FirstOrDefaultAsync(p => p.Id == projectId);

            // Try catch block for role check
            try
            {
                // For each ZUser member in project.Members (if project exists), see if member is a PM
                foreach (ZUser member in project?.Members)
                {
                    if (await _roleService.IsUserInRoleAsync(member, Roles.ProjectManager.ToString()))
                    {
                        // If member is a PM, remove member
                        await RemoveUserFromProjectAsync(member.Id, projectId);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        // Remove user from project by userId
        public async Task RemoveUserFromProjectAsync(string userId, int projectId)
        {
            try
            {
                // Find and store user to pass later to Project
                // Find and store project to receive user to be removed
                ZUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                Project project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

                try
                {
                    // If user on project, remove user
                    if (await IsUserOnProjectAsync(userId, project.Id))
                    {
                        project.Members.Remove(user);

                        // Save changes to DB async
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception)
                {
                    throw;
                }      
            }
            catch (Exception ex)
            {
                Console.WriteLine($"****ERROR**** - Removing User from Project - {ex.Message}");
            }
        }

        // Remove user from project by user role
        public async Task RemoveUsersFromProjectByRoleAsync(string role, int projectId)
        {
            // Try catch block
            try
            {
                // Store list of ZUsers in a role, use GetProjectMembersByRoleAsync to filter members
                List<ZUser> members = await GetProjectMembersByRoleAsync(projectId, role);

                // Create instance of Project project to store filtered members from DB, return them
                Project project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

                // Loop through members, remove member/user from project 
                // Save changes to DB
                foreach (var user in members)
                {
                    try
                    {
                        project.Members.Remove(user);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }

            }
            catch (Exception ex)
            {
                // Write custom error message to console/stack trace and throw exception
                Console.WriteLine($"** ERROR ** Removing users from project --> {ex.Message}");
                throw;
            }
        }

        // Update project
        public async Task UpdateProjectAsync(Project project)
        {
            _context.Update(project);
            await _context.SaveChangesAsync();
        }
    }
}
