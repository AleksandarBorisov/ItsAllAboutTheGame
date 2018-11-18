using ItsAllAboutTheGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItsAllAboutTheGame.Data.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(user => user.Deposit)
                .WithOne(deposit => deposit.User)
                .HasForeignKey<Deposit>(deposit => deposit.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(user => user.Cards)
                .WithOne(card => card.User)
                .HasForeignKey(user => user.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(user => user.Transactions)
                .WithOne(transaction => transaction.User)
                .HasForeignKey(user => user.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
