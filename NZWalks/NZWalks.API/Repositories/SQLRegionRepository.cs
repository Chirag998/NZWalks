using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;

        public SQLRegionRepository(NZWalksDbContext nZWalksDbContext)
        {
            _nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Region> CreateAsync(Region regionModel)
        {
            await _nZWalksDbContext.Regions.AddAsync(regionModel);
            await _nZWalksDbContext.SaveChangesAsync();
            return regionModel;
        }

        public async Task<Region?> DeleteByIdAsync(Guid Id)
        {
            var regionNeedToBeDeleted = await _nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);
            if (regionNeedToBeDeleted == null)
            {
                return null;
            }
            _nZWalksDbContext.Regions.Remove(regionNeedToBeDeleted);
            await _nZWalksDbContext.SaveChangesAsync();
            return regionNeedToBeDeleted;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await _nZWalksDbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid Id)
        {
            var regionModel = await _nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);
            if (regionModel == null)
            {
                return null;
            }
            return regionModel;
        }

        public async Task<Region?> UpdateAsync(Guid Id, Region updateRegion)
        {
            var regionNeedToBeUpdate = await _nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);
            if (regionNeedToBeUpdate == null)
            {
                return null;
            }
            regionNeedToBeUpdate.Name = updateRegion.Name;
            regionNeedToBeUpdate.Code = updateRegion.Code;
            regionNeedToBeUpdate.RegionImageUrl = updateRegion.RegionImageUrl;

            await _nZWalksDbContext.SaveChangesAsync();
            return regionNeedToBeUpdate;
        }
    }
}
