using Microsoft.Data.SqlClient;

namespace WebAppAzure.Repositories;

public class PlayersRepository : IPlayersRepository
{
    private readonly string _connectionString;

    public PlayersRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<ICollection<Player>> GetPlayers(CancellationToken cancellationToken = default(CancellationToken))
    {
        var players = new List<Player>();

        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var command = new SqlCommand("SELECT * FROM Players", connection);
        var adapter = new SqlDataAdapter(command);

        await using var reader = await adapter.SelectCommand.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            players.Add(new Player
            {
                Id = (int) reader["Id"],
                BirthDate = (DateTime) reader["BirthDate"],
                CoachId = (int) reader["CoachId"],
                Country = (string) reader["Country"],
                FirstName = (string) reader["FirstName"],
                Height = (int) reader["Height"],
                LastName = (string) reader["LastName"],
                Weight = (double) reader["Weight"]
            });
        }

        await connection.CloseAsync();

        return players;
    }

    public async Task<Player> GetPlayer(int id, CancellationToken cancellationToken = default(CancellationToken))
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var command = new SqlCommand($"SELECT * FROM Players WHERE Id = {id}", connection);
        var adapter = new SqlDataAdapter(command);

        await using var reader = await adapter.SelectCommand.ExecuteReaderAsync(cancellationToken);

        var result = await reader.ReadAsync(cancellationToken)
            ? new Player
            {
                Id = (int) reader["Id"],
                BirthDate = (DateTime) reader["BirthDate"],
                CoachId = (int) reader["CoachId"],
                Country = (string) reader["Country"],
                FirstName = (string) reader["FirstName"],
                Height = (int) reader["Height"],
                LastName = (string) reader["LastName"],
                Weight = (double) reader["Weight"]
            }
            : null;

        await connection.CloseAsync();
        return result;
    }
}