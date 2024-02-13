using BattleShip.Models;
public class Game
{
    public Guid Id { get; set; }
    public Grid PlayerGrid { get; private set; }
    public Grid OpponentGrid { get; private set; }
    public Player? Winner { get; set; } = null;

    public Player Player { get; set; } = new Player("Player");

    public Player Opponent { get; set; }


    public Game(int IADifficulty = -1) // if IADifficulty == -1 means no IA
    {
        Id = Guid.NewGuid();
        PlayerGrid = Player.PlayerGrid;
        PlaceShipsGrids(PlayerGrid);

        if (IADifficulty != -1)
        {
            Opponent = new PlayerIA("IA", IADifficulty, PlayerGrid);
        }
        else
        {
            Opponent = new Player("Opponent");
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
        char computerAttackResponse = '\0';
        SimpleAttackResponse? simpleAttackResponse = null;
        if (playerAttackResponse != '\0')
        {
            if (Winner == null)
            {
                Random rand = new Random();
                while (computerAttackResponse == '\0')
                {
                    simpleAttackResponse = AttackByIA();
                    computerAttackResponse = simpleAttackResponse.AttackResult;

                }
            }


            return new AttackResponse
            {
                ComputerAttackCoordinates = simpleAttackResponse != null && simpleAttackResponse.AttackedCoordinates != null
                    ? new Coordinates { X = simpleAttackResponse.AttackedCoordinates.X, Y = simpleAttackResponse.AttackedCoordinates.Y }
                    : null,
                PlayerAttackResponse = playerAttackResponse,
                ComputerAttackResponse = computerAttackResponse,
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
            PlayerIA computer = (PlayerIA)Opponent;
            Coordinates coordinates = computer.CoordinatesToPlay.First();
            computer.CoordinatesToPlay.RemoveAt(0);
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
            }
            return attackResponse;
        }

        return '\0';
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