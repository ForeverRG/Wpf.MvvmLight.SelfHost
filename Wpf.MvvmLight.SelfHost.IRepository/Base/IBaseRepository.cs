﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Wpf.MvvmLight.SelfHost.IRepository.Base
{
  public interface IBaseRepository<TEntity> where TEntity : class
  {
    /// <summary>
    /// SqlsugarClient实体
    /// </summary>
    ISqlSugarClient Db { get; }
    /// <summary>
    /// 根据Id查询实体
    /// </summary>
    /// <param name="objId"></param>
    /// <returns></returns>
    Task<TEntity> QueryById(object objId);
    Task<TEntity> QueryById(object objId, bool blnUseCache = false);
    /// <summary>
    /// 根据id数组查询实体list
    /// </summary>
    /// <param name="lstIds"></param>
    /// <returns></returns>
    Task<List<TEntity>> QueryByIDs(object[] lstIds);

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<int> Add(TEntity model);

    /// <summary>
    /// 批量添加
    /// </summary>
    /// <param name="listEntity"></param>
    /// <returns></returns>
    Task<int> Add(List<TEntity> listEntity);

    /// <summary>
    /// 根据id 删除某一实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteById(object id);

    /// <summary>
    /// 根据对象，删除某一实体
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> Delete(TEntity model);

    /// <summary>
    /// 根据id数组，删除实体list
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<bool> DeleteByIds(object[] ids);

    /// <summary>
    /// 更新model
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> Update(TEntity model);

    /// <summary>
    /// 根据model，更新，带where条件
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="strWhere"></param>
    /// <returns></returns>
    Task<bool> Update(TEntity entity, string strWhere);
    Task<bool> Update(object operateAnonymousObjects);

    /// <summary>
    /// 根据model，更新，指定列
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="lstColumns"></param>
    /// <param name="lstIgnoreColumns"></param>
    /// <param name="strWhere"></param>
    /// <returns></returns>
    Task<bool> Update(TEntity entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "");

    /// <summary>
    /// 更新指定列
    /// </summary>
    /// <param name="columsExpression"></param>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    bool UpdateFiled(Expression<Func<TEntity, TEntity>> columsExpression, Expression<Func<TEntity, bool>> whereExpression);

    /// <summary>
    /// 查询
    /// </summary>
    /// <returns></returns>
    Task<List<TEntity>> Query();

    /// <summary>
    /// 带sql where查询
    /// </summary>
    /// <param name="strWhere"></param>
    /// <returns></returns>
    Task<List<TEntity>> Query(string strWhere);

    /// <summary>
    /// 根据表达式查询
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression);

    /// <summary>
    /// 根据表达式，指定返回对象模型，查询
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression);

    /// <summary>
    /// 根据表达式，指定返回对象模型，排序，查询
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="expression"></param>
    /// <param name="whereExpression"></param>
    /// <param name="strOrderByFileds"></param>
    /// <returns></returns>
    Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression, Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds);
    Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds);
    Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);
    Task<List<TEntity>> Query(string strWhere, string strOrderByFileds);

    Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int intTop, string strOrderByFileds);
    Task<List<TEntity>> Query(string strWhere, int intTop, string strOrderByFileds);
    Task<List<TEntity>> QuerySql(string strSql, SugarParameter[] parameters = null);
    Task<DataTable> QueryTable(string strSql, SugarParameter[] parameters = null);

    Task<List<TEntity>> Query(
        Expression<Func<TEntity, bool>> whereExpression, int intPageIndex, int intPageSize, string strOrderByFileds);
    Task<List<TEntity>> Query(string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds);

    /// <summary>
    /// 三表联查
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="joinExpression"></param>
    /// <param name="selectExpression"></param>
    /// <param name="whereLambda"></param>
    /// <returns></returns>
    Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(
        Expression<Func<T, T2, T3, object[]>> joinExpression,
        Expression<Func<T, T2, T3, TResult>> selectExpression,
        Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new();

    /// <summary>
    /// 插入或更新
    /// </summary>
    /// <param name="listEntity"></param>
    /// <returns></returns>
    Task AddOrUpdate(List<TEntity> listEntity, Expression<Func<TEntity, object>> expression);

    /// <summary>
    /// 插入或更新
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="expression"></param>
    /// <returns></returns>
    Task AddOrUpdate(TEntity entity, Expression<Func<TEntity, object>> expression);
  }
}
