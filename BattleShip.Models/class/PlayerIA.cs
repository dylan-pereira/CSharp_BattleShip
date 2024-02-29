namespace BattleShip.Models
{
    public class PlayerIA : Player
    {
        public int Difficulty { get; set; } = 0;
        public List<Coordinates> CoordinatesToPlay { get; set; } = [];
        public Grid? OpponentGrid { get; set; }
        public PlayerIA(string name, int difficulty = 0, Grid? opponentGrid = null) : base(name)
        {
            Difficulty = difficulty;
            OpponentGrid = opponentGrid;
            GenerateCoordinatesToPlay();
        }

        public void SetDifficulty(int difficulty)
        {
            Difficulty = difficulty;
            CoordinatesToPlay = [];
            GenerateCoordinatesToPlay();
        }

        public void GenerateCoordinatesToPlay()
        {
            switch (Difficulty)
            {
                case 2://Devin, imbattable, n'essaye même pas.
                    if (OpponentGrid != null)
                    {
                        foreach (var ship in OpponentGrid.Ships)
                        {
                            foreach (var coords in ship.Coordinates)
                            {
                                CoordinatesToPlay.Add(coords);
                            }
                        }
                    }
                    CompleteRandomly();
                    break;

                case 1://priorise les coordonnées des cases voisines aux ships dans un rayon de +/- 1,2 ou 3
                    if (OpponentGrid != null)
                    {
                        Random rand = new Random();

                        foreach (var ship in OpponentGrid.Ships)
                        {
                            foreach (var coords in ship.Coordinates)
                            {
                                List<Coordinates> neighbors = new List<Coordinates>();
                                for (int dx = -rand.Next(1, 4); dx <= rand.Next(1, 4); dx++)
                                {
                                    for (int dy = -rand.Next(1, 4); dy <= rand.Next(1, 4); dy++)
                                    {
                                        int newX = coords.X + dx;
                                        int newY = coords.Y + dy;
                                        if (newX >= 0 && newX < 10 && newY >= 0 && newY < 10)
                                        {
                                            neighbors.Add(new Coordinates { X = newX, Y = newY });
                                        }
                                    }
                                }
                                neighbors = neighbors.OrderBy(c => rand.Next()).ToList();
                                CoordinatesToPlay.AddRange(neighbors);
                            }
                        }
                    }
                    CompleteRandomly();
                    break;

                default: // IA bête et méchante, classe les coordonées aléatoirement
                    CompleteRandomly();
                    break;
            }
        }

        public void CompleteRandomly()
        {
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
        }
    }
}
