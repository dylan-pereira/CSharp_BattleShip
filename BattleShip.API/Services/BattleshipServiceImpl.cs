using BattleShip.Models;
using FluentValidation;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

public class BattleshipServiceImpl : BattleshipService.BattleshipServiceBase
{
    public Game ServiceGame { get; set; } = new Game();
    public override Task<NewGameResponseMessage> CreateNewGame(Empty request, ServerCallContext context)
    {
        ServiceGame = new Game();
        var GameResponse = new NewGameResponseMessage
        {
            GameId = ServiceGame.Id.ToString()
        };

        foreach (var ship in ServiceGame.PlayerGrid.Ships)
        {
            GameResponse.PlayerShips.Add(ConvertToShipMessage(ship));
        }

        return Task.FromResult(GameResponse);
    }

    public override Task<AttackResponseMessage> Attack(AttackRequestMessage request, ServerCallContext context)
    {
        AttackResponse response = ServiceGame.PlayerAttackIA(request.X, request.Y);

        return Task.FromResult(new AttackResponseMessage 
        {
            OpponentAttackCoordinate = ConvertToCoordinateMessage(response.ComputerAttackCoordinates),
            PlayerAttackResponse = response.PlayerAttackResponse.ToString(),
            OpponentAttackResponse = response.ComputerAttackResponse.ToString(),
            Winner = response.Winner,
            OpponentAttackResponseToReplace = ConvertToSimpleAttackResponseMessage(response.OpponentAttackResponseToReplace)
        });
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
            Size = ship.Size.ToString(),
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