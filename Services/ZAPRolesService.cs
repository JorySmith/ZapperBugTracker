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

        // Get list/IEnumerable of all roles (strings) assigned to a user 
        public async Task<IEnumerable<string>> GetUserRolesAsync(ZUser user)
        {
            // Pass in user to userManager to get their roles
            IEnumerable<string> result = await _userManager.GetRolesAsync(user);
            return result;
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
            // Store List of ZUsers based on roleName passed into userManager.GetUsersinRoleAsync
            // Filter list where user's companyId matches passed in Id
            List<ZUser> users = (await _userManager.GetUsersInRoleAsync(roleName)).ToList();
            List<ZUser> result = users.Where(u => u.CompanyId == companyId).ToList();
            return result;

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
