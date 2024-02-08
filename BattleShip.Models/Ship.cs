using BattleShip.Models;
public class Ship
{
    public string? Letter { get; set; }
    public int Size { get; set; }

    public List<Coordinates> Coordinates { get; set; } = [];
}