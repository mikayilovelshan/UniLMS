using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Entities.Common;

namespace UniLMS.Application.Repositories
{
    public interface IWriteRepository<TEntity> : IRepositoryBase<TEntity> where TEntity : BaseEntity
    {
        Task<bool> AddAsync(TEntity entity);

        Task<bool> AddRangeAsync(List<TEntity> entities);

        bool HardDelete(TEntity entity);

        bool SoftDelete(TEntity entity);

        bool HardDeleteRange(List<TEntity> entities);

        bool SoftDeleteRange(List<TEntity> entities);

        Task<bool> HardDeleteAsync(Guid id);

        Task<bool> SoftDeleteAsync(Guid id);

        bool Update(TEntity entity);

        bool UpdateRange(List<TEntity> entities);

        Task<bool> RestoreAsync(Guid id);

        Task<int> SaveAsync();
    }
}
