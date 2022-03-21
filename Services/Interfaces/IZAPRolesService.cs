using ZapperBugTracker.Models;

namespace ZapperBugTracker.Services.Interfaces
{
    public interface IZAPRolesService
    {
        // IsUserInRoleAsync check, Task method is async and can return a data type
        public Task<bool> IsUserInRoleAsync(ZUser user, string roleName);

        // Get list/IEnumerable of user role strings
        public Task<IEnumerable<string>> GetUserRolesAsync(ZUser user);

        // AddUserToRoleAsync, return action success/failure
        public Task<bool> AddUserToRoleAsync(ZUser user, string roleName);

        // RemoveUserFromRoleAsync, return action success/failure
        public Task<bool> RemoveUserFromRoleAsync(ZUser user, string roleName);

        // Remove user from several roles/list of roles, return action success/failure
        public Task<bool> RemoveUserFromRolesAsync(ZUser user, IEnumerable<string> roles);

        // GetUsersInRoleAsync based on role and companyId, return list of users
        public Task<List<ZUser>> GetUsersInRoleAsync(string roleName, int companyId);

        // GetUsersNotInRoleAsync, return list of users
        public Task<List<ZUser>> GetUsersNotInRoleAsync(string roleName, int companyId);

        // Get a role name string based on its roleId
        public Task<string> GetRoleNameByIdAsync(string roleId);

    }
}
