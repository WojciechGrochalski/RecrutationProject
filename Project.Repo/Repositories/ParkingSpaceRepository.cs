using Microsoft.EntityFrameworkCore;
using Project.Logic.Interfaces;
using Project.Logic.Models;
namespace Project.Repo.Repositories;

public class ParkingSpaceRepository : RepositoryBase<ParkingSpace>, IParkingSpaceRepository
{
    public ParkingSpaceRepository(ProjectDbContext context) : base(context)
    {
    }


    public async Task<ParkingSpace?> GetFirstFreeSpace()
    {
        return await _dbSet.Where(s => !s.IsOccupied).OrderBy(s => s.Place).FirstOrDefaultAsync();
    }


    public async Task<bool> SetPlaceFree(int placeNumber)
    {
        return await _dbSet
                 .Where(x => x.Place == placeNumber)
                 .ExecuteUpdateAsync(s => s
                     .SetProperty(u => u.IsOccupied, u => false)) > 0;
    }
}
