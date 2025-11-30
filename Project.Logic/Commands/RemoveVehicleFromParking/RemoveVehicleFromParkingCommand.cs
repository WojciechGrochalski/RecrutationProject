using MediatR;

namespace Project.Logic.Commands;

public record RemoveVehicleFromParkingCommand(string VehicleReg) : IRequest<RemoveVehicleFromParkingCommandResponse>;
