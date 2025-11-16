using Application.Organizations.Models;
using Application.Shared.Data;
using Application.Shared.Messaging;

namespace Application.Organizations.Queries.GetFilteredOrganizationList;

public record GetFilteredOrganizationListQuery(
    string? SearchString, List<int>? CategoryIds, string? City) : IQuery<Result<OrganizationListVm>>;