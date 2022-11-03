namespace WebAppAzure.Repositories;

public interface IPlayersRepository
{
    public Task<ICollection<Player>> GetPlayers(CancellationToken cancellationToken = default(CancellationToken));
    public Task<Player> GetPlayer(int id, CancellationToken cancellationToken = default(CancellationToken));
}