using DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using ReposAndServ.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ReposAndServ.Repository
{
        public class SureCvsRepo<TEntity> : IGenericRepo<TEntity>,
                                             IGenericAsyncRepo<TEntity>,
                                             IDisposable where TEntity : class
        {
            private readonly SureCvDbContext _context;
            private readonly DbSet<TEntity> dbSet;
            public SureCvsRepo(DbContextOptions opt, SureCvDbContext context = null)
            {
                if (context == null)
                    _context = new SureCvDbContext(opt);
                else
                    _context = context;

                dbSet = _context.Set<TEntity>();
            }

            #region Synchronous 

            #region GET 
            public TEntity GetByID(params object[] keyValues)
            {
                return dbSet.Find(keyValues);
            }
            public IEnumerable<TEntity> GetAll()
            {
                IEnumerable<TEntity> query = dbSet;
                return query;
            }

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
            public virtual IEnumerable<TEntity> GetEntities(string ToIncludeDSets = "", Expression<Func<TEntity, bool>> filter = null,
                                                              Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
            {
                // Prepare The Type Of Entity DBSet => Select * from TEntity
                IQueryable<TEntity> dataQuery = dbSet;
                // If Filter Condition Not Null Filter  Add The Where Statement Predicate  
                #region Where 
                if (filter != null)
                    dataQuery = dataQuery.Where(filter);
                #endregion Where 
                // If The ToIncludeDSets String Contains Many DBSets / Properties To Include Separated By , Comma 
                #region Include
                foreach (var propSet in ToIncludeDSets.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    try { dataQuery = dataQuery.Include(propSet); }
                    // No Need To Handle This Error Just Skip To Next Prop.
                    catch { }
                #endregion Include
                // Finally Order By Statement 
                #region Order By 
                if (orderBy != null)
                    return orderBy(dataQuery).ToList();

                #endregion Order By
                return dataQuery.ToList();

            }

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
            public virtual IQueryable<TEntity> GetEntitiesAsQuerable(string ToIncludeDSets = "", Expression<Func<TEntity, bool>> filter = null,
                                                              Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
            {
                // Prepare The Type Of Entity DBSet => Select * from TEntity
                IQueryable<TEntity> dataQuery = dbSet;
                // If Filter Condition Not Null Filter  Add The Where Statement Predicate  
                #region Where 
                if (filter != null)
                    dataQuery = dataQuery.AsQueryable().Where(filter);
                #endregion Where 
                // If The ToIncludeDSets String Contains Many DBSets / Properties To Include Separated By , Comma 
                #region Include
                foreach (var propSet in ToIncludeDSets.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    try { dataQuery = dataQuery.Include(propSet); }
                    // No Need To Handle This Error Just Skip To Next Prop.
                    catch { }
                #endregion Include
                // Finally Order By Statement 
                #region Order By 
                if (orderBy != null)
                    return orderBy(dataQuery);

                #endregion Order By
                return dataQuery;

            }
            #endregion

            #region Find 

            public IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
            {
                IEnumerable<TEntity> query = dbSet.Where(predicate);
                return query;
            }

            public TEntity FindByFirstOrDefault(Expression<Func<TEntity, bool>> predicate)
            {
                TEntity query = dbSet.FirstOrDefault(predicate);
                return query;
            }

            #endregion


            #region Add , Modify , Edit , Delete ,MarkAsModified "Attach and Update " One Object 


            public TEntity Add(TEntity entity, bool autoSave = false)
            {

                var result = dbSet.Add(entity);
                TEntity resultObj = result.Entity;
                if (autoSave)
                    Save();


                return resultObj;
            }

            // Update/Save Entity in database
            public bool Edit(TEntity entity, bool autoSave = false)
            {
                _context.Entry(entity).State = EntityState.Modified;
                if (autoSave)
                    Save();


                return true;
            }

            public bool EditBySetValues(TEntity old, TEntity newVals, bool autoSave = false)
            {
                _context.Entry(old).CurrentValues.SetValues(newVals);
                if (autoSave)
                    Save();


                return true;
            }

            public void MarkAsModified(TEntity entity, bool autoSave = false)
            {
                dbSet.Attach(entity);
                _context.Entry<TEntity>(entity).State = EntityState.Modified;
                if (autoSave)
                    Save();


            }

            public bool Delete(bool autoSave = false, params object[] keyValues)
            {
                var entity = dbSet.Find(keyValues);
                if (entity != null)
                {
                    dbSet.Remove(entity);
                    if (autoSave)
                        Save();


                    return true;
                }
                else
                    return false;
            }

            public bool Delete(TEntity entity, bool autoSave = false)
            {
                dbSet.Remove(entity);
                if (autoSave)
                    Save();


                return true;
            }

            #endregion Add , Modify , Edit , Delete One


            #region Add , Modify , Edit , Delete ,MarkAsModified "Attach and Update " One Rang / List<Object> 
            public int AddRange(IEnumerable<TEntity> entities, bool autoSave = false)
            {
                dbSet.AddRange(entities);
                if (autoSave)
                    Save();


                return entities.Count();
            }

            public int DeleteRange(IEnumerable<TEntity> entities, bool autoSave = false)
            {

                dbSet.RemoveRange(entities);
                if (autoSave)
                    Save();


                return entities.Count();
            }

            public bool EditRange(IEnumerable<TEntity> entities, bool autoSave = false)
            {
                foreach (TEntity entity in entities)
                    _context.Entry(entity).State = EntityState.Modified;
                if (autoSave)
                    Save();


                return true;
            }

            #endregion Add , Modify , Edit , Delete ,MarkAsModified "Attach and Update " One Rang / List<Object> 

            #region Count 
            public int GetCount()
            {
                return dbSet.Count();
            }

            public int GetCount(Expression<Func<TEntity, bool>> predicate)
            {
                return dbSet.Count(predicate);
            }

            #endregion

            #region Save And Deactivate Or Set Flag To True 

            public bool SoftDetlete(string deActivatePropName = "IsActive", bool autoSave = false, bool setValueTo = true, params object[] keyValues)
            {
                TEntity entity = dbSet.Find(keyValues);
                if (entity != null)
                {

                    Type t = entity.GetType();
                    var isDeletedProp = t.GetProperty(deActivatePropName, System.Reflection.BindingFlags.IgnoreCase);
                    isDeletedProp.SetValue(entity, setValueTo);
                    Edit(entity);
                    if (autoSave)
                        Save();


                    return true;
                }
                else
                    return false;

            }



            public void Save()
            {
                _context.SaveChanges();

            }

            #endregion Save And Deactivate Or Set Flag To True 

            #endregion Sync

            #region Asynchronous 

            #region GET 
            public async Task<TEntity> GetByIDAsync(params object[] keyValues)
            {
                return await dbSet.FindAsync(keyValues);
            }
            public async Task<IEnumerable<TEntity>> GetAllAsync()
            {
                IEnumerable<TEntity> query = await dbSet.ToListAsync();
                return query;
            }

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
            public virtual async Task<IEnumerable<TEntity>> GetEntitiesAsync(string ToIncludeDSets = "", Expression<Func<TEntity, bool>> filter = null,
                                                              Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
            {
                // Prepare The Type Of Entity DBSet => Select * from TEntity
                IQueryable<TEntity> dataQuery = dbSet;
                // If Filter Condition Not Null Filter  Add The Where Statement Predicate  
                #region Where 
                if (filter != null)
                    dataQuery = dataQuery.Where(filter);
                #endregion Where 
                // If The ToIncludeDSets String Contains Many DBSets / Properties To Include Separated By , Comma 
                #region Include
                foreach (var propSet in ToIncludeDSets.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    try { dataQuery = dataQuery.Include(propSet); }
                    // No Need To Handle This Error Just Skip To Next Prop.
                    catch { }
                #endregion Include
                // Finally Order By Statement 
                #region Order By 
                if (orderBy != null)
                    return await orderBy(dataQuery).ToListAsync();

                #endregion Order By
                return await dataQuery.ToListAsync();

            }


            #endregion

            #region Find 

            public async Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
            {
                IEnumerable<TEntity> query = await dbSet.Where(predicate).ToListAsync();
                return query;
            }

            public async Task<TEntity> FindByFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
            {
                TEntity query = await dbSet.FirstOrDefaultAsync(predicate);
                return query;
            }

            #endregion


            #region Add , Modify , Edit , Delete ,MarkAsModified "Attach and Update " One Object 


            public async Task<TEntity> AddAsync(TEntity entity, bool autoSave = false)
            {

                var result = await dbSet.AddAsync(entity);
                TEntity resultObj = result.Entity;
                if (autoSave)
                    Save();


                return resultObj;
            }


            #endregion Add , Modify , Edit , Delete One

            #region Add , Modify , Edit , Delete ,MarkAsModified "Attach and Update " One Rang / List<Object> 
            public async Task<int> AddRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false)
            {
                await dbSet.AddRangeAsync(entities);
                if (autoSave)
                    Save();


                return entities.Count();
            }


            public async Task<bool> EditRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false)
            {
                foreach (TEntity entity in entities)
                    _context.Entry(entity).State = EntityState.Modified;
                if (autoSave)
                    await SaveAsync();


                return true;
            }

            #endregion Add , Modify , Edit , Delete ,MarkAsModified "Attach and Update " One Rang / List<Object> 

            #region Count 
            public async Task<int> GetCountAsync()
            {
                return await dbSet.CountAsync();
            }

            public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate)
            {
                return await dbSet.CountAsync(predicate);
            }

            #endregion

            #region Save And Deactivate Or Set Flag To True 

            public async Task<bool> SoftDetleteAsync(string deActivatePropName = "IsActive", bool autoSave = false, bool setValueTo = true, params object[] keyValues)
            {
                TEntity entity = await dbSet.FindAsync(keyValues);
                if (entity != null)
                {

                    Type t = entity.GetType();
                    var isDeletedProp = t.GetProperty(deActivatePropName, System.Reflection.BindingFlags.IgnoreCase);
                    isDeletedProp.SetValue(entity, setValueTo);
                    Edit(entity);
                    if (autoSave)
                        await SaveAsync();


                    return true;
                }
                else
                    return false;

            }



            public async Task SaveAsync()
            {
                await _context.SaveChangesAsync();

            }

            #endregion Save And Deactivate Or Set Flag To True 

            #endregion Sync

            #region IDisposable
            private bool disposed = false;
            protected virtual void Dispose(bool disposing)
            {
                if (!this.disposed)
                {
                    if (disposing)
                    {

                        _context.Dispose();
                    }
                }
                this.disposed = true;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }


            #endregion



        }
}
