using ItsAllAboutTheGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItsAllAboutTheGame.Data.Configurations
{
    internal class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.HasOne(deposit => deposit.User)
                .WithOne(user => user.Wallet)
                .HasForeignKey<User>(user => user.WalletId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(wallet => wallet.Currency)
                .HasConversion<string>()
                .HasMaxLength(3);
        }
    }
}
