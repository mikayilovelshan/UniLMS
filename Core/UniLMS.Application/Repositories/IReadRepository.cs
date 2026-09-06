using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using UniLMS.Domain.Entities.Common;

namespace UniLMS.Application.Repositories
{
    public interface IReadRepository<TEntity> : IRepositoryBase<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetAll(bool tracking = true);

        IQueryable<TEntity> GetWhere(Expression<Func<TEntity,bool>> expression, bool tracking = true);

        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true);

        Task<TEntity> GetByIdAsync(Guid id, bool tracking = true);
    } 
}
