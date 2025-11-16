using Application.Organizations.Interfaces;
using Application.Organizations.Models;
using Application.Shared.Data;
using Application.Shared.Messaging;

namespace Application.Organizations.Queries.GetOrganizationById;

public class GetOrganizationByIdQueryHandler : IQueryHandler<GetOrganizationByIdQuery, Result<OrganizationVm>>
{
    private readonly IOrganizationsRepository _organizationsRepository;

    public GetOrganizationByIdQueryHandler(IOrganizationsRepository organizationsRepository)
    {
        _organizationsRepository = organizationsRepository;
    }

    public async Task<Result<OrganizationVm>> Handle(
        GetOrganizationByIdQuery query, CancellationToken cancellationToken)
    {
        var organization = await _organizationsRepository.GetByIdVm(query.Id, cancellationToken).ConfigureAwait(false);

        if (organization == null)
        {
            return Result<OrganizationVm>.Failure(new Error("", ""));
        }
        
        return Result<OrganizationVm>.Success(organization);
    }
}