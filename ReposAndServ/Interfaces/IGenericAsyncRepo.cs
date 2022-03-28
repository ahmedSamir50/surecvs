using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReposAndServ.Interfaces
{
    public interface IGenericAsyncRepo<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIDAsync(params object[] keyValues);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Over-ridable  Method Gets All From DBSet By Default 
        /// Given ToIncludeDSets Param Will Lazy Load The Included Properties 
        /// </summary>
        /// <param name="ToIncludeDSets"> Optional Param To Include Lazy/Load Other Entities Related ... Please Make Sure If Sent
        /// That The Parameter Should Be In The Format =>"PropertyOne,PropertyTwo,PropertyThree" Right Table Names Separated With Comma, With No Spaces 
        /// </param>
        /// <param name="filter"> Optional Param Predicate To Filter Entities  </param>
        /// <param name="orderBy"> Optional Param To Order Entities By  </param>
        /// <returns> IEnumerable<TEntity> List<T> Entities </returns>
        Task<IEnumerable<TEntity>> GetEntitiesAsync(string ToIncludeDSets = "", Expression<Func<TEntity, bool>> filter = null,
                                          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<TEntity> FindByFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<int> GetCountAsync();
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> AddAsync(TEntity entity, bool autoSave = false);
        Task<int> AddRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false);
        Task<bool> EditRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false);

        Task<bool> SoftDetleteAsync(string deActivatePropName = "IsActive", bool autoSave = false, bool setValueTo = true, params object[] keyValues);

        Task SaveAsync();
    }
}
