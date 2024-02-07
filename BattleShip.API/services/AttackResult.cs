public class AttackResult
{

    public Coordinates? ComputerAttackCoordinates { get; set; }
    public char PlayerAttackResult { get; set; }
    public char ComputerAttackResult { get; set; }
    public string? Winner { get; set; } = string.Empty;

}
