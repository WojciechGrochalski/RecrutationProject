
namespace Project.Logic.Commands;

public class RemoveVehicleFromParkingCommandResponse
{
    public string VehicleReg { get; set; } = null!;
    public double VehicleCharge { get; set; }
    public DateTime TimeIn { get; set; }
    public DateTime TimeOut { get; set; }
}