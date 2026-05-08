using System.Dynamic;
using System.Reflection.Metadata;
using System.Transactions;
using EnterpriseBankingAPI.Domain.Common;
using EnterpriseBankingAPI.Infrastructure.Persistence;
using  Microsoft.EntityFrameworkCore;

namespace EnterpriseBankingAPI.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    public DbSet<User> Users => Set<User>();

    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();

    public DbSet<Transaction> Transactions => Set<Transaction>();

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.FromAccount)
            .WithMany(a => a.SentTransactions)
            .HasForeignKey(t => t.FromAccountId)
            .OneDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.ToAccount)
            .WithMany(a => a.ReceivedTransactions)
            .HasForeignKey(t => t.ToAccountId)
            .OneDelete(DeleteBehavior.Restrict);
    }
}