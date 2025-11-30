using MediatR;
using Project.Logic.Dtos;

namespace Project.Logic.Queries;

public record GetParkingDataQuery() : IRequest<ParkingSpaceDto>;
