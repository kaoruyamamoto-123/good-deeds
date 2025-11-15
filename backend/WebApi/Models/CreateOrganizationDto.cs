namespace WebAPI.Models;

public class CreateOrganizationDto
{
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string? Phone { get; init; }
    public string? City { get; init; }
    public string? Street { get; init; }
    public string Link { get; init; } = null!;
    public string? LogoPath { get; init; }
    public List<int> CategoryIds { get; init; } = [];
}