using Domain.Common;

namespace Domain.Events;

public class OrganizationDeletedEvent : DomainEvent
{
    public Guid OrganizationId { get; set; }
    public string? LogoPath { get; set; }
    
    public OrganizationDeletedEvent(Guid id, string? logoPath)
    {
        OrganizationId = id;
        LogoPath = logoPath;
    }
}