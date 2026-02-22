using Microsoft.EntityFrameworkCore;
using QuestionManagementService.Application.Infrastructure.Database.Contract;
using QuestionManagementService.Domain.Entities;
using QuestionManagementService.Infrastructure.Database.Context;

namespace QuestionManagementService.Infrastructure.Database.Repositories;

public class TestTemplateRepository : ITestTemplateRepository
{
    private readonly IQuestionDbContext _context;

    public TestTemplateRepository(IQuestionDbContext context)
    {
        _context = context;
    }

    public async Task<TestTemplate?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.TestTemplates
            .FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task AddAsync(TestTemplate template, CancellationToken ct)
    {
        await _context.TestTemplates.AddAsync(template, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(TestTemplate template, CancellationToken ct)
    {
        var trackedEntity = await _context.TestTemplates.FindAsync(template.Id, ct);
        if (trackedEntity != null)
        {
            trackedEntity.Name = template.Name;
            trackedEntity.Description = template.Description;
            trackedEntity.DefaultDuration = template.DefaultDuration;
            trackedEntity.DefaultPassingScore = template.DefaultPassingScore;
            trackedEntity.Configuration = template.Configuration;
            await _context.SaveChangesAsync(ct);
        }
    }
}
