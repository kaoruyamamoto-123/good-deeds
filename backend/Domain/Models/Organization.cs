using Domain.Common;
using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Models;

public class Organization : AggregateRoot<Guid>
{
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string? Phone { get; private set; }
    public Address? Address { get; private set; }
    public Coordinates? Coordinates { get; private set; }
    public string Link { get; private set; } = null!;
    public string? LogoPath { get; private set; }
    public DateTime CreatedAt { get; private init; }
    public bool IsActive { get; private set; }
    
    private readonly List<OrganizationCategory> _categories = [];
    public IReadOnlyCollection<OrganizationCategory> Categories => _categories.AsReadOnly();
    
    // Nullable user for tests
    public Guid? UserId { get; private init; }
    public virtual User? User { get; private init; }

    private Organization() { }

    public static Organization Create(
        Guid? userId, string title, string description,
        string? phone, Address? address, Coordinates? coordinates, string link, string? logoPath)
    {
        var organization = new Organization
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            Address = address,
            Coordinates = coordinates,
            Phone = phone,
            Link = link,
            LogoPath = logoPath,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            IsActive = false
        };
        
        return organization;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Delete()
    {
        IsActive = false;
        AddDomainEvent(new OrganizationDeletedEvent(Id, LogoPath));
    }
    public void Update(
        string title, string description, string? phone,
        Address? address, Coordinates? coordinates, string link, string? logoPath)
    {
        Title = title;
        Description = description;
        Phone = phone;
        Address = address;
        Coordinates = coordinates;
        Link = link;
        
        var oldLogoPath = LogoPath;
        LogoPath = logoPath;
        
        AddDomainEvent(new OrganizationUpdatedEvent(Id, oldLogoPath, LogoPath));
    }

    public void AddCategory(int categoryId)
    { 
        var oc = OrganizationCategory.Create(Id, categoryId);
        _categories.Add(oc);
    }
}