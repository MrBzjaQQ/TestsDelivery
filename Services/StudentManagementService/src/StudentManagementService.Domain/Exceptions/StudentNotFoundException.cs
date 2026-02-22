namespace StudentManagementService.Domain.Exceptions;

public class StudentNotFoundException : Exception
{
    public Guid StudentId { get; }

    public StudentNotFoundException(Guid studentId)
        : base($"Student with id '{studentId}' not found")
    {
        StudentId = studentId;
    }
}
