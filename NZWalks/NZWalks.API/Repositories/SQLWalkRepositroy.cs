using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepositroy : IWalkRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;

        public SQLWalkRepositroy(NZWalksDbContext nZWalksDbContext)
        {
            _nZWalksDbContext = nZWalksDbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _nZWalksDbContext.Walks.AddAsync(walk);
            await _nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid Id)
        {
            var walkToBeDeleted = await _nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == Id);
            if (walkToBeDeleted == null)
            {
                return null;
            }
            _nZWalksDbContext.Walks.Remove(walkToBeDeleted);
            await _nZWalksDbContext.SaveChangesAsync();
            return walkToBeDeleted;
        }

        public async Task<List<Walk>> GetAllAsync([FromQuery] string? filterOn = null, [FromQuery] string? filterQuery = null,
            [FromQuery] string? sortBy = null, [FromQuery] bool isAscending = true, [FromQuery] int pageNumber=1, [FromQuery]int pageSize=1000)
        {
            var walks = _nZWalksDbContext.Walks.Include("Region").Include("Difficulty").AsQueryable();
            //Filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
                
            }

            //sorting

            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length",StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            //paging

            var skipResults = (pageNumber - 1) * pageSize;
            walks = walks.Skip(skipResults).Take(pageSize);
            return await walks.ToListAsync();
            //return await _nZWalksDbContext.Walks.Include("Region").Include("Difficulty").ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid Id)
        {
            var walkDomainModel = await _nZWalksDbContext.Walks.Include("Region").Include("Difficulty").FirstOrDefaultAsync(x => x.Id == Id);
            if (walkDomainModel == null) 
            {
                return null;
            }
            return walkDomainModel;
            throw new NotImplementedException();
        }

        public async Task<Walk?> UpdateAsync(Guid Id,Walk walk)
        {
            var walkNeedToBeUpdate = await _nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == Id);
            if(walkNeedToBeUpdate == null) 
            {
                return null;
            }
            walkNeedToBeUpdate.Description = walk.Description;
            walkNeedToBeUpdate.Name = walk.Name;
            walkNeedToBeUpdate.LengthInKm= walk.LengthInKm;
            walkNeedToBeUpdate.DifficultyId = walk.DifficultyId;
            walkNeedToBeUpdate.RegionId = walk.RegionId;
            _nZWalksDbContext.Walks.Update(walkNeedToBeUpdate);
            await _nZWalksDbContext.SaveChangesAsync();
            return walkNeedToBeUpdate;
        }
    }
}
