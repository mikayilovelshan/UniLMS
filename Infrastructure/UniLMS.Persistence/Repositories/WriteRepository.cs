using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Repositories;
using UniLMS.Domain.Entities.Common;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Persistence.Repositories
{
    public class WriteRepository<TEntity>(AppDbContext _context) : IWriteRepository<TEntity> where TEntity : BaseEntity
    {
        public DbSet<TEntity> Table => _context.Set<TEntity>();

        public async Task<bool> AddAsync(TEntity entity)
        {
            EntityEntry<TEntity> entityEntry = await Table.AddAsync(entity);

            return entityEntry.State == EntityState.Added;

        }

        public async Task<bool> AddRangeAsync(List<TEntity> entities)
        {
            await Table.AddRangeAsync(entities);

            return true;
        }

 
        public bool Update(TEntity entity)
        {
            EntityEntry<TEntity> entityEntry = Table.Update(entity);
            return entityEntry.State == EntityState.Modified;

        }

 

        public bool HardDelete(TEntity entity)
        {
           EntityEntry entityEntry = Table.Remove(entity);
            return entityEntry.State == EntityState.Deleted;
        }

        public bool SoftDelete(TEntity entity)
        {
            entity.IsDeleted = true;
            return Update(entity);
        }

        public bool HardDeleteRange(List<TEntity> entities)
        {
            Table.RemoveRange(entities);
            return true;
        }

        public bool SoftDeleteRange(List<TEntity> entities)
        {
            foreach(var entity in entities)
            {
                entity.IsDeleted = true;
            }

            return UpdateRange(entities);
        }

        public async Task<bool> HardDeleteAsync(Guid id)
        {
            var entity = await Table.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return false;
            return HardDelete(entity);
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var entity = await Table.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return false;
            return SoftDelete(entity);
        }

        public bool UpdateRange(List<TEntity> entities)
        {
            Table.UpdateRange(entities);
            return true;
        }

        public async Task<bool> RestoreAsync(Guid id)
        {
            var entity = await Table.IgnoreQueryFilters().FirstOrDefaultAsync(f => f.Id == id);

            if (entity == null)
                return false;
            entity.IsDeleted = false;
            return Update(entity);
        }

        public async Task<int> SaveAsync()
     => await _context.SaveChangesAsync();
    }
}
