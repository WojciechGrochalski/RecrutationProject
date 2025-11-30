using Microsoft.EntityFrameworkCore;
using Project.Logic.Models;

namespace Project.Repo;

public class ProjectDbContext : DbContext
{
    public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options) { }

    public DbSet<ParkedVehicle> ParkedVehicles { get; set; }
    public DbSet<ParkingSpace> ParkingSpaces { get; set; }
}
