namespace BffPortalService.Application.Specifications.GroupSpecs;

public class GroupHasStudentsSpecification
{
    public Guid GroupId { get; }

    public int MinimumStudents { get; }

    public GroupHasStudentsSpecification(Guid groupId, int minimumStudents = 1)
    {
        GroupId = groupId;
        MinimumStudents = minimumStudents;
    }

    public bool IsSatisfiedBy(int studentCount)
    {
        return studentCount >= MinimumStudents;
    }
}
