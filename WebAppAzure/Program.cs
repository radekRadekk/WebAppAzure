using WebAppAzure.Repositories;

const string dbConnectionStringSecretName = "DbConnectionString";

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var keyVaultAccessLambdaAddress = builder.Configuration.GetValue<string>("KeyVaultAccessLambdaAddress");

using var httpClient = new HttpClient();
var dbConnectionStringResponse =
    await httpClient.GetAsync($"{keyVaultAccessLambdaAddress}?name={dbConnectionStringSecretName}");

var dbConnectionString = await dbConnectionStringResponse.Content.ReadAsStringAsync();

builder.Services.AddTransient<IPlayersRepository>(_ => new PlayersRepository(dbConnectionString));

var app = builder.Build();

app.MapGet("/players", async (IPlayersRepository playersRepository) => await playersRepository.GetPlayers());
app.MapGet("/players/{id:int}", async (IPlayersRepository playersRepository, int id) =>
{
    return await playersRepository.GetPlayer(id) is Player player
        ? Results.Ok(player)
        : Results.NotFound($"Player with given Id: {id} does not exist");
});

app.MapGet("/secrets/{name}", async (string name) =>
{
    var secretResponse = await httpClient.GetAsync($"{keyVaultAccessLambdaAddress}?name={name}");
    return await secretResponse.Content.ReadAsStringAsync();
});

app.Run();