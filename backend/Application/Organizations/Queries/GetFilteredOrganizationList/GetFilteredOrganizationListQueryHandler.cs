using Application.Organizations.Interfaces;
using Application.Organizations.Models;
using Application.Shared.Data;
using Application.Shared.Messaging;

namespace Application.Organizations.Queries.GetFilteredOrganizationList;

public class GetFilteredOrganizationListQueryHandler
    : IQueryHandler<GetFilteredOrganizationListQuery, Result<OrganizationListVm>>
{
    private readonly IOrganizationsRepository _organizationsRepository;

    public GetFilteredOrganizationListQueryHandler(IOrganizationsRepository organizationsRepository)
    {
        _organizationsRepository = organizationsRepository;
    }

    public async Task<Result<OrganizationListVm>> Handle(
        GetFilteredOrganizationListQuery query, CancellationToken cancellationToken)
    {
        var organizations = await _organizationsRepository.GetFilteredList(
            query.SearchString, query.CategoryIds, query.City, cancellationToken);
        
        return Result<OrganizationListVm>.Success(new OrganizationListVm(organizations));
    }
}