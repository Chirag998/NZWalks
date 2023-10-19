using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?> GetByIdAsync(Guid Id);
        Task<Region> CreateAsync(Region addRegion);
        Task<Region?> UpdateAsync(Guid Id,Region updateRegion);
        Task<Region?> DeleteByIdAsync(Guid Id);
    }
}
