namespace BattleShip.Models
{
    public class PlayerIA : Player
    {
        public int Difficulty { get; set; } = 0;
        public List<Coordinates> CoordinatesToPlay { get; set; } = [];
        public Grid? OpponentGrid { get; set; } = null;
        public PlayerIA(string name, int difficulty = 0) : base(name)
        {
            Difficulty = difficulty;
            switch (Difficulty)
            {
                //TODO: Faire les autres difficultées pour l'IA
                case 1:

                    break;

                default: // IA bête et méchante, classe les coordonées aléatoirement
                    while (CoordinatesToPlay.Count() < PlayerGrid.Size * PlayerGrid.Size)
                    {
                        Random rand = new Random();
                        int x = rand.Next(0, 10);
                        int y = rand.Next(0, 10);
                        if (!CoordinatesToPlay.Any(coord => coord.X == x && coord.Y == y))
                        {
                            CoordinatesToPlay.Add(new Coordinates { X = x, Y = y });
                        }
                    }
                    break;
            }
        }
    }
}
