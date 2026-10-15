using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;

namespace Store.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<Cashier, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Cashier> Cashier { get; set; }

    public DbSet<Operation> Operation { get; set; }

    public DbSet<OperationBreakdown> OperationBreakdown { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }
}