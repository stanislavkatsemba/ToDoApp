using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data.Interfaces;
using ToDoApp.Data.Models;
using ToDoApp.Domain.ToDoItems.ReadModel;

namespace ToDoApp.Data
{
    public class ToDoAppContext : DbContext
    {
        public ToDoAppContext()
        {

        }

        public ToDoAppContext(DbContextOptions<ToDoAppContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=ToDoApp;Integrated Security=True");
            }
#endif
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        #region Tables

        public DbSet<UserModel> Users { get; set; }

        public DbSet<ToDoItemModel> ToDoItems { get; set; }

        #endregion

        #region Views

        public DbSet<ToDoItem> ToDoItemsRead { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.HasIndex(x => x.Name).IsUnique();
            });

            modelBuilder.Entity<ToDoItemModel>(entity =>
            {
                entity.HasQueryFilter(x => x.IsDeleted == false);
                entity.HasOne<UserModel>()
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<ToDoItem>().HasNoKey().ToView(null);
        }

        #region Update additional data by saving
        public override int SaveChanges()
        {
            UpdateAdditionalData();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            UpdateAdditionalData();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAdditionalData()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Entity is IEntityHasCreationInfo entityHasCreationInfo)
                        {
                            entityHasCreationInfo.CreationDate = DateTime.Now;
                        }
                        break;
                    case EntityState.Modified:
                        if (entry.Entity is IEntityHasModificationInfo entityHasModificationInfo)
                        {
                            entityHasModificationInfo.ModificationDate = DateTime.Now;
                        }
                        break;
                    case EntityState.Deleted:
                        if (entry.Entity is IEntitySoftDeletion entitySoftDeletion)
                        {
                            entry.State = EntityState.Modified;
                            entitySoftDeletion.IsDeleted = true;
                            entitySoftDeletion.DeletionDateTime = DateTime.Now;
                        }
                        break;
                }
            }
        }

        #endregion
    }
}
