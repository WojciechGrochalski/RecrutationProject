using Project.Logic.Enums;
namespace Project.Logic.Models;

public class ParkedVehicle: BaseModel
{
    public string VehicleReg { get; set; } = null!;
    public VehicleType Type { get; set; }
    public int SpaceNumber { get; set; }
    public DateTime TimeIn { get; set; }
}
