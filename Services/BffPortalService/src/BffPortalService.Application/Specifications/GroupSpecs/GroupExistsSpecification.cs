namespace BffPortalService.Application.Specifications.GroupSpecs;

public class GroupExistsSpecification
{
    public Guid GroupId { get; }

    public GroupExistsSpecification(Guid groupId)
    {
        GroupId = groupId;
    }

    public bool IsSatisfiedBy(bool groupExists)
    {
        return groupExists;
    }
}
