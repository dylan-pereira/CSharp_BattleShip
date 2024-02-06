var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/game", () =>
{
    var game = new Game(); // Créer une instance du jeu

    return game;
})
.WithName("GetGame")
.WithOpenApi();


app.MapGet("/playerGrid", () =>
{
    var game = new Game(); // Créer une instance du jeu

    var playerGrid = game.PlayerGrid;
    var gridData = new List<List<string>>();
    for (int i = 0; i < 10; i++)
    {
        var row = new List<string>();
        for (int j = 0; j < 10; j++)
        {
            //row.Add(playerGrid[i, j]);
            row.Add("\0");
        }
        gridData.Add(row);
    }
    return Results.Ok(gridData);
})
.WithName("GetPlayerGrid")
.WithOpenApi();



app.Run();