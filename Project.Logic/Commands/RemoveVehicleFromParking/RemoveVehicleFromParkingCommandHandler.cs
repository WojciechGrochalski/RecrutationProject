using MediatR;
using Project.Logic.Interfaces;

namespace Project.Logic.Commands;

public class RemoveVehicleFromParkingCommandHandler : IRequestHandler<RemoveVehicleFromParkingCommand, RemoveVehicleFromParkingCommandResponse>
{
    private readonly IParkedVehicleRepository _parkedVehicleRepository;
    private readonly IParkingSpaceRepository _parkingSpaceRepository;
    private readonly ICalculationChargeService _calculationChargeService;

    public RemoveVehicleFromParkingCommandHandler(
        IParkedVehicleRepository parkedVehicleRepository,
        IParkingSpaceRepository parkingSpaceRepository,
        ICalculationChargeService calculationChargeService)
    {
        _parkedVehicleRepository = parkedVehicleRepository;
        _parkingSpaceRepository = parkingSpaceRepository;
        _calculationChargeService = calculationChargeService;
    }

    public async Task<RemoveVehicleFromParkingCommandResponse> Handle(RemoveVehicleFromParkingCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _parkedVehicleRepository.GetByVehicleReg(request.VehicleReg) ?? throw new Exception("The vehicle does not exist.");
        DateTime timeOut = DateTime.UtcNow;

        RemoveVehicleFromParkingCommandResponse respones = new()
        {
            VehicleReg = vehicle.VehicleReg,
            VehicleCharge = _calculationChargeService.CalculateVehicleCharge(vehicle, timeOut),
            TimeIn = vehicle.TimeIn,
            TimeOut = timeOut,
        };

        var isRemovedCar = await _parkedVehicleRepository.RemoveAsync(vehicle);
        var isRemovedPlace = await _parkingSpaceRepository.SetPlaceFree(vehicle.SpaceNumber);
        if (!isRemovedCar || !isRemovedPlace)
        {
            throw new Exception("An error occurred during this operation.");
        }

        return respones;
    }
}
