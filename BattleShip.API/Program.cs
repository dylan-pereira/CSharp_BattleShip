using BattleShip.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

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

Game game = new Game();
app.UseCors(c => { c.AllowAnyMethod(); c.AllowAnyOrigin(); c.AllowAnyHeader(); });


app.MapGet("/newgame", () =>
{
    game = new Game();
    var gridCells = new List<Coordinates>();
    for (int i = 0; i < 10; i++)
    {
        for (int j = 0; j < 10; j++)
        {
            gridCells.Add(new Coordinates { X = i, Y = j, Value = game.PlayerGrid.GridState[i, j] });
        }
    }

    return Results.Ok(new NewGameResponse { gameId = game.Id, PlayerShips = game.PlayerGrid.Ships, RealPlayerGrid = gridCells });
})
.WithName("GetNewGame")
.WithOpenApi();

app.MapGet("/test", () =>
{
    game = new Game();
    return Results.Ok(new { gameId = game.Id, playerShips = game.PlayerGrid.Ships });
})
.WithName("GetTest")
.WithOpenApi();

app.MapPost("/attack", ([FromBody] AttackRequest attackRequest) =>
{
    return Results.Ok(game.PlayerAttackIA(attackRequest.X, attackRequest.Y));
})
.WithName("PostAttack")
.WithOpenApi();

app.Run();