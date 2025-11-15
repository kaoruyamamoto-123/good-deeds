using Application.Shared.Data;
using Domain.ValueObjects;

namespace Application.Shared.Clients;

public interface IMapClient
{
    Task<Result<Coordinates>> GetCoordinates(string address, CancellationToken cancellationToken = default);
}