using Project.Logic.Enums;
using Project.Logic.Interfaces;
using Project.Logic.Models;

namespace Project.Repo.Repositories;

public class CalculationChargeService : ICalculationChargeService
{
    public double CalculateVehicleCharge(ParkedVehicle vehicle, DateTime timeOut)
    {
        double charge = GetChargeType(vehicle.Type);
        double minutes = (timeOut - vehicle.TimeIn).TotalMinutes;
        double timeCharge = minutes * charge;
        double additionalCharge = (int)(minutes / 5); ;
        double fullCharge = Math.Round(additionalCharge + timeCharge, 2);

        return fullCharge < 0.01 ? 0.01 : fullCharge;
    }

    private static double GetChargeType(VehicleType type)
    {
        return type switch
        {
            VehicleType.SmallCar => 0.1,
            VehicleType.MediumCar => 0.2,
            VehicleType.BigCar => 0.4,
            _ => 0.1,
        };
    }
}
