using Application.Shared.Data;
using Application.Shared.Messaging;

namespace Application.Organizations.Commands.DeleteOrganization;

public record DeleteOrganizationCommand(Guid Id) : ICommand<Result>;