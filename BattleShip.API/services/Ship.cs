public class Ship
{
    public string Letter { get; set; } = string.Empty;
    public int Size { get; set; }

    public Ship(string letter, int size)
    {
        Letter = letter;
        Size = size;
    }
}