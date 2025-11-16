using Domain.Common;

namespace Domain.Events;

public class OrganizationUpdatedEvent : DomainEvent
{
    public Guid OrganizationId { get; set; }
    public string? OldLogoPath { get; set; }
    public string? NewLogoPath { get; set; }

    public OrganizationUpdatedEvent(Guid organizationId, string? oldLogoPath, string? newLogoPath)
    {
        OrganizationId = organizationId;
        OldLogoPath = oldLogoPath;
        NewLogoPath = newLogoPath;
    }
}