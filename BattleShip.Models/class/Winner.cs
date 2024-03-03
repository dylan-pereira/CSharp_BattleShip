public class Winner
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Wins { get; set; } = 0;
    public DateTime LastWin { get; set; } = DateTime.UtcNow;
}
