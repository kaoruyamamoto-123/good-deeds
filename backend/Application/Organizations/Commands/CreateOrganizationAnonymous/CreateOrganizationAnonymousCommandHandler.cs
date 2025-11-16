using Application.Organizations.Interfaces;
using Application.Shared.Clients;
using Application.Shared.Data;
using Application.Shared.Messaging;
using Domain.Models;
using Domain.ValueObjects;

namespace Application.Organizations.Commands.CreateOrganizationAnonymous;

public class CreateOrganizationAnonymousCommandHandler : ICommandHandler<CreateOrganizationAnonymousCommand, Result>
{
    private readonly IOrganizationsRepository _organizationsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapClient _mapClient;

    public CreateOrganizationAnonymousCommandHandler(
        IUnitOfWork unitOfWork,
        IOrganizationsRepository organizationsRepository,
        IMapClient mapClient)
    {
        _unitOfWork = unitOfWork;
        _organizationsRepository = organizationsRepository;
        _mapClient = mapClient;
    }

    public async Task<Result> Handle(CreateOrganizationAnonymousCommand command, CancellationToken cancellationToken)
    {
        Coordinates? coordinates = null;
        Address? address = null;
        if (command is { City: not null, Street: not null })
        {
            address = new Address(command.City, command.Street);
            var result = await _mapClient.GetCoordinates(address.ToString(), cancellationToken);
            
            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }
            
            coordinates = result.Value;
        }
        
        var organization = Organization.Create(
            null, command.Title, command.Description,
            command.Phone, address, coordinates, command.Link, command.LogoPath);
        
        organization.Activate();
        
        foreach (var id in command.CategoryIds)
        {
            organization.AddCategory(id);
        }
        
        await _organizationsRepository.Add(organization, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}