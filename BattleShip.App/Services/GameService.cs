using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
public class GameService : IGameService
{
    private readonly HttpClient _httpClient;

    public GameService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<NewGameResponse?> StartNewGame()
    {
        return _httpClient.GetFromJsonAsync<NewGameResponse>("/newgame");
    }

    public async Task<AttackResponse?> AttackOpponent(Guid gameId, int x, int y)
    {
        var attackRequest = new AttackRequest
        {
            GameId = gameId,
            X = x,
            Y = y
        };

        var response = await _httpClient.PostAsJsonAsync("/attack", attackRequest);
        response.EnsureSuccessStatusCode(); // Throw if not successful
        return await response.Content.ReadFromJsonAsync<AttackResponse>();
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

    public async Task<List<Winner>> GetLeaderboard()
    {
        return await _httpClient.GetFromJsonAsync<List<Winner>>("/winners");
    }

}
