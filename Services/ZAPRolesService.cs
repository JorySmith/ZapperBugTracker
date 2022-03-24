using Microsoft.AspNetCore.Identity;
using ZapperBugTracker.Data;
using ZapperBugTracker.Models;
using ZapperBugTracker.Services.Interfaces;

namespace ZapperBugTracker.Services
{
    public class ZAPRolesService : IZAPRolesService
    {
        // Private variables storing DI states- encapsulation
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ZUser> _userManager;

        // Constructor and dependency injection
        public ZAPRolesService(ApplicationDbContext context,
                                RoleManager<IdentityRole> roleManager,
                                UserManager<ZUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // IsUserInRoleAsync check
        public async Task<bool> IsUserInRoleAsync(ZUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        // Get list/IEnumerable of user role strings
        public async Task<IEnumerable<string>> GetUserRolesAsync(ZUser user)
        {
            throw new NotImplementedException();
        }

        // AddUserToRoleAsync, return action success/failure
        public async Task<bool> AddUserToRoleAsync(ZUser user, string roleName)
        {
            bool result = (await _userManager.AddToRoleAsync(user, roleName)).Succeeded;
            return result;
        }

        // RemoveUserFromRoleAsync, return action success/failure
        public async Task<bool> RemoveUserFromRoleAsync(ZUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        // Remove user from several roles/list of roles, return action success/failure
        public async Task<bool> RemoveUserFromRolesAsync(ZUser user, IEnumerable<string> roles)
        {
            throw new NotImplementedException();
        }

        // GetUsersInRoleAsync based on role and companyId, return list of users
        public async Task<List<ZUser>> GetUsersInRoleAsync(string roleName, int companyId)
        {
            throw new NotImplementedException();
        }

        // GetUsersNotInRoleAsync, return list of users
        public async Task<List<ZUser>> GetUsersNotInRoleAsync(string roleName, int companyId)
        {
            throw new NotImplementedException();
        }

        // Get a role name string based on its roleId
        public async Task<string> GetRoleNameByIdAsync(string roleId)
        {
            // Pass roleManager the role of type IdentityRole from DB
            // Find role in DB based on roleId
            IdentityRole role = _context.Roles.Find(roleId);
            string result = await _roleManager.GetRoleNameAsync(role);
            return result;
        }



    }
}
