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
            // Pass user and role to userManager to get bool
            bool result = await _userManager.IsInRoleAsync(user, roleName);
            return result;
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
            // Pass user and role to userManager to remove user
            // Use Succeeded property to return a bool
            bool result = (await _userManager.RemoveFromRoleAsync(user, roleName)).Succeeded;
            return result;
        }

        // Remove user from several roles/list of roles, return action success/failure
        public async Task<bool> RemoveUserFromRolesAsync(ZUser user, IEnumerable<string> roles)
        {
            // Pass user and role to userManager to remove user
            // Use Succeeded property to return a bool
            bool result = (await _userManager.RemoveFromRolesAsync(user, roles)).Succeeded;
            return result;

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
            // Store list of userId(GUIDs) strings but only select Ids from userManager query
            // Use list to query DB where userId doesn't contain dbUser.Id and matches companyId
            List<string> userIds = (await _userManager.GetUsersInRoleAsync(roleName)).Select(u => u.Id).ToList();
            List<ZUser> roleUsers = _context.Users.Where(u => !userIds.Contains(u.Id)).ToList();
            List<ZUser> result = roleUsers.Where(u => u.CompanyId == companyId).ToList();
            return result;
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
