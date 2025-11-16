using Application.Shared.Data;
using Application.Shared.Messaging;

namespace Application.Users.Commands.CreateOrganization;

public record CreateOrganizationCommand(
    Guid UserId,
    string Title,
    string Description,
    string? Phone,
    string? City,
    string? Street,
    string Link,
    string? LogoPath,
    List<int> CategoryIds) : ICommand<Result>;