public class Game
{
    public Grid PlayerGrid { get; private set; }
    public Grid ComputerGrid { get; private set; }

    public Game()
    {
        PlayerGrid = new Grid();
        ComputerGrid = new Grid();
        PlaceShipsGrids(ComputerGrid);
        PlaceShipsGrids(PlayerGrid);
        Console.WriteLine("Player Grid: ");
        DisplayPlayerGrid();
        Console.WriteLine("Computer Grid: ");
        DisplayComputerGrid();
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
    public void PlayerAttack(int x, int y)
    {
        ComputerGrid.ReceiveAttack(x, y);
    }

    // Méthode pour que l'IA attaque une case de la grille du joueur
    public void ComputerAttack()
    {
        Random rand = new Random();
        int x = rand.Next(0, 10);
        int y = rand.Next(0, 10);
        PlayerGrid.ReceiveAttack(x, y);
    }
}