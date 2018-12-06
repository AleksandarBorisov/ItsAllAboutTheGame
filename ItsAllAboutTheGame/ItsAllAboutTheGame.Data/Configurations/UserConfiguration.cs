using ItsAllAboutTheGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItsAllAboutTheGame.Data.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(user => user.Wallet)
                .WithOne(deposit => deposit.User)
                .HasForeignKey<Wallet>(wallet => wallet.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(user => user.Cards)
                .WithOne(card => card.User)
                .HasForeignKey(user => user.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(user => user.Transactions)
                .WithOne(transaction => transaction.User)
                .HasForeignKey(user => user.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(user => user.UserRoles)
            //    .WithOne(userRole => userRole.User)
            //    .HasForeignKey(userRole => userRole.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
