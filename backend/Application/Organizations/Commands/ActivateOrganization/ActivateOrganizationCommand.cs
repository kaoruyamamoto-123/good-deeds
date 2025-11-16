using Application.Shared.Data;
using Application.Shared.Messaging;

namespace Application.Organizations.Commands.ActivateOrganization;

public record ActivateOrganizationCommand(Guid Id) : ICommand<Result>;