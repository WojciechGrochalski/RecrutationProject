using MediatR;
using Project.Logic.Dtos;
using Project.Logic.Interfaces;

namespace Project.Logic.Queries;

public class GetParkingDataQueryHandler : IRequestHandler<GetParkingDataQuery, ParkingSpaceDto>
{
    private readonly IParkingSpaceRepository _parkingSpaceRepository;

    public GetParkingDataQueryHandler(IParkingSpaceRepository parkingSpaceRepository)
    {
        _parkingSpaceRepository = parkingSpaceRepository;
    }

    public async Task<ParkingSpaceDto> Handle(GetParkingDataQuery request, CancellationToken cancellationToken)
    {
        var parkingSpaces = await _parkingSpaceRepository.GetByAllAsync();

        int occupiedSpaces = parkingSpaces.Where(s => s.IsOccupied).Count();
        int availableSpaces = parkingSpaces.Count() - occupiedSpaces;
        ParkingSpaceDto dto = new()
        {
            AvailableSpaces = availableSpaces,
            OccupiedSpaces = occupiedSpaces
        };

        return dto;
    }
}
