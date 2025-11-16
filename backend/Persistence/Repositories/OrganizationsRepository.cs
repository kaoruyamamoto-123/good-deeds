using Microsoft.EntityFrameworkCore;

using Application.Organizations.Interfaces;
using Application.Organizations.Models;
using Domain.Models;

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

    public void Delete(Organization organization)
    {
        _dbContext.Organizations.Remove(organization);
    }

    public async Task<Organization?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var organization = await _dbContext.Organizations
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken)
            .ConfigureAwait(false);
        
        return organization;
    }

    public async Task<OrganizationVm?> GetByIdVm(Guid id, CancellationToken cancellationToken = default)
    {
        var organization = await _dbContext.Organizations
            .AsNoTracking()
            .Where(o => o.Id == id) 
            .Include(o => o.Categories)
                .ThenInclude(c => c.Category)
            .Select(o => ToVm(o))
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
        
        return organization;
    }

    public async Task<Organization?> GetByUserId(Guid userId, CancellationToken cancellationToken = default)
    {
        var organization = await _dbContext.Organizations
            .FirstOrDefaultAsync(o => o.UserId == userId, cancellationToken)
            .ConfigureAwait(false);
        
        return organization;
    }

    public async Task<OrganizationVm?> GetByUserIdVm(Guid userId, CancellationToken cancellationToken = default)
    {
        var organization = await _dbContext.Organizations
            .AsNoTracking()
            .Where(o => o.UserId == userId) 
            .Include(o => o.Categories)
                .ThenInclude(c => c.Category)
            .Select(o => ToVm(o))
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
        
        return organization;
    }

    public async Task<List<OrganizationVm>> GetList(CancellationToken cancellationToken = default)
    {
        var organizations = await _dbContext.Organizations
            .AsNoTracking()
            .Where(o => o.IsActive)
            .Include(o => o.Categories)
                .ThenInclude(c => c.Category)
            .Select(o => ToVm(o))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return organizations;
    }

    public async Task<List<OrganizationVm>> GetFilteredList(
        string? searchString, List<int>? categoryIds, string? city, CancellationToken cancellationToken = default)
    {
        var organizations = _dbContext.Organizations
            .AsNoTracking()
            .Where(o => o.IsActive);

        if (!string.IsNullOrWhiteSpace(city))
        {
            organizations = organizations
                .Where(o => o.Address != null &&
                            EF.Functions.TrigramsSimilarity(o.Address.City, city) > 0.5)
                .OrderBy(o => EF.Functions.TrigramsSimilarityDistance(o.Address!.City, city));
        }

        if (categoryIds is not null && categoryIds.Count != 0)
        {
            organizations = organizations
                .Include(o => o.Categories)
                    .ThenInclude(c => c.Category)
                .Where(o => o.Categories.Any(c => categoryIds.Contains(c.Category.Id)));
        }

        if (searchString is not null)
        {
            organizations = organizations
                .Where(o => EF.Functions.TrigramsSimilarity(o.Title, searchString) > 0.15)
                .OrderBy(o => EF.Functions.TrigramsSimilarityDistance(o.Title, searchString));
        }
        
        var filtered = await organizations
            .Select(o => ToVm(o))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        
        return filtered;
    }

    public async Task<List<OrganizationVm>> GetNotActiveList(CancellationToken cancellationToken = default)
    {
        var organizations = await _dbContext.Organizations
            .AsNoTracking()
            .Where(o => !o.IsActive)
            .Include(o => o.Categories)
                .ThenInclude(c => c.Category)
            .Select(o => ToVm(o))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        
        return organizations;
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