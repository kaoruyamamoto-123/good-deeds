using Application.Organizations.Models;
using Domain.Models;

namespace Application.Organizations.Interfaces;

public interface IOrganizationsRepository
{
    Task Add(Organization organization, CancellationToken cancellationToken = default);
    void Update(Organization organization);
    Task<List<OrganizationVm>> GetList(CancellationToken cancellationToken = default);
    Task<List<Category>> GetCategoriesByIds(List<int> ids, CancellationToken cancellationToken = default);
}