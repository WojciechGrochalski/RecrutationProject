using AutoMapper;
using Project.Logic.Commands;
using Project.Logic.Models;

namespace Project.Logic.Profiles;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<ParkedVehicle, AddVehicleToParkingCommandResponse>();
    }
}