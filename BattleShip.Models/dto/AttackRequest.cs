using FluentValidation;

public class AttackRequest
{
    public Guid GameId { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}

public class AttackRequestValidator : AbstractValidator<AttackRequest>
{
    public AttackRequestValidator() 
    {
        RuleFor(x => x.GameId).NotNull();
        RuleFor(x => x.GameId).NotEmpty();
        RuleFor(x => x.X).InclusiveBetween(0, 9);
        RuleFor(x => x.Y).InclusiveBetween(0, 9);
    }
}