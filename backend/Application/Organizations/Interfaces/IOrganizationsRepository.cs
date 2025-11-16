using Application.Organizations.Models;
using Domain.Models;

namespace Application.Organizations.Interfaces;

public interface IOrganizationsRepository
{
    Task Add(Organization organization, CancellationToken cancellationToken = default);
    void Update(Organization organization);
    void Delete(Organization organization);
    Task<Organization?> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<OrganizationVm?> GetByIdVm(Guid id, CancellationToken cancellationToken = default);
    Task<Organization?> GetByUserId(Guid userId, CancellationToken cancellationToken = default);
    Task<OrganizationVm?> GetByUserIdVm(Guid userId, CancellationToken cancellationToken = default);
    Task<List<OrganizationVm>> GetList(CancellationToken cancellationToken = default);
    Task<List<OrganizationVm>> GetFilteredList(
        string? searchString, List<int>? categoryIds, string? city, CancellationToken cancellationToken = default);
    Task<List<OrganizationVm>> GetNotActiveList(CancellationToken cancellationToken = default);
}