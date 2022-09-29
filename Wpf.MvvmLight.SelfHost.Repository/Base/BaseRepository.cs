using Wpf.MvvmLight.SelfHost.IRepository.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Wpf.MvvmLight.SelfHost.Repository.Base
{
  public class BaseRepository<TEntity> : SimpleClient<TEntity>, IBaseRepository<TEntity> where TEntity : class, new()
  {
    public BaseRepository(ISqlSugarClient context = null) : base(context)
    {
      Context = context;
    }

    public ISqlSugarClient Db => Context;

    public async Task<int> Add(TEntity model)
    {
      return await Context.Insertable<TEntity>(model).ExecuteCommandAsync();
    }

    public async Task<int> Add(List<TEntity> listEntity)
    {
      return await Context.Insertable(listEntity).ExecuteCommandAsync();
    }

    public async Task AddOrUpdate(List<TEntity> listEntity, Expression<Func<TEntity, object>> expression)
    {
      var executor = Context.Storageable(listEntity).WhereColumns(expression).ToStorage();
      await executor.AsInsertable.ExecuteCommandAsync();
      await executor.AsUpdateable.ExecuteCommandAsync();
    }

    public async new Task<bool> Delete(TEntity model)
    {
      return await Context.Deleteable<TEntity>().Where(model).ExecuteCommandHasChangeAsync();
    }

    public async new Task<bool> DeleteById(object id)
    {
      return await Context.Deleteable<TEntity>().In(id).ExecuteCommandHasChangeAsync();
    }

    public async new Task<bool> DeleteByIds(object[] ids)
    {
      return await Context.Deleteable<TEntity>().In(ids).ExecuteCommandHasChangeAsync();
    }

    public async Task<List<TEntity>> Query()
    {
      return await Context.Queryable<TEntity>().ToListAsync();
    }

    public async Task<List<TEntity>> Query(string strWhere)
    {
      return await Context.Queryable<TEntity>().Where(strWhere).ToListAsync();
    }

    public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
    {
      return await Context.Queryable<TEntity>().Where(whereExpression).ToListAsync();
    }

    public async Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression)
    {
      return await Context.Queryable<TEntity>().Select(expression).ToListAsync();
    }

    public async Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression, Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
    {
      return await Context.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).Select(expression).ToListAsync();
    }

    public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
    {
      return await Context.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).OrderByIF(strOrderByFileds != null, strOrderByFileds).ToListAsync();
    }

    public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
    {
      return await Context.Queryable<TEntity>().OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToListAsync();
    }

    public async Task<List<TEntity>> Query(string strWhere, string strOrderByFileds)
    {
      return await Context.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToListAsync();
    }

    public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int intTop, string strOrderByFileds)
    {
      return await Context.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).Take(intTop).ToListAsync();
    }

    public async Task<List<TEntity>> Query(string strWhere, int intTop, string strOrderByFileds)
    {
      return await Context.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).Take(intTop).ToListAsync();
    }

    public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex, int intPageSize, string strOrderByFileds)
    {
      return await Context.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).ToPageListAsync(intPageIndex, intPageSize);
    }

    public async Task<List<TEntity>> Query(string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds)
    {
      return await Context.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToPageListAsync(intPageIndex, intPageSize);
    }

    public async Task<TEntity> QueryById(object objId)
    {
      return await Context.Queryable<TEntity>().In(objId).SingleAsync();
    }

    public async Task<TEntity> QueryById(object objId, bool blnUseCache = false)
    {
      return await Context.Queryable<TEntity>().WithCacheIF(blnUseCache).In(objId).SingleAsync();
    }

    public async Task<List<TEntity>> QueryByIDs(object[] lstIds)
    {
      return await Context.Queryable<TEntity>().In(lstIds).ToListAsync();
    }

    public async Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(Expression<Func<T, T2, T3, object[]>> joinExpression, Expression<Func<T, T2, T3, TResult>> selectExpression, Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new()
    {
      if (whereLambda == null)
      {
        return await Context.Queryable(joinExpression).Select(selectExpression).ToListAsync();
      }
      return await Context.Queryable(joinExpression).Where(whereLambda).Select(selectExpression).ToListAsync();
    }

    public async Task<List<TEntity>> QuerySql(string strSql, SugarParameter[] parameters = null)
    {
      return await Context.Ado.SqlQueryAsync<TEntity>(strSql, parameters);
    }

    public async Task<DataTable> QueryTable(string strSql, SugarParameter[] parameters = null)
    {
      return await Context.Ado.GetDataTableAsync(strSql, parameters);
    }

    public async new Task<bool> Update(TEntity model)
    {
      return await Context.Updateable(model).ExecuteCommandHasChangeAsync();
    }

    public async Task<bool> Update(TEntity entity, string strWhere)
    {
      return await Context.Updateable(entity).Where(strWhere).ExecuteCommandHasChangeAsync();
    }

    public async Task<bool> Update(object operateAnonymousObjects)
    {
      return await Context.Updateable<TEntity>(operateAnonymousObjects).ExecuteCommandAsync() > 0;
    }

    public async Task<bool> Update(TEntity entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "")
    {
      IUpdateable<TEntity> up = Context.Updateable(entity);
      if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
      {
        up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
      }
      if (lstColumns != null && lstColumns.Count > 0)
      {
        up = up.UpdateColumns(lstColumns.ToArray());
      }
      if (!string.IsNullOrEmpty(strWhere))
      {
        up = up.Where(strWhere);
      }
      return await up.ExecuteCommandHasChangeAsync();
    }

    public bool UpdateFiled(Expression<Func<TEntity, TEntity>> columsExpression, Expression<Func<TEntity, bool>> whereExpression)
    {
      return base.Update(columsExpression, whereExpression);
    }
  }
}
