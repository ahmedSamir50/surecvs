using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ReposAndServ.Interfaces
{
    public interface IGenericRepo<TEntity> where TEntity : class
    {
        TEntity GetByID(params object[] keyValues);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

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
        IEnumerable<TEntity> GetEntities(string ToIncludeDSets = "", Expression<Func<TEntity, bool>> filter = null,
                                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        TEntity FindByFirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        int GetCount();
        int GetCount(Expression<Func<TEntity, bool>> predicate);

        void MarkAsModified(TEntity entity, bool autoSave = false);
        TEntity Add(TEntity entity, bool autoSave = false);
        int AddRange(IEnumerable<TEntity> entities, bool autoSave = false);

        bool Delete(bool autoSave = false, params object[] keyValues);
        bool Delete(TEntity entity, bool autoSave = false);
        int DeleteRange(IEnumerable<TEntity> entities, bool autoSave = false);

        bool Edit(TEntity entity, bool autoSave = false);
        bool EditRange(IEnumerable<TEntity> entities, bool autoSave = false);

        bool SoftDetlete(string deActivatePropName = "IsActive", bool autoSave = false, bool setValueTo = true, params object[] keyValues);

        void Save();
    }
}
