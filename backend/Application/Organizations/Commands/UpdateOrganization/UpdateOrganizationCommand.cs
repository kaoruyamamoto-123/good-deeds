using Application.Shared.Data;
using Application.Shared.Messaging;

namespace Application.Organizations.Commands.UpdateOrganization;

public record UpdateOrganizationCommand(
    Guid UserId,
    string Title,
    string Description,
    string? Phone,
    string? City,
    string? Street,
    string Link,
    string? LogoPath,
    List<int> CategoryIds) : ICommand<Result>;