using Application.Organizations.Interfaces;
using Application.Shared.Clients;
using Application.Shared.Data;
using Application.Shared.Messaging;
using Domain.ValueObjects;

namespace Application.Organizations.Commands.UpdateOrganization;

public class UpdateOrganizationCommandHandler : ICommandHandler<UpdateOrganizationCommand, Result>
{
    private readonly IOrganizationsRepository _organizationsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapClient _mapClient;

    public UpdateOrganizationCommandHandler(
        IOrganizationsRepository organizationsRepository,
        IUnitOfWork unitOfWork,
        IMapClient mapClient)
    {
        _organizationsRepository = organizationsRepository;
        _unitOfWork = unitOfWork;
        _mapClient = mapClient;
    }

    public async Task<Result> Handle(UpdateOrganizationCommand command, CancellationToken cancellationToken)
    {
        var organization = await _organizationsRepository.GetByUserId(command.UserId, cancellationToken);

        if (organization == null)
        {
            return Result.Failure(new Error("", ""));
        }
        
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
        
        organization.Update(command.Title, command.Description, command.Phone,
            address, coordinates, command.Link, command.LogoPath);
        
        _organizationsRepository.Update(organization);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}