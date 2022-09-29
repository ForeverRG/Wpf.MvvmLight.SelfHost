using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.IServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;

namespace Wpf.MvvmLight.SelfHost.Services
{
  public class JigServices : BaseServices<Jig>, IJigServices
  {
    /// <summary>
    /// Jig获取配置
    /// </summary>
    /// <returns></returns>
    public async Task<IList<Jig>> GetSettings()
    {
      return await JigDb.Query();
    }

    /// <summary>
    /// 根据条件获取配置
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public async Task<IList<Jig>> GetSettings(Expression<Func<Jig, bool>> expression)
    {
      return (await JigDb.Query(expression)).ToList();
    }

    /// <summary>
    /// Jig保存配置
    /// </summary>
    public async Task SaveSettings(IList<Jig> jigs)
    {
      var settings = await GetSettings();
      if (settings.Count == 0)
      {
        await JigDb.Add(jigs.ToList());
      }
      else
      {
        await JigDb.Update(jigs);
      }
    }

    /// <summary>
    /// Jig保存配置
    /// </summary>
    public async Task SaveSettings(Jig jig)
    {
      var settings = (await GetSettings(j => j.Type == jig.Type && j.Number == jig.Number && j.ModuleType == jig.ModuleType)).FirstOrDefault();
      if (settings != null)
      {
        jig.Id = settings.Id;
        await JigDb.Update(jig);
      }
      else
      {
        await JigDb.Add(jig);
      }
    }

    /// <summary>
    /// 插入或更新配置
    /// </summary>
    /// <param name="configs"></param>
    /// <returns></returns>
    public async Task SaveSettings(List<Jig> jigs, Expression<Func<Jig, object>> expression)
    {
      await JigDb.AddOrUpdate(jigs, expression);
    }

    /// <summary>
    /// 更新夹具部分字段
    /// </summary>
    /// <param name="columsExpression"></param>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    public bool UpdateJigField(Expression<Func<Jig, Jig>> columsExpression, Expression<Func<Jig, bool>> whereExpression)
    {
      return JigDb.UpdateFiled(columsExpression, whereExpression);
    }
  }
}

