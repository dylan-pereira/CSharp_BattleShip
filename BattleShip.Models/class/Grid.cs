using BattleShip.Models;
public class Grid
{
    public int Size { get; set; } = 10;
    public char[,] GridState { get; set; }
    public List<Ship> Ships { get; } = new List<Ship>();
    public Grid()
    {
        GridState = new char[Size, Size];
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GridState[i, j] = '\0';
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
                if (newX >= 10 || newY >= 10 || GridState[newX, newY] != '\0')
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
            GridState[newX, newY] = ship.Letter[0];
            ship.Coordinates.Add(new Coordinates { X = newX, Y = newY });
            ship.Horizontal = horizontal;
        }
        Ships.Add(ship);
    }

    public char ReceiveAttack(int x, int y)
    {
        if (GridState[x, y] != '\0' && GridState[x, y] != 'O')
        {
            GridState[x, y] = 'X'; // Marquer la case comme touchée
        }
        else
        {
            GridState[x, y] = 'O';
        }
        return GridState[x, y];
    }

    public bool isOver()
    {
        return getNumberOfDestroyedShip() == getTotalShipSize();
    }

    public bool isAlreadyHitted(int x, int y)
    {
        return GridState[x, y] == 'X' || GridState[x, y] == 'O';
    }

    public int getNumberOfDestroyedShip()
    {
        int countX = 0;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (GridState[i, j] == 'X')
                {
                    countX++;
                }
            }
        }
        return countX;
    }

    public int getTotalShipSize()
    {
        int totalShipSize = 0;
        foreach (var ship in Ships)
        {
            totalShipSize += ship.Size;
        }
        return totalShipSize;
    }

    public void Display()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Console.Write(GridState[i, j] + " ");
            }
            Console.WriteLine();
        }
    }


}