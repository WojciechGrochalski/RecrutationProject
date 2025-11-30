using Microsoft.EntityFrameworkCore;
using Project.Logic.Interfaces;
using Project.Logic.Models;

namespace Project.Repo.Repositories;

public class ParkedVehicleRepository : RepositoryBase<ParkedVehicle>, IParkedVehicleRepository
{
    public ParkedVehicleRepository(ProjectDbContext context) : base(context)
    {
    }

    public async Task<ParkedVehicle?> GetByVehicleReg(string vehicleReg)
    {
        return await _dbSet.FirstOrDefaultAsync(v => v.VehicleReg == vehicleReg);
    }
}
