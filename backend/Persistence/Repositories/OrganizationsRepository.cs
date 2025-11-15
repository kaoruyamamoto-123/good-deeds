using Application.Organizations.Interfaces;
using Application.Organizations.Models;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class OrganizationsRepository : IOrganizationsRepository
{
    private readonly AppDbContext _dbContext;

    public OrganizationsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Organization organization, CancellationToken cancellationToken = default)
    {
        await _dbContext.Organizations.AddAsync(organization, cancellationToken);
    }

    public void Update(Organization organization)
    {
        _dbContext.Organizations.Update(organization);
    }

    public async Task<List<OrganizationVm>> GetList(CancellationToken cancellationToken = default)
    {
        var organizations = await _dbContext.Organizations
            .AsNoTracking()
            .Include(o => o.Categories)
                .ThenInclude(c => c.Category)
            .Select(o => ToVm(o))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return organizations;
    }

    public async Task<List<Category>> GetCategoriesByIds(List<int> ids, CancellationToken cancellationToken = default)
    {
        var categories = await _dbContext.Categories
            .AsNoTracking()
            .Where(c => ids.Contains(c.Id))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        
        return categories;
    }
    private static OrganizationVm ToVm(Organization organization)
    {
        return new OrganizationVm(
            Id: organization.Id,
            Title: organization.Title,
            Description: organization.Description,
            Phone: organization.Phone,
            Address: organization.Address is not null
                ? $"{organization.Address.City}, {organization.Address.Street}"
                : "",
            Coordinates: organization.Coordinates,
            Link: organization.Link,
            LogoPath: organization.LogoPath,
            CreatedAt: organization.CreatedAt,
            Categories: organization.Categories.Select(c => c.Category.Title).ToList());
    }
}