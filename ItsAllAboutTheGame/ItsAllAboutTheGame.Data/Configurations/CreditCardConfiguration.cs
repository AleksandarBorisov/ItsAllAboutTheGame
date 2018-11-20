using ItsAllAboutTheGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItsAllAboutTheGame.Data.Configurations
{
    internal class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
    {
        public void Configure(EntityTypeBuilder<CreditCard> builder)
        {
            builder.HasOne(card => card.User)
                .WithMany(user => user.Cards)
                .HasForeignKey(card => card.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
