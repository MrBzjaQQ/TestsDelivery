namespace BffPortalService.Application.Exceptions;

public class DataAggregationException : Exception
{
    public string SourceService { get; }

    public string Operation { get; }

    public DataAggregationException(string sourceService, string operation)
        : base($"Failed to aggregate data from '{sourceService}' during '{operation}' operation")
    {
        SourceService = sourceService;
        Operation = operation;
    }

    public DataAggregationException(string sourceService, string operation, string message)
        : base(message)
    {
        SourceService = sourceService;
        Operation = operation;
    }

    public DataAggregationException(string sourceService, string operation, string message, Exception innerException)
        : base(message, innerException)
    {
        SourceService = sourceService;
        Operation = operation;
    }
}
