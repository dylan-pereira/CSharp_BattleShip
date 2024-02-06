public class Game
{
    private Grid playerGrid;
    private Grid computerGrid;

    public Game()
    {
        playerGrid = new Grid();
        computerGrid = new Grid();
        InitializeGrids();
    }

    private void InitializeGrids()
    {
        // Placer les bateaux sur la grille de l'ordinateur
        Random rand = new Random();
        string[] shipTypes = { "A", "B", "C", "D", "E", "F" };
        foreach (string shipType in shipTypes)
        {
            Ship ship = new Ship(shipType, rand.Next(1, 5));
            computerGrid.PlaceShip(ship, rand);
        }
    }

    public void DisplayPlayerGrid()
    {
        playerGrid.Display();
    }

    // Méthode pour que le joueur attaque une case de la grille de l'IA
    public void PlayerAttack(int x, int y)
    {
        computerGrid.ReceiveAttack(x, y);
    }

    // Méthode pour que l'IA attaque une case de la grille du joueur
    public void ComputerAttack()
    {
        Random rand = new Random();
        int x = rand.Next(0, 10);
        int y = rand.Next(0, 10);
        playerGrid.ReceiveAttack(x, y);
    }
}