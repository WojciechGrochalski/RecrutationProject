using MediatR;

namespace Project.Logic.Commands;

public record AddVehicleToParkingCommand : IRequest<AddVehicleToParkingCommandResponse>
{
    public string VehicleReg { get; set; }
    public int VehicleType { get; set; }

    public AddVehicleToParkingCommand(string vehicleReg, int vehicleType)
    {
        VehicleType = vehicleType;
        VehicleReg = vehicleReg;
    }
}
