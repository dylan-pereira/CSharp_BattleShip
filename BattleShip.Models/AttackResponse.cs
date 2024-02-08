using BattleShip.Models;

public class AttackResponse
{

    public Coordinates? ComputerAttackCoordinates { get; set; }
    public char PlayerAttackResponse { get; set; }
    public char ComputerAttackResponse { get; set; }
    public string? Winner { get; set; } = null;

}
