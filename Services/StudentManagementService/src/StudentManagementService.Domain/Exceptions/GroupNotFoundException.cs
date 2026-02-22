namespace StudentManagementService.Domain.Exceptions;

public class GroupNotFoundException : Exception
{
    public Guid GroupId { get; }

    public GroupNotFoundException(Guid groupId)
        : base($"Group with id '{groupId}' not found")
    {
        GroupId = groupId;
    }
}
