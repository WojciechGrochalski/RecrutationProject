using AutoMapper;
using Moq;
using Project.Logic.Commands;
using Project.Logic.Enums;
using Project.Logic.Interfaces;
using Project.Logic.Models;
using Project.Logic.Profiles;
using Project.Repo.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Test.Commands;

public class RemoveVehicleFromParkingCommandTest
{
    private readonly IMapper _mapper;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IParkedVehicleRepository> _mockParkedVehicleRepository;
    private readonly Mock<IParkingSpaceRepository> _mockParkingSpaceRepository;
    private readonly Mock<ICalculationChargeService> _mockCalculationChargeService;


    public RemoveVehicleFromParkingCommandTest()
    {
        _mockMapper = new Mock<IMapper>();
        _mockParkedVehicleRepository = new Mock<IParkedVehicleRepository>();
        _mockParkingSpaceRepository = new Mock<IParkingSpaceRepository>();
        _mockCalculationChargeService = new Mock<ICalculationChargeService>();

        var profile = new AutoMapperProfiles();
        var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
        _mapper = new Mapper(config);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_NotFound()
    {
        RemoveVehicleFromParkingCommand command = new("reg");
        ParkedVehicle? vehicleNotFound = null;

        _mockParkedVehicleRepository.Setup(m => m.GetByVehicleReg(command.VehicleReg)).ReturnsAsync(vehicleNotFound);

        var handler = new RemoveVehicleFromParkingCommandHandler(
            _mockParkedVehicleRepository.Object,
            _mockParkingSpaceRepository.Object,
            _mockCalculationChargeService.Object
            );


        await Assert.ThrowsAnyAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

    }

    [Fact]
    public async Task Handle_Pass()
    {
        RemoveVehicleFromParkingCommand command = new("reg");
        ParkedVehicle vehicle = new()
        {
            VehicleReg = command.VehicleReg,
            Type = VehicleType.MediumCar,
            SpaceNumber = 1,
            TimeIn = DateTime.UtcNow
        };

        _mockParkedVehicleRepository.Setup(m => m.GetByVehicleReg(command.VehicleReg)).ReturnsAsync(vehicle);
        _mockParkedVehicleRepository.Setup(m => m.RemoveAsync(vehicle)).ReturnsAsync(true);
        _mockParkingSpaceRepository.Setup(m => m.SetPlaceFree(vehicle.SpaceNumber)).ReturnsAsync(true);

        var handler = new RemoveVehicleFromParkingCommandHandler(
            _mockParkedVehicleRepository.Object,
            _mockParkingSpaceRepository.Object,
            _mockCalculationChargeService.Object
            );


        var result = await handler.Handle(command, CancellationToken.None);
        _mockParkedVehicleRepository.Verify(x => x.RemoveAsync(It.IsAny<ParkedVehicle>()), Times.Once());
        _mockParkingSpaceRepository.Verify(x => x.SetPlaceFree(It.IsAny<int>()), Times.Once());

    }

    [Theory]
    [InlineData(VehicleType.SmallCar, 0.3, "2025-11-30 12:00:00", "2025-11-30 12:01:30")]
    [InlineData(VehicleType.MediumCar, 0.45, "2025-11-30 12:00:00", "2025-11-30 12:01:30")]
    [InlineData(VehicleType.BigCar, 0.75, "2025-11-30 12:00:00", "2025-11-30 12:01:30")]
    public async Task Handle_CalculationCharge(VehicleType type, double result, string timeIn, string timeOut)
    {
        RemoveVehicleFromParkingCommand command = new("reg");
        ParkedVehicle vehicle = new()
        {
            VehicleReg = command.VehicleReg,
            Type = type,
            SpaceNumber = 1,
            TimeIn = DateTime.ParseExact(timeIn, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
        };

        _mockParkedVehicleRepository.Setup(m => m.GetByVehicleReg(command.VehicleReg)).ReturnsAsync(vehicle);
        _mockParkedVehicleRepository.Setup(m => m.RemoveAsync(vehicle)).ReturnsAsync(true);
        _mockParkingSpaceRepository.Setup(m => m.SetPlaceFree(vehicle.SpaceNumber)).ReturnsAsync(true);
        _mockCalculationChargeService.Setup(m => m.CalculateVehicleCharge(
            It.IsAny<ParkedVehicle>(), It.IsAny<DateTime>())).Returns(result);

        var handler = new RemoveVehicleFromParkingCommandHandler(
            _mockParkedVehicleRepository.Object,
            _mockParkingSpaceRepository.Object,
            _mockCalculationChargeService.Object
            );


        var commandResult = await handler.Handle(command, CancellationToken.None);
        Assert.Equal(commandResult.VehicleCharge, result);
        _mockCalculationChargeService.Verify(x => x.CalculateVehicleCharge(It.IsAny<ParkedVehicle>(), It.IsAny<DateTime>()), Times.Once());

    }
}
