using Application.Organizations.Models;
using Application.Shared.Data;
using Application.Shared.Messaging;

namespace Application.Organizations.Queries.GetOrganizationByUserId;

public record GetOrganizationByUserIdQuery(Guid UserId) : IQuery<Result<OrganizationVm>>;