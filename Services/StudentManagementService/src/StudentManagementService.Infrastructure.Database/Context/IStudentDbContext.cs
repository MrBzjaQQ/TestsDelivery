using Microsoft.EntityFrameworkCore;
using StudentManagementService.Domain.Entities;

namespace StudentManagementService.Infrastructure.Database.Context;

public interface IStudentDbContext
{
    DbSet<Student> Students { get; }

    DbSet<StudyGroup> StudyGroups { get; }

    DbSet<TestAssignment> TestAssignments { get; }

    DbSet<TestProgress> TestProgresses { get; }

    Task<int> SaveChangesAsync(CancellationToken ct);
}
