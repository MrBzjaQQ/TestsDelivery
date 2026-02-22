using StudentManagementService.Domain.Entities;

namespace StudentManagementService.Application.DTOs.Requests;

public class SubmitTestRequest
{
    public List<AnswerDto> Answers { get; set; } = [];
}

public class AnswerDto
{
    public Guid QuestionId { get; set; }

    public Guid? SelectedOptionId { get; set; }

    public string? TextAnswer { get; set; }
}
