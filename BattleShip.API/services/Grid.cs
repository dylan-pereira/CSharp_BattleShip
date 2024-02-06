public class Grid
{
    private char[,] grid { get; }
    public Grid()
    {
        grid = new char[10, 10];
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                grid[i, j] = 'X';
            }
        }
    }

    public void PlaceShip(Ship ship, Random rand)
    {
        int x = rand.Next(0, 10);
        int y = rand.Next(0, 10);
        bool horizontal = rand.Next(2) == 0;

        // Vérifier si le placement est possible
        bool validPlacement = false;
        while (!validPlacement)
        {
            validPlacement = true;
            for (int i = 0; i < ship.Size; i++)
            {
                int newX = horizontal ? x + i : x;
                int newY = horizontal ? y : y + i;
                if (newX >= 10 || newY >= 10 || grid[newX, newY] != 'X')
                {
                    validPlacement = false;
                    x = rand.Next(0, 10);
                    y = rand.Next(0, 10);
                    horizontal = rand.Next(2) == 0;
                    break;
                }
            }
        }

        // Placer le bateau sur la grille
        for (int i = 0; i < ship.Size; i++)
        {
            int newX = horizontal ? x + i : x;
            int newY = horizontal ? y : y + i;
            grid[newX, newY] = ship.Letter[0];
        }
    }

    public void Display()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Console.Write(grid[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    public void ReceiveAttack(int x, int y)
    {
        if (grid[x, y] != 'X')
        {
            Console.WriteLine("Touché !");
            grid[x, y] = 'X'; // Marquer la case comme touchée
        }
        else
        {
            Console.WriteLine("Dans l'eau.");
        }
    }
}