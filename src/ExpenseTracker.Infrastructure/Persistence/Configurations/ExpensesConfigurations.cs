using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Infrastructure.Persistence.Configurations;

public class ExpensesConfigurations : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Amount)
            .HasPrecision(18, 2)
            .IsRequired();
        builder.Property(e => e.UserId);
        builder.Property(e => e.Category);
        builder.Property(e => e.Description);
        builder.Property(e => e.Date);
    }
}