public class Game
{
    public Guid Id { get; set; }
    public Grid PlayerGrid { get; private set; }
    public Grid ComputerGrid { get; private set; }

    public Game()
    {
        PlayerGrid = new Grid();
        ComputerGrid = new Grid();
        Id = Guid.NewGuid();
        PlaceShipsGrids(ComputerGrid);
        PlaceShipsGrids(PlayerGrid);
    }

    private void PlaceShipsGrids(Grid grid)
    {
        // Placer les bateaux sur la grille de l'ordinateur
        string[] shipTypes = { "A", "B", "C", "D", "E", "F" };
        int[] shipSizes = { 4, 3, 2, 2, 1, 1 };
        for (int i = 0; i < shipTypes.Length; i++)
        {
            Ship ship = new Ship(shipTypes[i], shipSizes[i]);
            grid.PlaceShip(ship, new Random());
        }
    }

    public void DisplayPlayerGrid()
    {
        PlayerGrid.Display();
    }
    public void DisplayComputerGrid()
    {
        ComputerGrid.Display();
    }

    // Méthode pour que le joueur attaque une case de la grille de l'IA
    public AttackResult PlayerAttack(int x, int y)
    {
        int xComputer = -1;
        int yComputer = -1;
        char computerAttackResult = '\0';
        char playerAttackResult = ComputerGrid.ReceiveAttack(x, y);
        string? winner = "null";
        if (ComputerGrid.isOver())
        {
            winner = "player";
        }
        else
        {
            Random rand = new Random();
            xComputer = rand.Next(0, 10);
            yComputer = rand.Next(0, 10);
            computerAttackResult = PlayerGrid.ReceiveAttack(xComputer, yComputer);
            if (PlayerGrid.isOver())
            {
                winner = "computer";
            }
        }

        return new AttackResult
        {
            ComputerAttackCoordinates = new Coordinates { X = xComputer, Y = yComputer },
            PlayerAttackResult = playerAttackResult,
            ComputerAttackResult = computerAttackResult,
            Winner = winner
        };
    }
}