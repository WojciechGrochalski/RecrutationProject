using MediatR;
using Microsoft.EntityFrameworkCore;
using Project.Logic.Commands;
using Project.Logic.Enums;
using Project.Logic.Interfaces;
using Project.Logic.Models;
using Project.Logic.Profiles;
using Project.Repo;
using Project.Repo.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProjectDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
builder.Services.AddMediatR(typeof(AddVehicleToParkingCommandHandler).Assembly);
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfiles)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IParkedVehicleRepository, ParkedVehicleRepository>();
builder.Services.AddScoped<IParkingSpaceRepository, ParkingSpaceRepository>();
builder.Services.AddScoped<ICalculationChargeService, CalculationChargeService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProjectDbContext>();
    db.Database.Migrate();

    if (!db.ParkingSpaces.Any())
    {
        db.ParkingSpaces.AddRange(
            new ParkingSpace { Id = Guid.NewGuid(), Place = 1, IsOccupied = false },
            new ParkingSpace { Id = Guid.NewGuid(), Place = 2, IsOccupied = false },
            new ParkingSpace { Id = Guid.NewGuid(), Place = 3, IsOccupied = false }
        );
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
