namespace Project.Logic.Commands;

public class AddVehicleToParkingCommandResponse
{
    public string VehicleReg { get; set; } = null!;
    public int SpaceNumber { get; set; }
    public DateTime TimeIn { get; set; }
}
