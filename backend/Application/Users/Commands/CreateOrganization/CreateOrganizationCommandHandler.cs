using Application.Shared.Clients;
using Application.Shared.Data;
using Application.Shared.Messaging;
using Application.Users.Errors;
using Application.Users.Interfaces;
using Domain.ValueObjects;

namespace Application.Users.Commands.CreateOrganization;

public class CreateOrganizationCommandHandler : ICommandHandler<CreateOrganizationCommand, Result>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapClient _mapClient;

    public CreateOrganizationCommandHandler(
        IUsersRepository usersRepository,
        IUnitOfWork unitOfWork,
        IMapClient mapClient)
    {
        _usersRepository = usersRepository;
        _unitOfWork = unitOfWork;
        _mapClient = mapClient;
    }

    public async Task<Result> Handle(CreateOrganizationCommand command, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetById(command.UserId, cancellationToken);

        if (user == null)
        {
            return Result.Failure(UserErrors.NotFound);
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

        user.CreateOrganization(
            title: command.Title, description: command.Description, command.CategoryIds,
            command.Phone, address, coordinates, command.Link, command.LogoPath);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}