using Project.Logic.Models;

namespace Project.Logic.Interfaces;

public interface IParkingSpaceRepository : IRrepositoryBase<ParkingSpace>
{
    public Task<ParkingSpace?> GetFirstFreeSpace();
    public Task<bool> SetPlaceFree(int placeNumber);
}
