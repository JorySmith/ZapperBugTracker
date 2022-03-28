using ZapperBugTracker.Models;
using ZapperBugTracker.Services.Interfaces;

namespace ZapperBugTracker.Services
{
    public class ZAPProjectService : IZAPProjectService
    {
        public Task AddNewProjectAsync(Project project)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddProjectManagerAsync(string userId, int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddUserToProjectAsync(string userId, int projectId)
        {
            throw new NotImplementedException();
        }

        public Task ArchiveProjectAsync(Project project)
        {
            throw new NotImplementedException();
        }

        public Task<List<ZUser>> GetAllProjectMembersExceptPMAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Project>> GetAllProjectsByCompany(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Project>> GetAllProjectsByPriority(int companyId, string priorityName)
        {
            throw new NotImplementedException();
        }

        public Task<List<Project>> GetArchivedProjectsByCompany(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ZUser>> GetDevelopersOnProjectAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<Project> GetProjectByIdAsync(int projectId, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<ZUser> GetProjectManagerAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ZUser>> GetProjectMembersByRoleAsync(int projectId, string role)
        {
            throw new NotImplementedException();
        }

        public Task<List<ZUser>> GetSubmittersOnProjectAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Project>> GetUserProjectsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ZUser>> GetUsersNotOnProjectAsync(int projectId, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserOnProject(string userId, int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<int> LookupProjectPriorityId(string priorityName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveProjectManagerAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveUserFromProjectAsync(string userId, int projectId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveUsersFromProjectByRoleAsync(string role, int projectId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProjectAsync(Project project)
        {
            throw new NotImplementedException();
        }
    }
}
