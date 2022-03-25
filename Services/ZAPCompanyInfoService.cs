using Microsoft.EntityFrameworkCore;
using ZapperBugTracker.Data;
using ZapperBugTracker.Models;
using ZapperBugTracker.Services.Interfaces;

namespace ZapperBugTracker.Services
{
    public class ZAPCompanyInfoService : IZAPCompanyInfoService
    {
        // Implementation of company info interface
        // Private vars to store dependency injected instance data
        private readonly ApplicationDbContext _context;

        // Dependency injection into class constructor
        public ZAPCompanyInfoService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all members for a companyId
        public async Task<List<ZUser>> GetAllMembersAsync(int companyId)
        {
            // Instantiate a new blank list of ZUsers to store user query results
            List<ZUser> result = new();

            // Store async list of users that match input companyId
            result = await _context.Users.Where(u => u.CompanyId == companyId).ToListAsync();

            return result;
        }

        // Get all projects for a companyId
        public async Task<List<Project>> GetAllProjectsAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        // Get all tickets for a companyId
        public async Task<List<Ticket>> GetAllTicketsAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        // Get company info using companyId, nullable to enable input validation checks
        public Task<Company> GetCompanyInfoById(int? companyId)
        {
            throw new NotImplementedException();
        }
    }
}
