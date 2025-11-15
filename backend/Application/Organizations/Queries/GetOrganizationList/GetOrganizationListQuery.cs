using Application.Organizations.Models;
using Application.Shared.Data;
using Application.Shared.Messaging;

namespace Application.Organizations.Queries.GetOrganizationList;

public class GetOrganizationListQuery : IQuery<Result<OrganizationListVm>>;