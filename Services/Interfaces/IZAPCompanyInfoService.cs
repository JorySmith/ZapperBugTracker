using ZapperBugTracker.Models;

namespace ZapperBugTracker.Services.Interfaces
{
    public interface IZAPCompanyInfoService
    {
        // Company CRUD operations 
        // Get company info using companyId, nullable to enable input validation checks
        public Task<Company> GetCompanyInfoByIdAsync(int? companyId);

        // Get all members for a companyId
        public Task<List<ZUser>> GetAllMembersAsync(int companyId);

        // Get all projects for a companyId
        public Task<List<Project>> GetAllProjectsAsync(int companyId);

        // Get all tickets for a companyId
        public Task<List<Ticket>> GetAllTicketsAsync(int companyId);
    }
}
