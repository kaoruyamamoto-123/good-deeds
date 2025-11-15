namespace Domain.Models;

public class OrganizationCategory
{
    public Guid OrganizationId { get; set; }
    public virtual Organization Organization { get; set; } = null!;
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; } =  null!;
    public DateTime CreatedAt { get; set; }
    
    private OrganizationCategory() { }

    public static OrganizationCategory Create(Guid organizationId, int categoryId)
    {
        var oc = new OrganizationCategory
        {
            OrganizationId = organizationId,
            CategoryId = categoryId,
            CreatedAt = DateTime.UtcNow
        };
        
        return oc;
    }
}