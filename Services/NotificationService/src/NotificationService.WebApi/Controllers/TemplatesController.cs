using NotificationService.Application.Contracts;
using NotificationService.WebApi.Shared;
using Microsoft.AspNetCore.Mvc;

namespace NotificationService.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TemplatesController : ControllerBase
{
    private readonly ITemplateService _templateService;

    public TemplatesController(ITemplateService templateService)
    {
        _templateService = templateService;
    }

    [HttpGet("{templateName}/exists")]
    public async Task<ActionResult<ResponseResultModel<bool>>> TemplateExists(string templateName, CancellationToken ct)
    {
        var exists = await _templateService.TemplateExistsAsync(templateName, ct);

        return Ok(new ResponseResultModel<bool>
        {
            IsError = false,
            Message = exists ? "Template exists" : "Template not found",
            Data = exists
        });
    }
}
