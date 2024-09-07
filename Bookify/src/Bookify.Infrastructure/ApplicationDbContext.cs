using Bookify.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure;


public sealed class ApplicationDbContext: DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options): base(options)
    {
        
    }
}