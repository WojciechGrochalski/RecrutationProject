using AutoMapper;
using Moq;
using Project.Logic.Commands;
using Project.Logic.Interfaces;
using Project.Logic.Models;
using Project.Logic.Profiles;

namespace Test.Commands;

public class AddVehicleToParkingCommandTest
{
    private readonly IMapper _mapper;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IParkedVehicleRepository> _mockParkedVehicleRepository;
    private readonly Mock<IParkingSpaceRepository> _mockParkingSpaceRepository;


    public AddVehicleToParkingCommandTest()
    {
        _mockMapper = new Mock<IMapper>();
        _mockParkedVehicleRepository = new Mock<IParkedVehicleRepository>();
        _mockParkingSpaceRepository = new Mock<IParkingSpaceRepository>();

        var profile = new AutoMapperProfiles();
        var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
        _mapper = new Mapper(config);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WrongType()
    {
        AddVehicleToParkingCommand command = new("reg", 0);
        ParkingSpace space = new()
        {
            Place = 1,
            IsOccupied = false,
        };

        _mockParkingSpaceRepository.Setup(m => m.GetFirstFreeSpace()).ReturnsAsync(space);

        var handler = new AddVehicleToParkingCommandHandler(
            _mapper,
            _mockParkedVehicleRepository.Object,
            _mockParkingSpaceRepository.Object
            );


        await Assert.ThrowsAnyAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

    }

    [Fact]
    public async Task Handle_Pass()
    {
        AddVehicleToParkingCommand command = new("reg", 1);
        ParkingSpace space = new()
        {
            Place = 1,
            IsOccupied = false,
        };
        ParkedVehicle parkedVehicle = new()
        {
            VehicleReg = command.VehicleReg,
            SpaceNumber = space.Place,
            Type = (Project.Logic.Enums.VehicleType)command.VehicleType,
            TimeIn = DateTime.UtcNow,
        };

        _mockParkingSpaceRepository.Setup(m => m.GetFirstFreeSpace()).ReturnsAsync(space);
        _mockParkingSpaceRepository.Setup(m => m.UpdateAsync(space)).ReturnsAsync(true);
        _mockParkedVehicleRepository.Setup(m => m.AddAsync(parkedVehicle)).ReturnsAsync(true);

        var handler = new AddVehicleToParkingCommandHandler(
            _mapper,
            _mockParkedVehicleRepository.Object,
            _mockParkingSpaceRepository.Object
            );


        var result = await handler.Handle(command, CancellationToken.None);
        _mockParkedVehicleRepository.Verify(x => x.AddAsync(It.IsAny<ParkedVehicle>()), Times.Once());
        _mockParkingSpaceRepository.Verify(x => x.UpdateAsync(It.IsAny<ParkingSpace>()), Times.Once());

    }
}
