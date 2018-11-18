using ItsAllAboutTheGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItsAllAboutTheGame.Data.Configurations
{
    internal class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasOne(transaction => transaction.User)
                .WithMany(user => user.Transactions)
                .HasForeignKey(transaction => transaction.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(transation => transation.Type)
                .HasConversion<string>()
                .HasMaxLength(50);
        }
    }
}
