using BattleShip.Models;

public class NewGameResponse
{

    public Guid gameId { get; set; }
    public List<Ship>? PlayerShips { get; set; }

}
