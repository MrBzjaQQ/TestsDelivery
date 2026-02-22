using NotificationService.Application.Contracts;
using NotificationService.Application.DTOs.Requests;
using NotificationService.Application.DTOs.Responses;
using NotificationService.Domain.Exceptions;
using NotificationService.WebApi.Shared;
using Microsoft.AspNetCore.Mvc;

namespace NotificationService.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly IEmailNotificationService _emailService;
    private readonly INotificationQueueService _queueService;

    public NotificationsController(
        IEmailNotificationService emailService,
        INotificationQueueService queueService)
    {
        _emailService = emailService;
        _queueService = queueService;
    }

    [HttpPost("send-email")]
    public async Task<ActionResult<ResponseResultModel<SendEmailResponse>>> SendEmail(
        [FromBody] SendEmailRequest request,
        CancellationToken ct)
    {
        var result = await _emailService.SendEmailAsync(request, ct);

        return Ok(new ResponseResultModel<SendEmailResponse>
        {
            IsError = false,
            Message = "Email sent successfully",
            Data = result
        });
    }

    [HttpPost("test-created/{testId}")]
    public async Task<ActionResult<ResponseResultModel<TestCreatedNotificationResponse>>> SendTestCreatedNotification(
        Guid testId,
        [FromBody] TestCreatedNotificationRequest request,
        CancellationToken ct)
    {
        var sendRequest = new SendEmailRequest
        {
            To = request.StudentEmail,
            Subject = "New Test Available",
            TemplateName = "test-created",
            TemplateData = new
            {
                TestId = testId,
                StudentName = request.StudentName
            }
        };

        var result = await _emailService.SendEmailAsync(sendRequest, ct);

        var response = new TestCreatedNotificationResponse(
            result.EmailId,
            request.StudentId,
            testId,
            result.SentAt);

        return Ok(new ResponseResultModel<TestCreatedNotificationResponse>
        {
            IsError = false,
            Message = "Test created notification sent",
            Data = response
        });
    }

    [HttpPost("result-ready/{studentId}/{testId}")]
    public async Task<ActionResult<ResponseResultModel<TestResultNotificationResponse>>> SendTestResultNotification(
        Guid studentId,
        Guid testId,
        [FromBody] TestResultNotificationRequest request,
        CancellationToken ct)
    {
        var sendRequest = new SendEmailRequest
        {
            To = request.StudentEmail,
            Subject = "Test Result Available",
            TemplateName = "test-result",
            TemplateData = new
            {
                StudentName = request.StudentName,
                TestTitle = request.TestTitle,
                Score = request.Score,
                MaxScore = request.MaxScore,
                Percentage = request.Percentage,
                IsPassed = request.IsPassed
            }
        };

        var result = await _emailService.SendEmailAsync(sendRequest, ct);

        var response = new TestResultNotificationResponse(
            studentId,
            testId,
            request.Score,
            request.MaxScore,
            request.IsPassed,
            result.SentAt);

        return Ok(new ResponseResultModel<TestResultNotificationResponse>
        {
            IsError = false,
            Message = "Test result notification sent",
            Data = response
        });
    }

    [HttpGet("pending")]
    public async Task<ActionResult<ResponseResultModel<PendingNotificationsResponse>>> GetPending(CancellationToken ct)
    {
        var result = await _queueService.GetPendingNotificationsAsync(ct);

        return Ok(new ResponseResultModel<PendingNotificationsResponse>
        {
            IsError = false,
            Message = $"{result.TotalCount} pending notifications",
            Data = result
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseResultModel<NotificationStatusDto>>> GetById(Guid id, CancellationToken ct)
    {
        var notification = await _queueService.GetByIdAsync(id, ct);

        if (notification == null)
        {
            throw new NotificationNotFoundException(id);
        }

        var dto = new NotificationStatusDto(
            notification.Id,
            notification.To,
            notification.Subject,
            notification.TemplateName,
            notification.Status,
            notification.RetryCount,
            notification.CreatedAt,
            notification.SentAt,
            notification.ErrorMessage);

        return Ok(new ResponseResultModel<NotificationStatusDto>
        {
            IsError = false,
            Message = "Notification retrieved",
            Data = dto
        });
    }
}
