using Application.Organizations.Interfaces;
using Application.Organizations.Models;
using Application.Shared.Data;
using Application.Shared.Messaging;

namespace Application.Organizations.Queries.GetOrganizationByUserId;

public class GetOrganizationByUserIdQueryHandler : IQueryHandler<GetOrganizationByUserIdQuery, Result<OrganizationVm>>
{
    private readonly IOrganizationsRepository _organizationsRepository;

    public GetOrganizationByUserIdQueryHandler(IOrganizationsRepository organizationsRepository)
    {
        _organizationsRepository = organizationsRepository;
    }

    public async Task<Result<OrganizationVm>> Handle(
        GetOrganizationByUserIdQuery query, CancellationToken cancellationToken)
    {
        var organization = await _organizationsRepository.GetByUserIdVm(query.UserId, cancellationToken).ConfigureAwait(false);

        if (organization == null)
        {
            return Result<OrganizationVm>.Failure(new Error("", ""));
        }
        
        return Result<OrganizationVm>.Success(organization);
    }
}