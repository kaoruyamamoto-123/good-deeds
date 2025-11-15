using Domain.ValueObjects;

namespace Application.Organizations.Models;

public record OrganizationVm(
    Guid Id,
    string Title,
    string Description,
    string? Phone,
    string? Address,
    Coordinates? Coordinates,
    string Link,
    string? LogoPath,
    DateTime CreatedAt,
    List<string> Categories);