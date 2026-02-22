namespace BffPortalService.Domain.Exceptions;

public class PortalDataNotFoundException : Exception
{
    public string DataType { get; }

    public string Identifier { get; }

    public PortalDataNotFoundException(string dataType, string identifier)
        : base($"{dataType} not found for identifier: {identifier}")
    {
        DataType = dataType;
        Identifier = identifier;
    }

    public PortalDataNotFoundException(string dataType, string identifier, string message)
        : base(message)
    {
        DataType = dataType;
        Identifier = identifier;
    }

    public PortalDataNotFoundException(string dataType, string identifier, string message, Exception innerException)
        : base(message, innerException)
    {
        DataType = dataType;
        Identifier = identifier;
    }
}
