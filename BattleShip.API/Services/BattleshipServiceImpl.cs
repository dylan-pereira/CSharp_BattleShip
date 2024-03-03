using BattleShip.Models;
using FluentValidation;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

public class BattleshipServiceImpl : BattleshipService.BattleshipServiceBase
{
    private readonly Game _game;

    public BattleshipServiceImpl(Game game)
    {
        _game = game;
    }
    public override Task<NewGameResponseMessage> CreateNewGame(Empty request, ServerCallContext context)
    {
        _game.RestartGame();
        var GameResponse = new NewGameResponseMessage
        {
            GameId = _game.Id.ToString()
        };

        foreach (var ship in _game.PlayerGrid.Ships)
        {
            GameResponse.PlayerShips.Add(ConvertToShipMessage(ship));
        }

        return Task.FromResult(GameResponse);
    }

    public override Task<AttackResponseMessage> Attack(AttackRequestMessage request, ServerCallContext context)
    {
        AttackResponse response = _game.PlayerAttackIA(request.X, request.Y);

        AttackResponseMessage attackResponseMessage = new AttackResponseMessage
        {
            OpponentAttackCoordinate = ConvertToCoordinateMessage(response.OpponentAttackCoordinates),
            PlayerAttackResponse = response.PlayerAttackResponse.ToString(),
            OpponentAttackResponse = response.OpponentAttackResponse.ToString(),
            Winner = response.Winner ?? "",
            OpponentAttackResponseToReplace = ConvertToSimpleAttackResponseMessage(response.OpponentAttackResponseToReplace)
        };

        return Task.FromResult(attackResponseMessage);
    }

    public SimpleAttackResponseMessage? ConvertToSimpleAttackResponseMessage(SimpleAttackResponse? attackResponse)
    {

        if (attackResponse == null) return null;
        return new SimpleAttackResponseMessage
        {
            AttackedCoordinates = ConvertToCoordinateMessage(attackResponse.AttackedCoordinates),
            AttackResult = attackResponse.AttackResult.ToString()
        };
    }

    public CoordinateMessage? ConvertToCoordinateMessage(Coordinates? coordinates)
    {
        if (coordinates == null) return null;
        return new CoordinateMessage
        {
            X = coordinates.X,
            Y = coordinates.Y,
            Value = coordinates.Value.ToString()
        };
    }

    public ShipMessage ConvertToShipMessage(Ship ship)
    {
        var shipMessage = new ShipMessage
        {
            Letter = ship.Letter,
            Size = ship.Size,
            Horizontal = ship.Horizontal,
            ImagePath = ship.ImagePath
        };
        foreach (var Coordinate in ship.Coordinates)
        {
            shipMessage.Coordinates.Add(ConvertToCoordinateMessage(Coordinate));
        };
        return shipMessage;
    }
}