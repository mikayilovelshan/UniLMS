using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using UniLMS.Application.Repositories;
using UniLMS.Domain.Entities.Common;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Persistence.Repositories
{
    public class ReadRepository<TEntity>(AppDbContext _context) : IReadRepository<TEntity> where TEntity : BaseEntity
    {
        public DbSet<TEntity> Table => _context.Set<TEntity>();

        public IQueryable<TEntity> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable();

            if (!tracking)
                query = query.AsNoTracking();

            return query;
        }

        public async Task<TEntity> GetByIdAsync(Guid id, bool tracking = true)
            => await GetSingleAsync(x => x.Id == id, tracking);

        public Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true)
        {
            var query = Table.AsQueryable();
            
            if(!tracking)
                query = query.AsNoTracking();
           
            return query.FirstOrDefaultAsync(expression);
        }

        public IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> expression, bool tracking = true)
        {
            var query = Table.Where(expression);

            if (tracking)
                query = query.AsNoTracking();

            return query;
        }
    }
}
