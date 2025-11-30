using Project.Logic.Models;

namespace Project.Logic.Interfaces;

public interface IParkedVehicleRepository : IRrepositoryBase<ParkedVehicle>
{
    public Task<ParkedVehicle?> GetByVehicleReg(string vehicleReg);
}
