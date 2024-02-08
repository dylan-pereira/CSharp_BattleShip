using System.Threading.Tasks;

public interface IGameService
{
    Task<NewGameResponse?> StartNewGame();
    Task<AttackResponse?> AttackOpponent(Guid gameId, int x, int y);
}
