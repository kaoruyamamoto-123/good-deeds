using Application.Organizations.Interfaces;
using Application.Shared.Data;
using Application.Shared.Messaging;

namespace Application.Organizations.Commands.DeleteOrganization;

public class DeleteOrganizationCommandHandler : ICommandHandler<DeleteOrganizationCommand, Result>
{
    private readonly IOrganizationsRepository _organizationsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrganizationCommandHandler(IOrganizationsRepository organizationsRepository, IUnitOfWork unitOfWork)
    {
        _organizationsRepository = organizationsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteOrganizationCommand command, CancellationToken cancellationToken)
    {
        var organization = await _organizationsRepository.GetById(command.Id, cancellationToken);

        if (organization == null)
        {
            return Result.Failure(new Error("", ""));
        }
        
        organization.Delete();
        _organizationsRepository.Delete(organization);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}