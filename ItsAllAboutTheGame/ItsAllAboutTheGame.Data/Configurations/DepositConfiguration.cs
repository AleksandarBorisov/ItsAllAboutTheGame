using ItsAllAboutTheGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItsAllAboutTheGame.Data.Configurations
{
    internal class DepositConfiguration : IEntityTypeConfiguration<Deposit>
    {
        public void Configure(EntityTypeBuilder<Deposit> builder)
        {
            builder.HasOne(deposit => deposit.User)
                .WithOne(user => user.Deposit)
                .HasForeignKey<User>(user => user.DepositId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(deposit => deposit.Currency)
                .HasConversion<string>()
                .HasMaxLength(3);
        }
    }
}
