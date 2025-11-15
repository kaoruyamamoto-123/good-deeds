using Application.Organizations.Interfaces;
using Application.Organizations.Models;
using Application.Shared.Data;
using Application.Shared.Messaging;
using Domain.Models;

namespace Application.Organizations.Queries.GetOrganizationList;

public class GetOrganizationListQueryHandler : IQueryHandler<GetOrganizationListQuery, Result<OrganizationListVm>>
{
    private readonly IOrganizationsRepository _organizationsRepository;

    public GetOrganizationListQueryHandler(IOrganizationsRepository organizationsRepository)
    {
        _organizationsRepository = organizationsRepository;
    }

    public async Task<Result<OrganizationListVm>> Handle(
        GetOrganizationListQuery query, CancellationToken cancellationToken)
    {
        var organizations = await _organizationsRepository.GetList(cancellationToken).ConfigureAwait(false);
        return Result<OrganizationListVm>.Success(new OrganizationListVm(organizations));
    }
}