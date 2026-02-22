namespace NotificationService.WebApi.Shared;

public class ResponseResultModel<T>
{
    public bool IsError { get; set; }

    public DateTime Timestamp => DateTime.UtcNow;

    public string Message { get; set; } = string.Empty;

    public T? Data { get; set; }
}
