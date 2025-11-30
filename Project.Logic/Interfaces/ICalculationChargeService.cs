using Project.Logic.Models;

namespace Project.Logic.Interfaces;

public interface ICalculationChargeService
{
    public double CalculateVehicleCharge(ParkedVehicle vehicle, DateTime timeOut);
}
