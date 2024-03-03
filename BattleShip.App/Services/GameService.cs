using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using BattleShip.Models;
using System.Runtime.InteropServices;
public class GameService : IGameService
{
    private readonly HttpClient _httpClient;
    private readonly BattleshipService.BattleshipServiceClient _grpcClient;

    public GameService(HttpClient httpClient, BattleshipService.BattleshipServiceClient grpcClient)
    {
        _httpClient = httpClient;
        _grpcClient = grpcClient;
    }

    public async Task<NewGameResponse?> StartNewGame()
    {
        NewGameResponseMessage responseMessage = await _grpcClient.CreateNewGameAsync(new Google.Protobuf.WellKnownTypes.Empty());
        NewGameResponse response = new NewGameResponse
        {
            gameId = Guid.Parse(responseMessage.GameId),
            PlayerShips = new List<Ship>()
        };
        if (responseMessage.PlayerShips == null) return response;
        foreach (var item in responseMessage.PlayerShips)
        {
            response.PlayerShips.Add(ConvertToShip(item));
        }
        return response;
    }

    public async Task<AttackResponse?> AttackOpponent(Guid gameId, int x, int y)
    {
        AttackRequestMessage attackRequestMessage = new AttackRequestMessage
        {
            GameId = gameId.ToString(),
            X = x,
            Y = y
        };
        AttackResponseMessage attackResponseMessage = await _grpcClient.AttackAsync(attackRequestMessage);

        SimpleAttackResponse? simpleAttackResponse = null;
        if (attackResponseMessage.OpponentAttackResponseToReplace != null)
        {
            simpleAttackResponse = new SimpleAttackResponse
            {
                AttackedCoordinates = ConvertToCoordinates(attackResponseMessage.OpponentAttackResponseToReplace.AttackedCoordinates),
                AttackResult = attackResponseMessage.OpponentAttackResponseToReplace.AttackResult[0]
            };
        }

        return new AttackResponse
        {
            OpponentAttackCoordinates = ConvertToCoordinates(attackResponseMessage.OpponentAttackCoordinate),
            PlayerAttackResponse = attackResponseMessage.PlayerAttackResponse[0],
            OpponentAttackResponse = attackResponseMessage.OpponentAttackResponse[0],
            Winner = attackResponseMessage.Winner,
            OpponentAttackResponseToReplace = simpleAttackResponse
        };
    }
    public async Task<DifficultyRequest?> ChangeDifficulty(Guid gameId, int difficulty)
    {
        var difficultyRequest = new DifficultyRequest
        {
            GameId = gameId,
            Difficulty = difficulty,
        };

        var response = await _httpClient.PostAsJsonAsync("/difficulty", difficultyRequest);
        response.EnsureSuccessStatusCode(); // Throw if not successful
        return await response.Content.ReadFromJsonAsync<DifficultyRequest>();
    }

    public Coordinates? ConvertToCoordinates(CoordinateMessage? coordinates)
    {
        if (coordinates == null) return null;
        char? val;
        try
        {
            val = coordinates.Value[0];
        }
        catch (System.Exception)
        {
            val = null;
        }
        return new Coordinates
        {
            X = coordinates.X,
            Y = coordinates.Y,
            Value = val
        };
    }

    public Ship ConvertToShip(ShipMessage shipMessage)
    {
        var ship = new Ship(shipMessage.Size, shipMessage.Letter)
        {
            Horizontal = shipMessage.Horizontal
        };
        foreach (var Coordinate in shipMessage.Coordinates)
        {
            ship.Coordinates.Add(ConvertToCoordinates(Coordinate));
        };
        return ship;
    }

    public async Task<List<Winner>> GetLeaderboard()
    {
        return await _httpClient.GetFromJsonAsync<List<Winner>>("/winners");
    }

    public async Task<PlayerNameRequest?> ChangePlayerName(Guid gameId, string playerName)
    {
        var playernameRequest = new PlayerNameRequest
        {
            GameId = gameId,
            Name = playerName,
        };

        var response = await _httpClient.PostAsJsonAsync("/playername", playernameRequest);
        response.EnsureSuccessStatusCode(); // Throw if not successful
        return await response.Content.ReadFromJsonAsync<PlayerNameRequest>();
    }

}
