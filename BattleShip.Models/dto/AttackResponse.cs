using BattleShip.Models;

public class AttackResponse
{

    public Coordinates? OpponentAttackCoordinates { get; set; }
    public char PlayerAttackResponse { get; set; }
    public char OpponentAttackResponse { get; set; }

    public SimpleAttackResponse? OpponentAttackResponseToReplace { get; set; }
    public string? Winner { get; set; } = null;

}
