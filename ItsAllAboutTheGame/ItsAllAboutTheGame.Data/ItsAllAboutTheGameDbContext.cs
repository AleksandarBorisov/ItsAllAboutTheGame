using ItsAllAboutTheGame.Data.Configurations;
using ItsAllAboutTheGame.Data.Constants;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Data.Models.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Data
{
    public class ItsAllAboutTheGameDbContext : IdentityDbContext<User>
    {
        public ItsAllAboutTheGameDbContext(DbContextOptions<ItsAllAboutTheGameDbContext> options)
            : base(options)
        {
        }

        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.ApplyAuditInfoRules();
            this.ApplyDeletionRules();
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<IdentityRole>()
            //    .HasData(new IdentityRole { Name = DataConstants.AdminRole, NormalizedName = DataConstants.AdminRole.ToUpper() });
            //builder.Entity<IdentityRole>()
            //    .HasData(new IdentityRole { Name = DataConstants.MasterAdminRole, NormalizedName = DataConstants.MasterAdminRole.ToUpper() });
            builder.ApplyConfiguration(new CreditCardConfiguration());
            builder.ApplyConfiguration(new WalletConfiguration());
            builder.ApplyConfiguration(new TransactionConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());

            base.OnModelCreating(builder);
        }

        private void ApplyDeletionRules()
        {
            var entitiesForDeletion = this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted && e.Entity is IDeletable);

            foreach (var entry in entitiesForDeletion)
            {
                var entity = (IDeletable)entry.Entity;
                entity.DeletedOn = DateTime.Now;
                entity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }
        }

        private void ApplyAuditInfoRules()
        {
            var newlyCreatedEntities = this.ChangeTracker.Entries()
                .Where(e => e.Entity is IAuditable && ((e.State == EntityState.Added) || (e.State == EntityState.Modified)));

            foreach (var entry in newlyCreatedEntities)
            {
                var entity = (IAuditable)entry.Entity;

                if (entry.State == EntityState.Added && entity.CreatedOn == null)
                {
                    entity.CreatedOn = DateTime.Now;
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }
    }
}
