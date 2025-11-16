using Application.Organizations.Interfaces;
using Application.Shared.Data;
using Application.Shared.Messaging;

namespace Application.Organizations.Commands.ActivateOrganization;

public class ActivateOrganizationCommandHandler : ICommandHandler<ActivateOrganizationCommand, Result>
{
    private readonly IOrganizationsRepository _organizationsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateOrganizationCommandHandler(IOrganizationsRepository organizationsRepository, IUnitOfWork unitOfWork)
    {
        _organizationsRepository = organizationsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ActivateOrganizationCommand command, CancellationToken cancellationToken)
    {
        var organization = await _organizationsRepository.GetById(command.Id, cancellationToken);

        if (organization == null)
        {
            return Result.Failure(new Error("",""));
        }
        
        organization.Activate();
        _organizationsRepository.Update(organization);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}