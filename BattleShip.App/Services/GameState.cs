using BattleShip.Models;

public class GameState
{
    public Guid Id { get; set; } = Guid.Empty;
    public char[,] PlayerGrid { get; set; }
    public bool?[,] OpponentGrid { get; set; }
    public string? WinnerName { get; set; } = null;
    public List<Ship>? PlayerShips { get; set; }

    public GameState()
    {
        PlayerGrid = new char[10, 10];
        OpponentGrid = new bool?[10, 10];
    }

    public void ResetGame()
    {
        WinnerName = null;

        // Réinitialisation de la grille du joueur
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                PlayerGrid[i, j] = '\0';
            }
        }

        // Réinitialisation de la grille de l'adversaire
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                OpponentGrid[i, j] = null;
            }
        }

    }

    public int GetNumberOfDestroyedShipPlayer()
    {
        int countX = 0;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (PlayerGrid[i, j] == 'X')
                {
                    countX++;
                }
            }
        }
        return countX;
    }

    public int GetNumberOfDestroyedShipOpponent()
    {
        int countX = 0;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (OpponentGrid[i, j] == true)
                {
                    countX++;
                }
            }
        }
        return countX;
    }

    public int GetTotalShipSize()
    {
        int totalShipSize = 0;
        if (PlayerShips != null)
        {
            foreach (var ship in PlayerShips)
            {
                totalShipSize += ship.Size;
            }
        }

        return totalShipSize;
    }

}
