namespace BffPortalService.Application.Specifications.PortalSpecs;

public class UserHasAccessSpecification
{
    public Guid UserId { get; }

    public Guid ResourceId { get; }

    public string ResourceType { get; }

    public UserHasAccessSpecification(Guid userId, Guid resourceId, string resourceType)
    {
        UserId = userId;
        ResourceId = resourceId;
        ResourceType = resourceType;
    }

    public bool IsSatisfiedBy(Guid? resourceOwnerId)
    {
        if (resourceOwnerId == null)
        {
            return false;
        }

        return UserId == resourceOwnerId.Value;
    }
}
