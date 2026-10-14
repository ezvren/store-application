using Store.Core.Entities;
using Store.Infrastructure.Data;
using Store.Infrastructure.Repositories.Contract;

namespace Store.Infrastructure.Repositories.Implementation
{
    public class OperationRepository(AppDbContext context) : Repository<Operation>(context), IOperationRepository
    {
    }
}
