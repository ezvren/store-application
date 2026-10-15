using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Infrastructure.Data;
using Store.Infrastructure.Repositories.Contract;

namespace Store.Infrastructure.Repositories.Implementation
{
    public class OperationRepository(AppDbContext context) : Repository<Operation>(context), IOperationRepository
    {
        public override Task<Operation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return context.Operation.Include(x => x.OperationBreakdowns).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
