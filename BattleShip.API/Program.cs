using BattleShip.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
IValidator<AttackRequest> validatorAttack = new AttackRequestValidator();
IValidator<DifficultyRequest> validatorDifficulty = new DifficultyRequestValidator();

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

Game game = new Game(1);
builder.Services.AddSingleton<Game>(game);

var app = builder.Build();
app.MapGrpcService<BattleshipServiceImpl>().EnableGrpcWeb();
app.UseGrpcWeb();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGrpcReflectionService();
}

app.UseCors(c => { c.AllowAnyMethod(); c.AllowAnyOrigin(); c.AllowAnyHeader(); });

app.MapGet("/newgame", () =>
{
    game.RestartGame();

    return Results.Ok(new NewGameResponse { gameId = game.Id, PlayerShips = game.PlayerGrid.Ships });
})
.WithName("GetNewGame")
.WithOpenApi();

app.MapPost("/attack", ([FromBody] AttackRequest attackRequest) =>
{
    ValidationResult result = validatorAttack.Validate(attackRequest);
    if (!result.IsValid)
    {
        return Results.BadRequest(result.ToDictionary());
    }
    return Results.Ok(game.PlayerAttackIA(attackRequest.X, attackRequest.Y));
})
.WithName("PostAttack")
.WithOpenApi();

app.MapPost("/difficulty", ([FromBody] DifficultyRequest difficultyRequest) =>
{
    ValidationResult result = validatorDifficulty.Validate(difficultyRequest);
    if (!result.IsValid)
    {
        return Results.BadRequest(result.ToDictionary());
    }
    return Results.Ok(game.ChangeIADifficulty(difficultyRequest.Difficulty));
})
.WithName("PostDifficulty")
.WithOpenApi();

app.Run();