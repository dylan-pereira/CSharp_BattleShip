using System.Threading.Tasks;

public interface IGameService
{
    Task<NewGameResponse?> StartNewGame();
    Task<AttackResponse?> AttackOpponent(Guid gameId, int x, int y);
    Task<DifficultyRequest?> ChangeDifficulty(Guid gameId, int difficulty);
    Task<List<Winner>> GetLeaderboard();
}
