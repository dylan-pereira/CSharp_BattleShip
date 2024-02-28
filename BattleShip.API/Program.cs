using BattleShip.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
IValidator<AttackRequest> validator = new AttackRequestValidator();

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
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();
app.MapGrpcService<BattleshipServiceImpl>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGrpcReflectionService();
}

app.UseHttpsRedirection();

app.UseCors(c => { c.AllowAnyMethod(); c.AllowAnyOrigin(); c.AllowAnyHeader(); });

Game game = new Game(1);

app.MapGet("/newgame", () =>
{
    game = new Game(1);

    return Results.Ok(new NewGameResponse { gameId = game.Id, PlayerShips = game.PlayerGrid.Ships });
})
.WithName("GetNewGame")
.WithOpenApi();

app.MapPost("/attack", ([FromBody] AttackRequest attackRequest) =>
{
    ValidationResult result = validator.Validate(attackRequest);
    if (!result.IsValid)
    {
        return Results.BadRequest(result.ToDictionary());
    }
    return Results.Ok(game.PlayerAttackIA(attackRequest.X, attackRequest.Y));
})
.WithName("PostAttack")
.WithOpenApi();

app.Run();