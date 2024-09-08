using Bookify.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure;


internal class Repository<T> where T: Entity
{
    protected readonly ApplicationDbContext dbContext;

    public Repository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<T>()
            .FirstOrDefaultAsync(user=> user.Id == id, cancellationToken);
    }

    public void Add(T entity)
    {
        dbContext.Add(entity);
    }
}