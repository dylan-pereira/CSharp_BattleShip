namespace BattleShip.Models;
public class Player
{
    public string Name { get; set; }

    public Grid PlayerGrid { get; set; } = new Grid { };

    public Player(string name)
    {
        Name = name;
    }


}