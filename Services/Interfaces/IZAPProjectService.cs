using ZapperBugTracker.Models;

namespace ZapperBugTracker.Services.Interfaces
{
    public interface IZAPProjectService
    {
        // Tasks that handle all project related CRUD functionality 
        // Add new project
        public Task AddNewProjectAsync(Project project);

        // Add project manager, return bool
        public Task<bool> AddProjectManagerAsync(string userId, int projectId);

        // Add user to project, return bool
        public Task<bool> AddUserToProjectAsync(string userId, int projectId);

        // Archive project
        public Task ArchiveProjectAsync(Project project);

        // Get list of all projects by companyId
        public Task<List<Project>> GetAllProjectsByCompany(int companyId);

        // Get list of all projects by companyId and priority name
        public Task<List<Project>> GetAllProjectsByPriority(int companyId, string priorityName);

        // Get list of all project members except PMs by projectId
        public Task<List<ZUser>> GetAllProjectMembersExceptPMAsync(int projectId);

        // Get list of archived projects by companyId
        public Task<List<Project>> GetArchivedProjectsByCompany(int companyId);

        // Get list of project developers by projectId
        public Task<List<ZUser>> GetDevelopersOnProjectAsync(int projectId);

        // Get project manager by projectId
        public Task<ZUser> GetProjectManagerAsync(int projectId);

        // Get list of project members by projectId and role 
        public Task<List<ZUser>> GetProjectMembersByRoleAsync(int projectId, string role);

        // Get project info by projectId and companyId
        public Task<Project> GetProjectByIdAsync(int projectId, int companyId);

        // Get list of project submitters by projectId
        public Task<List<ZUser>> GetSubmittersOnProjectAsync(int projectId);

        // Get list of users not working on project by projectId and companyId
        public Task<List<ZUser>> GetUsersNotOnProjectAsync(int projectId, int companyId);

        // Get user's list of assigned projects
        public Task<List<Project>> GetUserProjectsAsync(string userId);

        // Is user working on a projectId?, return bool
        public Task<bool> IsUserOnProject(string userId, int projectId);

        // Lookup project priority Id based on priority name
        public Task<int> LookupProjectPriorityId(string priorityName);

        // Remove project manager based on projectId
        public Task RemoveProjectManagerAsync(int projectId);

        // Remove users from project based on role and projectId
        public Task RemoveUsersFromProjectByRoleAsync(string role, int projectId);

        // Remove one user from project based on userId and projectId
        public Task RemoveUserFromProjectAsync(string userId, int projectId);

        // Update project
        public Task UpdateProjectAsync(Project project);


    }
}
