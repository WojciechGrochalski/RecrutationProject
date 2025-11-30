using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.Logic.Commands;
using Project.Logic.Queries;

namespace Project.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParkingController : ControllerBase
{
    private readonly IMediator _mediator;

    public ParkingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("parking")]
    public async Task<IActionResult> AddVehicleToParking([FromBody] AddVehicleToParkingCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("parking/exit")]
    public async Task<IActionResult> RemoveVehicleFromParking([FromBody] RemoveVehicleFromParkingCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("parking")]
    public async Task<IActionResult> GetParkingData()
    {
        var result = await _mediator.Send(new GetParkingDataQuery());
        return Ok(result);
    }
}
