namespace Bookify.Domain;

public interface IApparmentRepository
{
    Task<Apartment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

}