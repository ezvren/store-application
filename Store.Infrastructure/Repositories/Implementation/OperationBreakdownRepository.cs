using Store.Core.Entities;
using Store.Infrastructure.Data;
using Store.Infrastructure.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Infrastructure.Repositories.Implementation
{
    public class OperationBreakdownRepository(AppDbContext context) : Repository<OperationBreakdown>(context), IOperationBreakdownRepository
    {
    }
}
