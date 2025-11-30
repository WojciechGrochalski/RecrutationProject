namespace Project.Logic.Models;

public class ParkingSpace : BaseModel
{
    public int Place { get; set; }
    public bool IsOccupied { get; set; }
}
