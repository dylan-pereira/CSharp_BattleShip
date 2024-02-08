using BattleShip.Models;
public class Game
{
    public Guid Id { get; set; }
    public Grid PlayerGrid { get; private set; }
    public Grid OpponentGrid { get; private set; }
    public string? WinnerName { get; set; } = null;

    public Game()
    {
        Id = Guid.NewGuid();
        PlayerGrid = new Grid("Joueur");
        OpponentGrid = new Grid("Ordinateur");
        PlaceShipsGrids(OpponentGrid);
        PlaceShipsGrids(PlayerGrid);
    }

    private void PlaceShipsGrids(Grid grid)
    {
        // Placer les bateaux sur la grille de l'ordinateur
        string[] shipTypes = { "A", "B", "C", "D", "E", "F" };
        int[] shipSizes = { 4, 4, 4, 3, 2, 2 };
        for (int i = 0; i < shipTypes.Length; i++)
        {
            Ship ship = new Ship { Letter = shipTypes[i], Size = shipSizes[i] };
            grid.PlaceShip(ship, new Random());
        }
    }

    public AttackResponse PlayerAttackIA(int x, int y)
    {

        char playerAttackResponse = Attack(OpponentGrid, x, y);
        char computerAttackResponse = '\0';

        if (playerAttackResponse != '\0')
        {
            int xOpponent = -1;
            int yOpponent = -1;

            if (WinnerName == null)
            {
                Random rand = new Random();
                while (computerAttackResponse == '\0')
                {
                    xOpponent = rand.Next(0, 10);
                    yOpponent = rand.Next(0, 10);
                    computerAttackResponse = Attack(PlayerGrid, xOpponent, yOpponent);
                }
            }
            return new AttackResponse
            {
                ComputerAttackCoordinates = new Coordinates { X = xOpponent, Y = yOpponent },
                PlayerAttackResponse = playerAttackResponse,
                ComputerAttackResponse = computerAttackResponse,
                Winner = WinnerName
            };
        }
        return new AttackResponse { };
    }

    public char Attack(Grid grid, int x, int y)
    {
        if (!grid.isAlreadyHitted(x, y) && WinnerName == null)
        {
            char attackResponse = grid.ReceiveAttack(x, y);
            if (grid.isOver())
            {
                if (grid == OpponentGrid)
                {

                    WinnerName = PlayerGrid.Name;
                }
                else if (grid == PlayerGrid)
                {
                    WinnerName = OpponentGrid.Name;
                }
            }
            return attackResponse;
        }

        return '\0';
    }

    public void RestartGame()
    {
        string? lastPlayerGridName = PlayerGrid.Name;
        string? lastOpponentGridName = OpponentGrid.Name;
        if (lastPlayerGridName != null && lastOpponentGridName != null)
        {
            PlayerGrid = new Grid(lastPlayerGridName);
            OpponentGrid = new Grid(lastOpponentGridName);
        }

        PlaceShipsGrids(OpponentGrid);
        PlaceShipsGrids(PlayerGrid);
    }
}