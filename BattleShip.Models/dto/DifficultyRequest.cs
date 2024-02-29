using FluentValidation;

public class DifficultyRequest
{
    public Guid GameId { get; set; }
    public int Difficulty { get; set; }
}

public class DifficultyRequestValidator : AbstractValidator<DifficultyRequest>
{
    public DifficultyRequestValidator()
    {
        RuleFor(x => x.GameId).NotNull();
        RuleFor(x => x.GameId).NotEmpty();
        RuleFor(x => x.Difficulty).InclusiveBetween(0, 2);
    }
}