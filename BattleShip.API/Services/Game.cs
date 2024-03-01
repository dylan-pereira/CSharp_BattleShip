using BattleShip.Models;
public class Game
{
    public Guid Id { get; set; }
    public Grid PlayerGrid { get; private set; }
    public Grid OpponentGrid { get; private set; }
    public Player? Winner { get; set; } = null;

    public Player Player { get; set; } = new Player("Joueur");
    public Player Opponent { get; set; }


    public Game(int IADifficulty = -1) // if IADifficulty == -1 means no IA
    {
        Id = Guid.NewGuid();
        PlayerGrid = Player.PlayerGrid;
        PlaceShipsGrids(PlayerGrid);

        if (IADifficulty != -1)
        {
            Opponent = new PlayerIA("Ordi", IADifficulty, PlayerGrid);
        }
        else
        {
            Opponent = new Player("Adversaire");
        }
        OpponentGrid = Opponent.PlayerGrid;
        PlaceShipsGrids(OpponentGrid);
    }

    private void PlaceShipsGrids(Grid grid)
    {
        // Placer les bateaux sur la grille de l'ordinateur
        string[] shipTypes = { "A", "B", "C", "D", "E", "F" };
        int[] shipSizes = { 4, 4, 3, 2, 2, 1 };
        for (int i = 0; i < shipTypes.Length; i++)
        {
            Ship ship = new Ship(shipSizes[i], shipTypes[i]);
            grid.PlaceShip(ship, new Random());
        }
    }

    public AttackResponse PlayerAttackIA(int x, int y)
    {

        char playerAttackResponse = Attack(OpponentGrid, x, y);
        char opponentAttackResponse = '\0';
        SimpleAttackResponse? simpleAttackResponse = null;
        if (playerAttackResponse != '\0')
        {
            if (Winner == null)
            {
                Random rand = new Random();
                while (opponentAttackResponse == '\0')
                {
                    simpleAttackResponse = AttackByIA();
                    opponentAttackResponse = simpleAttackResponse.AttackResult;

                }
            }


            return new AttackResponse
            {
                OpponentAttackCoordinates = simpleAttackResponse != null && simpleAttackResponse.AttackedCoordinates != null
                    ? new Coordinates { X = simpleAttackResponse.AttackedCoordinates.X, Y = simpleAttackResponse.AttackedCoordinates.Y }
                    : null,
                PlayerAttackResponse = playerAttackResponse,
                OpponentAttackResponse = opponentAttackResponse,
                OpponentAttackResponseToReplace = simpleAttackResponse,
                Winner = Winner != null ? Winner.Name : null,

            };
        }
        return new AttackResponse { };
    }


    public SimpleAttackResponse AttackByIA()
    {
        if (Opponent is PlayerIA)
        {
            PlayerIA opponent = (PlayerIA)Opponent;
            Coordinates coordinates = opponent.CoordinatesToPlay.First();
            opponent.CoordinatesToPlay.RemoveAt(0);
            return new SimpleAttackResponse { AttackedCoordinates = coordinates, AttackResult = Attack(PlayerGrid, coordinates.X, coordinates.Y) };
        }
        return new SimpleAttackResponse { };
    }
    public char Attack(Grid grid, int x, int y)
    {
        if (!grid.isAlreadyHitted(x, y) && Winner == null)
        {
            char attackResponse = grid.ReceiveAttack(x, y);
            if (grid.isOver())
            {
                if (grid == OpponentGrid)
                {

                    Winner = Player;
                }
                else if (grid == PlayerGrid)
                {
                    Winner = Opponent;
                }
                using (var context = new WinnerDbContext())
                {
                    SaveWinner(context);
                }
            }
            return attackResponse;
        }

        return '\0';
    }

    public void SaveWinner(WinnerDbContext context)
    {
        if (Winner != null)
        {

            var winnerRecords = context.Winners.Where(w => w.Name == Winner.Name).ToList();
            if (!winnerRecords.Any())
            {
                var newWinner = new Winner
                {
                    Name = Winner.Name,
                    Wins = 1,
                };
                context.Winners.Add(newWinner);
                context.SaveChanges();
            }
            else
            {
                var winnerToChange = winnerRecords[0];
                winnerToChange.Wins = winnerToChange.Wins + 1;
                winnerToChange.LastWin = DateTime.UtcNow;
                context.SaveChanges();
            }
        }
    }

    public DifficultyRequest ChangeIADifficulty(int difficulty)
    {
        if (Opponent is PlayerIA)
        {
            PlayerIA opponent = (PlayerIA)Opponent;
            opponent.SetDifficulty(difficulty);
            return new DifficultyRequest { GameId = Id, Difficulty = difficulty };
        }
        return new DifficultyRequest { };
    }
    public void RestartGame()
    {
        string? lastPlayerGridName = Player.Name;
        string? lastOpponentGridName = Opponent.Name;
        if (lastPlayerGridName != null && lastOpponentGridName != null)
        {
            PlayerGrid = new Grid();
            OpponentGrid = new Grid();
        }
        PlaceShipsGrids(OpponentGrid);
        PlaceShipsGrids(PlayerGrid);
    }
}