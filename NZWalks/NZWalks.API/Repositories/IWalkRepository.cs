using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery=null,[FromQuery]string? sortBy=null, [FromQuery] bool isAscending = true, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000);
        Task<Walk?> UpdateAsync(Guid Id, Walk walk);
        Task<Walk?> GetByIdAsync(Guid Id);
        Task<Walk?> DeleteAsync(Guid Id);
    }
}
