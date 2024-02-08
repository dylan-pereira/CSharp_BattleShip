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
    return Results.Ok(new { gameId = game.Id, playerShips = game.PlayerGrid.Ships });
})
.WithName("GetNewGame")
.WithOpenApi();

app.MapPost("/attack", ([FromBody] AttackRequest attackRequest) =>
{
    return Results.Ok(game.PlayerAttackIA(attackRequest.X, attackRequest.Y));
})
.WithName("PostAttack")
.WithOpenApi();

app.Run();