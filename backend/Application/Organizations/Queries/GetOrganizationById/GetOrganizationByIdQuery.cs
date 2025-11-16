using Application.Organizations.Models;
using Application.Shared.Data;
using Application.Shared.Messaging;

namespace Application.Organizations.Queries.GetOrganizationById;

public record GetOrganizationByIdQuery(Guid Id) : IQuery<Result<OrganizationVm>>;