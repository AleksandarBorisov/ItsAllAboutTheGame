//using ItsAllAboutTheGame.Data.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace ItsAllAboutTheGame.Data.Configurations
//{
//    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
//    {
//        public void Configure(EntityTypeBuilder<UserRole> builder)
//        {
//            builder
//                .HasOne(e => e.Role)
//                .WithMany()
//                .HasForeignKey(e => e.RoleId)
//                .IsRequired()
//                .OnDelete(DeleteBehavior.Cascade);
//        }
//    }
//}
