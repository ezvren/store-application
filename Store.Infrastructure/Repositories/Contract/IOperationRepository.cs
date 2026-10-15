using Store.Core.Entities;

namespace Store.Infrastructure.Repositories.Contract
{
    public interface IOperationRepository : IRepository<Operation>
    {
        Task<List<Operation>> GetOperationByUser(Guid idUser, CancellationToken cancellationToken = default);
    }
}
