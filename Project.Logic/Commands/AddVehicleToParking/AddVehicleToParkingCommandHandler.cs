using AutoMapper;
using MediatR;
using Project.Logic.Interfaces;
using Project.Logic.Models;

namespace Project.Logic.Commands;

public class AddVehicleToParkingCommandHandler : IRequestHandler<AddVehicleToParkingCommand, AddVehicleToParkingCommandResponse>
{
    private readonly IMapper _mapper;
    private readonly IParkedVehicleRepository _parkedVehicleRepository;
    private readonly IParkingSpaceRepository _parkingSpaceRepository;

    public AddVehicleToParkingCommandHandler(IMapper mapper, IParkedVehicleRepository parkedVehicleRepository, IParkingSpaceRepository parkingSpaceRepository)
    {
        _mapper = mapper;
        _parkedVehicleRepository = parkedVehicleRepository;
        _parkingSpaceRepository = parkingSpaceRepository;
    }

    public async Task<AddVehicleToParkingCommandResponse> Handle(AddVehicleToParkingCommand request, CancellationToken cancellationToken)
    {
        ValidVehicleType(request.VehicleType);
        var avialableParkingSpace = await _parkingSpaceRepository.GetFirstFreeSpace()
                ?? throw new Exception("No avialable space.");

        ParkedVehicle parkedVehicle = new()
        {
            VehicleReg = request.VehicleReg,
            SpaceNumber = avialableParkingSpace.Place,
            Type = (Enums.VehicleType)request.VehicleType,
            TimeIn = DateTime.UtcNow
        };

        var result = await _parkedVehicleRepository.AddAsync(parkedVehicle);

        avialableParkingSpace.IsOccupied = true;
        result = await _parkingSpaceRepository.UpdateAsync(avialableParkingSpace);

        if (result)
        {
            return _mapper.Map<AddVehicleToParkingCommandResponse>(parkedVehicle);
        }
        else
        {
            avialableParkingSpace.IsOccupied = false;
            await _parkingSpaceRepository.UpdateAsync(avialableParkingSpace);
            throw new Exception("An error occurred during this operation.");
        }
    }

    private static void ValidVehicleType(int type)
    {
        if (type != 1 && type != 2 && type != 3)
        {
            throw new Exception("Invalid vehicle type");
        }
    }
}
