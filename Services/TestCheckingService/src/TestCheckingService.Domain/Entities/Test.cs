namespace TestCheckingService.Domain.Entities;

public class Test
{
    public Guid Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public int PassPercentage { get; init; }

    public int MaxScore { get; init; }

    public DateTime CreatedAt { get; init; }
}
