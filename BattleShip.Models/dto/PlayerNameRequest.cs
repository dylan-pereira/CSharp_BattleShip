using FluentValidation;

public class PlayerNameRequest
{
    public Guid GameId { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class PlayerNameRequestValidator : AbstractValidator<PlayerNameRequest>
{
    public PlayerNameRequestValidator()
    {
        RuleFor(x => x.GameId).NotNull();
        RuleFor(x => x.GameId).NotEmpty();
        RuleFor(x => x.Name).MaximumLength(50).WithMessage("Le nom du joueur ne peut pas dépasser 50 caractères.");

    }
}