using Wpf.MvvmLight.SelfHost.IServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Data;
using SqlSugar;
using Wpf.MvvmLight.SelfHost.IRepository.Base;
using Wpf.MvvmLight.SelfHost.Repository.DBSeed;

namespace Wpf.MvvmLight.SelfHost.Services
{
  public class BaseServices<TEntity> : SQLiteDbContext, IBaseServices<TEntity> where TEntity : class, new()
  {
    public virtual IBaseRepository<TEntity> BaseDb { get; set; }

    public BaseServices(IBaseRepository<TEntity> baseDb = null)
    {
      this.BaseDb = BaseDb;
    }

    public async Task<int> Add(TEntity model)
    {
      return await BaseDb.Add(model);
    }

    public async Task<int> Add(List<TEntity> lisT)
    {
      return await BaseDb.Add(lisT);
    }

    public async Task AddOrUpdate(List<TEntity> lisT, Expression<Func<TEntity, object>> expression)
    {
      await BaseDb.AddOrUpdate(lisT, expression);
    }

    public async Task AddOrUpdate(TEntity entity, Expression<Func<TEntity, object>> expression)
    {
      await BaseDb.AddOrUpdate(entity, expression);
    }

    public async Task<bool> Delete(TEntity model)
    {
      return await BaseDb.Delete(model);
    }

    public async Task<bool> DeleteById(object id)
    {
      return await BaseDb.DeleteById(id);
    }

    public async Task<bool> DeleteByIds(object[] ids)
    {
      return await BaseDb.DeleteByIds(ids);
    }

    public async Task<List<TEntity>> Query()
    {
      return await BaseDb.Query();
    }

    public async Task<List<TEntity>> Query(string strWhere)
    {
      return await BaseDb.Query(strWhere);
    }

    public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
    {
      return await BaseDb.Query(whereExpression);
    }

    public Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression)
    {
      return BaseDb.Query<TResult>(expression);
    }

    public Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression, Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
    {
      return BaseDb.Query<TResult>(expression, whereExpression, strOrderByFileds);
    }

    public Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
    {
      return BaseDb.Query(whereExpression, strOrderByFileds);
    }

    public Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
    {
      return BaseDb.Query(whereExpression, connStr);
    }

    public Task<List<TEntity>> Query(string strWhere, string strOrderByFileds)
    {
      return BaseDb.Query(strWhere, strOrderByFileds);
    }

    public Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int intTop, string strOrderByFileds)
    {
      return BaseDb.Query(whereExpression, intTop, strOrderByFileds);
    }

    public Task<List<TEntity>> Query(string strWhere, int intTop, string strOrderByFileds)
    {
      return BaseDb.Query(strWhere, intTop, strOrderByFileds);
    }

    public Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex, int intPageSize, string strOrderByFileds)
    {
      return BaseDb.Query(whereExpression, intPageIndex, intPageSize, strOrderByFileds);
    }

    public Task<List<TEntity>> Query(string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds)
    {
      return BaseDb.Query(strWhere, intPageIndex, intPageSize, strOrderByFileds);
    }

    public Task<TEntity> QueryById(object objId)
    {
      return BaseDb.QueryById(objId);
    }

    public Task<TEntity> QueryById(object objId, bool blnUseCache = false)
    {
      return BaseDb.QueryById(objId, blnUseCache);
    }

    public Task<List<TEntity>> QueryByIDs(object[] lstIds)
    {
      return BaseDb.QueryByIDs(lstIds);
    }

    public Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(Expression<Func<T, T2, T3, object[]>> joinExpression, Expression<Func<T, T2, T3, TResult>> selectExpression, Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new()
    {
      return BaseDb.QueryMuch<T, T2, T3, TResult>(joinExpression, selectExpression, whereLambda);
    }

    public Task<List<TEntity>> QuerySql(string strSql, SugarParameter[] parameters = null)
    {
      return BaseDb.QuerySql(strSql, parameters);
    }

    public Task<DataTable> QueryTable(string strSql, SugarParameter[] parameters = null)
    {
      return BaseDb.QueryTable(strSql, parameters);
    }

    public Task<bool> Update(TEntity model)
    {
      return BaseDb.Update(model);
    }

    public Task<bool> Update(TEntity entity, string strWhere)
    {
      return BaseDb.Update(entity, strWhere);
    }

    public Task<bool> Update(object operateAnonymousObjects)
    {
      return BaseDb.Update(operateAnonymousObjects);
    }

    public Task<bool> Update(TEntity entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "")
    {
      return BaseDb.Update(entity, lstColumns, lstIgnoreColumns, strWhere);
    }

    public bool UpdateFiled(Expression<Func<TEntity, TEntity>> columsExpression, Expression<Func<TEntity, bool>> whereExpression)
    {
      return BaseDb.UpdateFiled(columsExpression, whereExpression);
    }
  }
}
