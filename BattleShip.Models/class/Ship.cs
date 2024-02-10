using System.Reflection.Metadata.Ecma335;
using BattleShip.Models;
public class Ship
{
    public string Letter { get; set; }
    public int Size { get; set; }
    public List<Coordinates> Coordinates { get; set; } = [];

    public bool Horizontal { get; set; } = true;
    public string? ImagePath { get; set; }

    public Ship(int size, string letter)
    {
        Letter = letter;
        Size = size;

        switch (Size)
        {
            case 4:
                ImagePath = "images/ships/object1.svg";
                break;
            case 3:
                ImagePath = "images/ships/object2.svg";
                break;
            case 2:
                Random random = new Random();
                ImagePath = $"images/ships/object{random.Next(3, 5)}.svg";
                break;
            case 1:
                ImagePath = "images/ships/object5.svg";
                break;
            default:
                Console.WriteLine("Invalid size");
                break;
        }

        /*

    4<img style="width: 200px;" class="ship" src="images/ships/object1.svg" alt = "logo" />
    3<img style="width: 150px;" class="ship" src="images/ships/object2.svg" alt = "logo" />
    2<img style="width: 100px;" class="ship"src= "images/ships/object3.svg" alt = "logo" />
    2<img style="width: 100px;" class="ship"src= "images/ships/object4.svg" alt = "logo" />
    1<img style="width: 50px;" class="ship" src="images/ships/object5.svg" alt = "logo" />*/
    }
    public Coordinates? getFirstCoordinates()
    {
        return Coordinates.FirstOrDefault();
    }
}