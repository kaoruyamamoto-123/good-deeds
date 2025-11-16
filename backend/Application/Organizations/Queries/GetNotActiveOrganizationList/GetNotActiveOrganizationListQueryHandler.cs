using Application.Organizations.Interfaces;
using Application.Organizations.Models;
using Application.Shared.Data;
using Application.Shared.Messaging;

namespace Application.Organizations.Queries.GetNotActiveOrganizationList;

public class GetNotActiveOrganizationListQueryHandler
    : IQueryHandler<GetNotActiveOrganizationListQuery, Result<OrganizationListVm>>
{
    private readonly IOrganizationsRepository _organizationsRepository;

    public GetNotActiveOrganizationListQueryHandler(IOrganizationsRepository organizationsRepository)
    {
        _organizationsRepository = organizationsRepository;
    }

    public async Task<Result<OrganizationListVm>> Handle(GetNotActiveOrganizationListQuery query, CancellationToken cancellationToken)
    {
        var organizations = await _organizationsRepository.GetNotActiveList(cancellationToken);
        return Result<OrganizationListVm>.Success(new OrganizationListVm(organizations));
    }
}