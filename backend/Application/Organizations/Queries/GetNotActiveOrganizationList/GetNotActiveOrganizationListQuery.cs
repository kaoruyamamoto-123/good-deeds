using Application.Organizations.Models;
using Application.Shared.Data;
using Application.Shared.Messaging;

namespace Application.Organizations.Queries.GetNotActiveOrganizationList;

public record GetNotActiveOrganizationListQuery : IQuery<Result<OrganizationListVm>>;